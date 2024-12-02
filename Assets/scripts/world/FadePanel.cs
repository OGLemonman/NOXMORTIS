using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour {
    public Image panelImage;
    private IEnumerator coroutine;

    public void Fade(float alpha, float duration) {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        coroutine = FadeToAlpha(alpha, duration);
        StartCoroutine(coroutine);
    }

    IEnumerator FadeToAlpha(float alpha, float duration) {
        Color startColor = panelImage.color;
        Color endColor = panelImage.color;
        endColor.a = alpha;

        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.unscaledDeltaTime;
            panelImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        panelImage.color = endColor;
    }
}