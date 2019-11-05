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
    public string nextScene;
    public float timeToFadeIn;
    public float timeTillFadeOut;
    public float timeToFadeOut;
    public float timeTillNextScene;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        splashImage.canvasRenderer.SetAlpha(0.0f);
        FadeIn();
        yield return new WaitForSeconds(timeTillFadeOut);
        FadeOut();
        yield return new WaitForSeconds(timeTillNextScene);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f, timeToFadeIn, false);
    }

    void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, timeToFadeOut, false);
    }
}
