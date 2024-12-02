using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour {
    public float secondsPerDay = 300f;
    public float dayEndFadeInTime = 4f;
    public float dayStartFadeOutTime = 2f;
    public FadePanel fadePanel;
    public Text dayAnnouncementText;
    public AudioClip keyPressAudio;
    public AudioClip sirenAudio;
    public AudioSource uiSource;
    public MobSpawner mobSpawner;
    public int spawnCount;
    public Transform player;
    public Bounds houseBounds;
    private int currentDay = 0;

    void Start() {
        StartCoroutine(AdvanceDay());
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(houseBounds.center, houseBounds.extents);
    }

    IEnumerator AdvanceDay() {
        // Pause game and fade panel in
        Time.timeScale = 0f;
        fadePanel.Fade(1, dayEndFadeInTime);

        yield return new WaitForSecondsRealtime(dayEndFadeInTime + 0.5f);

        // Despawn mobs and increment day
        mobSpawner.DespawnMobs();
        currentDay++;

        // Display cinematic day text
        dayAnnouncementText.text = "";

        Color textColor = dayAnnouncementText.color;
        textColor.a = 1;
        dayAnnouncementText.color = textColor;

        string dayText = "DAY " + currentDay.ToString();

        for (int i = 0; i < dayText.Length; i++) {
            dayAnnouncementText.text = dayText.Substring(0, i+1);
            uiSource.pitch = Random.Range(0.9f, 1.1f);
            uiSource.PlayOneShot(keyPressAudio);

            yield return new WaitForSecondsRealtime(0.15f);
        }

        yield return new WaitForSecondsRealtime(2f);

        // Player win condition
        if (currentDay == 6) {
            yield break;            
        }

        // Check player is inside their house
        if (currentDay > 0) {
            if (houseBounds.Contains(player.position)) {
                Debug.Log("Safe!");
            } else {
                // Kill player
                Debug.Log("Dead");
            }
        }

        // Subtract Hunger & Thirst
        playerstats playerStats = player.gameObject.GetComponent<playerstats>();
        playerStats.currentHunger -= 20f;
        playerStats.currentWater -= 20f;

        // Fade day text out
        Color startColor = dayAnnouncementText.color;
        Color endColor = startColor;
        endColor.a = 0;

        float elapsedTime = 0f;

        while (elapsedTime < 2f) {
            elapsedTime += Time.unscaledDeltaTime;
            dayAnnouncementText.color = Color.Lerp(startColor, endColor, elapsedTime / 2f);
            yield return null;
        }

        dayAnnouncementText.color = endColor;
        yield return new WaitForSecondsRealtime(1.5f);

        // Spawn mobs
        for (int i = 0; i < spawnCount; i++) {
            mobSpawner.GenerateMob(currentDay);
        }

        // Resume game and fade panel out
        Time.timeScale = 1f;
        fadePanel.Fade(0, dayStartFadeOutTime);

        // New day
        float dayElapsedTime = 0f;
        bool playedWarning = false;

        while (dayElapsedTime < secondsPerDay) {
            dayElapsedTime += Time.unscaledDeltaTime;

            if (!playedWarning && dayElapsedTime > secondsPerDay * 0.75f) {
                playedWarning = true;
                uiSource.pitch = 1f;
                uiSource.PlayOneShot(sirenAudio);
            }

            yield return null;
        }

        yield return AdvanceDay();
    }
}