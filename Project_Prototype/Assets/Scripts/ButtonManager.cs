using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject startButton, exitButton;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == startButton)
                    SceneManager.LoadScene("Map01", LoadSceneMode.Single);

                else if (hit.collider.gameObject == exitButton)
                    Application.Quit();
            }
        }
    }
}
