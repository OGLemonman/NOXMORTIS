using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DayCycle : MonoBehaviour {
    public float secondsPerDay = 300f;
    public float dayEndFadeInTime = 4f;
    public float dayStartFadeOutTime = 2f;
    public FadePanel fadePanel;
    public Text dayAnnouncementText;
    public TMP_Text dayDisplay;
    public TMP_Text timeDisplay;
    public AudioClip keyPressAudio;
    public AudioClip sirenAudio;
    public AudioClip diedAudio;
    public AudioClip winAudio;
    public AudioSource uiSource;
    public MobSpawner mobSpawner;
    public int spawnCount;
    public Transform balcony;
    public Transform player;
    public Bounds houseBounds;
    public AudioSource radio;
    public AudioClip[] radioAudios;
    public TMP_Text warningText; 
    public RectTransform diedPanel;
    public RectTransform winPanel;
    public GameObject craneOld;
    public GameObject craneNew;
    private int currentDay = 0;
    private float dayTimeElapsed;
    private IEnumerator dayCoroutine;
    private playerstats playerStats;

    void Start() {
        playerStats = player.GetComponent<playerstats>();
        Cursor.lockState = CursorLockMode.Locked;
        craneOld.SetActive(true);
        craneNew.SetActive(false);
        dayCoroutine = AdvanceDay();
        StartCoroutine(dayCoroutine);
    }

    void Update() {
        if (currentDay != 6) {
            dayTimeElapsed += Time.deltaTime * (54000 / secondsPerDay);

            int hours = (int)dayTimeElapsed / 3600;
            int minutes = ((int)dayTimeElapsed % 3600) / 60;

            string formattedTime = $"{hours:D2}:{minutes:D2}";
            timeDisplay.text = formattedTime;
        }

        if (currentDay >= 4) {
            craneOld.SetActive(false);
            craneNew.SetActive(true);
        }

        if (playerStats.currentHP <= 0 && !diedPanel.gameObject.activeSelf) {
            Died();
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(houseBounds.center, houseBounds.extents);
    }

    public void Died() {
        StopCoroutine(dayCoroutine);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        diedPanel.gameObject.SetActive(true);
        uiSource.PlayOneShot(diedAudio);
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator Win() {
        Cursor.lockState = CursorLockMode.None;
        uiSource.PlayOneShot(winAudio);
        yield return new WaitForSecondsRealtime(8);
        winPanel.gameObject.SetActive(true);
    }

    IEnumerator AdvanceDay() {
        // Pause game and fade panel in
        Time.timeScale = 0f;
        fadePanel.Fade(1, dayEndFadeInTime);

        yield return new WaitForSecondsRealtime(dayEndFadeInTime + 0.5f);

        // Despawn mobs and increment day
        mobSpawner.DespawnMobs();
        currentDay++;
        dayTimeElapsed = 28800f; 

        dayDisplay.text = "DAY: " + currentDay.ToString();

        // Player win condition
        if (currentDay == 6) {
            yield return Win();
            yield break;            
        }

        // Check player is inside their house
        if (currentDay > 1) {
            if (houseBounds.Contains(player.position)) {
                Debug.Log("Safe!");
            } else {
                // Kill player
                Died();
                yield break;
            }
        }

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

        // Place Character
        player.position = balcony.position;
        player.LookAt(player.position + balcony.forward);

        // Subtract Hunger & Thirst
        playerstats playerStats = player.gameObject.GetComponent<playerstats>();
        playerStats.currentHunger -= 20f;
        playerStats.currentWater -= 20f;
        if (playerStats.currentHP <= 70) {
            playerStats.currentHP += 30;
        }

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

        // Radio
        AudioClip radioAudio = radioAudios[currentDay-1];
        radio.PlayOneShot(radioAudio);

        // New day
        float dayElapsedTime = 0f;
        bool playedWarning = false;

        while (dayElapsedTime < secondsPerDay) {
            dayElapsedTime += Time.unscaledDeltaTime;

            if (!playedWarning && dayElapsedTime > secondsPerDay * 0.75f) {
                playedWarning = true;
                uiSource.pitch = 1f;
                uiSource.PlayOneShot(sirenAudio);

                warningText.gameObject.SetActive(true);
            } else if (playedWarning && dayElapsedTime > (secondsPerDay * 0.75f) + 2f) {
                warningText.gameObject.SetActive(false);
            }

            yield return null;
        }

        yield return AdvanceDay();
    }
}