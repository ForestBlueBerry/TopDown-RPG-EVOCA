using UnityEngine;
using UnityEngine.InputSystem;

public class ExitUIInGame : MonoBehaviour
{
    public GameObject exitUI;
    public GameObject bookUI;
    public GameObject mapUI;

  

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !bookUI.activeInHierarchy && !mapUI.activeInHierarchy)
        {
            OpenExUI();
        }
    }


    public void OpenExUI()
    {
        if (exitUI.activeInHierarchy)
        {
            exitUI.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            exitUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
