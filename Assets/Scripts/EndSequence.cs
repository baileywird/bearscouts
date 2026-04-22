
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndSequence : MonoBehaviour
{
    [SerializeField] Image fadePanel;
    [SerializeField] TMP_Text endText;
    public float fadeDuration = 2f;

    public void PlaySequence()
    {
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        //fade image
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        //fade text
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            endText.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}
