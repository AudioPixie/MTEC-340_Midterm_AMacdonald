using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueShift : MonoBehaviour
{
    private Color currentColor;
    private float fadeDuration = 1f;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0.4f, 0, 0.4f, 1);
        currentColor = sr.color;
    }

    public void ShiftColor(Color goTo)
    {
        currentColor = sr.color;
        StartCoroutine(Fade(currentColor, goTo));
    }

    private IEnumerator Fade(Color currentColor, Color goTo)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            sr.color = Color.Lerp(currentColor, goTo, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sr.color = goTo;
    }
}


