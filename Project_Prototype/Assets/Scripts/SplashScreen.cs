/*=============================================================================
 * Game:        Metallicide
 * Version:     Beta
 * 
 * Class:       SplashScreen.cs
 * Purpose:     To show a scene before starting the game:
 * 
 * Author:      Lachlan Wernert
 * Team:        Skylighter
 * 
 * Deficiences:
 * 
 *===========================================================================*/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [Header("Attributes")]
    public RawImage splashImage;
    public Image panel;
    public float timeToFadeIn;
    public float timeTillFadeOut;
    public float timeToFadeOut;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        splashImage.canvasRenderer.SetAlpha(0.0f);
        FadeIn();
        yield return new WaitForSeconds(timeTillFadeOut);
        FadeOut();
    }

    void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f, timeToFadeIn, false);
    }

    void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, timeToFadeOut, false);
        panel.CrossFadeAlpha(0.0f, timeToFadeOut, false);
    }
}
