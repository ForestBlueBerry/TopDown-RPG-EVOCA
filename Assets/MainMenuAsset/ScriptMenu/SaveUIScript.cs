using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class SaveUIScript : MonoBehaviour
{
    public GameObject mainUI;
    public AudioSource source;

    public TMPro.TextMeshProUGUI textMeshPro;
    private void Start()
    {
        FillTextSave();
    }
    public void LoadGame()
    {
        if(textMeshPro.text == "Empty slot") return;
        SaveManager.Instance.currentMode = SaveManager.GameMode.LoadGame;
        SceneManager.LoadScene("SampleScene");
    }
  public void OnBackToMain()
    {
        this.gameObject.SetActive(false);
        source.Pause();
        mainUI.SetActive(true);
    }

    private void OnEnable()
    {
        source.UnPause();
    }


    public void FillTextSave()
    {
        textMeshPro.text = GetFormattedPlayTime(GetSavePreview());
    }

    public string GetFormattedPlayTime(DataForSave data)
    {

        if (data == null) return "Empty slot";
        System.TimeSpan t = System.TimeSpan.FromSeconds(data.playtime);

        string playtimes = string.Format("{0:D2}h {1:D2}m", t.Hours + (t.Days * 24), t.Minutes);

        return $"{data.saveTime} || {playtimes}";
    }

    public DataForSave GetSavePreview()
    {
        string path = SaveManager.Instance.savePath;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            string decrypt = SaveManager.Instance.EncryptDecrypt(json);

            DataForSave data = JsonUtility.FromJson<DataForSave>(decrypt);
            return data;
        }
        return null;
    }



}
