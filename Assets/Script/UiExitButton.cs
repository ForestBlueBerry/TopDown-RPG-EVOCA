using UnityEngine;

public class UiExitButton : MonoBehaviour
{
    public void OnSaveButtonClick()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Save();
        }
    }
}

