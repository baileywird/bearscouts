
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class EndSequence : MonoBehaviour
{
    [SerializeField] Image fadePanel;
    [SerializeField] TMP_Text endText;
    [SerializeField] GameObject endPopup;
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
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        endPopup.SetActive(true);

        //fade text
        elapsed = 0f;
        yield return new WaitForSeconds(0.25f);

        endText.color = new Color(1, 1, 1, 0);
        endPopup.SetActive(true);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            endText.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}
