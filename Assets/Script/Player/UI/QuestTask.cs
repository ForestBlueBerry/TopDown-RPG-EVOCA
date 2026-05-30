using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestTask : MonoBehaviour
{

    public RequestTasks[] tasks;
    public int curtask = 0;
    public FreshDataUI forsave;

    void Start()
    {
        forsave = FindAnyObjectByType<FreshDataUI>();
        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.OnCheckProgressQuest += CheckTasks;
            tasks[0].fillimage.fillAmount = (float)PlayerInventory.Instance.storagePlayer[tasks[0].bottleElement] / (float)tasks[0].countelement;
        }
      
    }
    public void NextTasks(int numTask)
    {
        if (tasks[numTask].bottleElement == BottleElement.none && tasks[curtask].taskObject.activeInHierarchy && PlayerInventory.Instance.storagePlayer[tasks[curtask].bottleElement] >= tasks[curtask].countelement) {
            tasks[curtask].taskObject.SetActive(false);
            tasks[numTask].taskObject.SetActive(true);
            curtask = numTask;
            forsave.curtask = curtask;
            return;
        } 

        if (tasks[curtask].taskObject.activeInHierarchy && PlayerInventory.Instance.storagePlayer[tasks[curtask].bottleElement]>=tasks[curtask].countelement)
        {
            tasks[curtask].taskObject.SetActive(false);
            PlayerInventory.Instance.GetExp(20);
            tasks[numTask].taskObject.SetActive(true);
            tasks[numTask].fillimage.fillAmount = 0f;
            tasks[numTask].fillimage.fillAmount = (float)PlayerInventory.Instance.storagePlayer[tasks[numTask].bottleElement] / (float)tasks[numTask].countelement;
            curtask = numTask;
            forsave.curtask = curtask;
        }
    }

    public void CheckTasks(BottleElement bottleElement) {
        if (tasks[curtask].bottleElement == bottleElement) {
            tasks[curtask].fillimage.fillAmount = (float)PlayerInventory.Instance.storagePlayer[tasks[curtask].bottleElement] / (float)tasks[curtask].countelement;
        } 
    }
    public void UpdateProgressTask()
    {
        if (tasks[curtask].fillimage != null) tasks[curtask].fillimage.fillAmount = (float)PlayerInventory.Instance.storagePlayer[tasks[curtask].bottleElement] / (float)tasks[curtask].countelement;
    }

    private void OnDestroy()
    {
        PlayerInventory.Instance.OnCheckProgressQuest -= CheckTasks;
    }


    ///SAVE
    public void LoadTasks(int level)
    {
        tasks[curtask].taskObject.SetActive(false);
        tasks[level].taskObject.SetActive(true);
  if(tasks[level].fillimage != null) tasks[level].fillimage.fillAmount = (float)PlayerInventory.Instance.storagePlayer[tasks[level].bottleElement] / (float)tasks[level].countelement;
        curtask = level;
    }
    private void OnEnable()
    {
        UpdateProgressTask();

        if (forsave == null)
            forsave = FindFirstObjectByType<FreshDataUI>(FindObjectsInactive.Include);

        if (forsave != null)
        {
            LoadTasks(forsave.curtask);
        }
    }

}
[System.Serializable]
public struct RequestTasks
{
    public GameObject taskObject;
    public BottleElement bottleElement;
    public int countelement;
    public Image fillimage;

}