using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    [Header("Attributes")]
    private Image panel;
    public float timeToFadeIn;
    public float timeTillFadeOut;
    public float timeToFadeOut;
    public bool fadeIn = true;
    public bool fadeOut = true;
    public bool shouldWait = true;
    private bool hasFinished = false;

    private void Awake()
    {
        panel = this.GetComponent<Image>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if(fadeIn)
        {
            panel.canvasRenderer.SetAlpha(0.0f);
            FadeIn();
        }

        else if(fadeOut)
            panel.canvasRenderer.SetAlpha(1.0f);

        if(shouldWait)
            yield return new WaitForSeconds(timeTillFadeOut);

        if(fadeOut)
            FadeOut();
    }

    private void Update()
    {
        if(panel.canvasRenderer.GetAlpha() == 1.0f && fadeIn)
        {
            hasFinished = true;
        }

        if (panel.canvasRenderer.GetAlpha() == 0.0f && fadeOut)
        {
            hasFinished = false;
        }
    }

    void FadeIn()
    {
        panel.CrossFadeAlpha(1.0f, timeToFadeIn, false);
    }

    void FadeOut()
    {
        panel.CrossFadeAlpha(0.0f, timeToFadeOut, false);
    }

    public bool HasFinished()
    {
        return hasFinished;
    }
}
