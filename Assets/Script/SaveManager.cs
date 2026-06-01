
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using System;


public class SaveManager : MonoBehaviour
{
   

    public PlayerHealth health;
    public PlayerAttack attack;
    public PlayerController controller;
    public PlayerInventory inventory;
    public BookUI book;
    public QuestTask questTask;
    public FreshDataUI freshUI;

    public CharacterDatabase characterDatabase;
    public string savePath;

    public static SaveManager Instance;

    public GameMode currentMode;

    private float sessionStartTime;
    public float totalPlayTimeBeforeSession;

    public IdleProgression idleProgression;


    void Awake()
    {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                savePath = Path.Combine(Application.persistentDataPath, "save.json");
            }
            else
            {
                Destroy(gameObject); 
            }
            Time.timeScale = 1.0f;  
    }

    public void Save()
    {
        FindReferences();
        DataForSave data = new DataForSave();
        data.SaveModifier(health, attack, controller,inventory, freshUI);
        data.saveTime = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        data.playtime = totalPlayTimeBeforeSession + (Time.time - sessionStartTime);
        data.completerewardexp = idleProgression.completerewardexp;
        string json = JsonUtility.ToJson(data, true);
        string encrypted = EncryptDecrypt(json);
        File.WriteAllText(savePath, encrypted);
        SceneManager.LoadScene("MainMenu");
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            string decrypted = EncryptDecrypt(json);

            DataForSave data = JsonUtility.FromJson<DataForSave>(decrypted);
            FindSO(data.idCharacter);
            data.LoadModifier(health, attack, controller, inventory, freshUI, questTask);
            sessionStartTime = Time.time;
            totalPlayTimeBeforeSession =data.playtime;
         
            idleProgression.LoadExpAfterRest(data.saveTime);
        }
    }
    public void FindSO(int idCharacter)
    {
        PlayerSO playerSO = characterDatabase.character.FirstOrDefault(so => so.idCharacter == idCharacter);
        Debug.Log(playerSO != null);
        if (playerSO != null) controller.SetupSO(playerSO);
    }

    public enum GameMode
    {
        NewGame,LoadGame
    }
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
        {
            if (currentMode == GameMode.NewGame)
            {
                totalPlayTimeBeforeSession = 0f;
                freshUI.curtask = 0;
                freshUI.currentMutationPage = 0;    
            }
            sessionStartTime = Time.time;
            if (currentMode == GameMode.LoadGame)
            {
                
                StartCoroutine(LoadRoutine());  
            }
        }
    }
    public void FindReferences()
    {
        health = FindFirstObjectByType<PlayerHealth>(FindObjectsInactive.Include);
        attack = FindFirstObjectByType<PlayerAttack>(FindObjectsInactive.Include);
        controller = FindFirstObjectByType<PlayerController>(FindObjectsInactive.Include);
        inventory = FindFirstObjectByType<PlayerInventory>(FindObjectsInactive.Include);
        book = FindFirstObjectByType<BookUI>(FindObjectsInactive.Include);
        questTask = FindFirstObjectByType<QuestTask>(FindObjectsInactive.Include);
        idleProgression = FindFirstObjectByType<IdleProgression>(FindObjectsInactive.Include);
    }
    private System.Collections.IEnumerator LoadRoutine()
    {
        yield return null;
        FindReferences();
        if (currentMode == GameMode.LoadGame)
        {
            Load();
        }
        sessionStartTime = Time.time;
    }
    public string EncryptDecrypt(string data)
    {
        string key = "defaults"; 
        string result = "";
        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ key[i % key.Length]);
        }
        return result;
    }
}

