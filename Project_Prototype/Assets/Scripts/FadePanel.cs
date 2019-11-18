using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    public Image panel;
    public float timeToFadeIn;
    public float timeToFadeOut;

    private bool fadeOut, fadeIn;

    // Start is called before the first frame update
    void Start()
    {
        panel.canvasRenderer.SetAlpha(0.0f);
    }

    public void Fade()
    {

    }

    IEnumerator DoFade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha < 0)
        {
            canvasGroup.alpha += Time.deltaTime / 2;
            yield return null;
        }
        canvasGroup.interactable = false;
        yield return null;
    }

    private void Update()
    {
        if(fadeIn)
        {
            panel.CrossFadeAlpha(1.0f, timeToFadeIn, false);
            if (panel.material.color.a >= 1.0f)
                fadeIn = false;
        }
        else if (fadeOut)
        {
            panel.CrossFadeAlpha(0.0f, timeToFadeOut, false);
            if (panel.material.color.a <= 0.0f)
                fadeOut = false;
        }
    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }
}
