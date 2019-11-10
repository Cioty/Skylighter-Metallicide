using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
   public IEnumerator Shake (float duration, float magnitiude)
   {
       Vector3 originalPos = transform.localPosition;

       float elapsed = 0.0f;

       while( elapsed <duration)
       {
           float x = Random.Range(-1f, 1f) * magnitiude;
           float y = Random.Range(-1f, 1f) * magnitiude;

           transform.localPosition = new Vector3(x, y, originalPos.z);

           elapsed += Time.deltaTime;
           yield return null;
       }
       transform.localPosition = originalPos;
   }
}
