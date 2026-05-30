using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine.Rendering;
using System.Collections;
using System;

public class BookUI : MonoBehaviour
{
    public GameObject[] indexPageMutation ;
    public GameObject canvasIcon ;

    public PlayerController playerController ;

    //public RequestEvoElements[] requestElements;
    private Dictionary<BottleElement, int> requestBottleEvo = new Dictionary<BottleElement, int>();

    public FillTextMeshElement elementsTextMesh;

    private Dictionary<BottleElement,TextMeshProUGUI>fillStorageUI = new Dictionary<BottleElement, TextMeshProUGUI>();


    private int _pendingPageIndex = 0;
    private Animator _bookAnimator;
    public GameObject[] pageGeneralPages;
    private Animator animator;
    
    private RectTransform _canvasIconRect;
    public Canvas canvasMinibarHealth;

    public PlayerAttack playerAttack;
    public PlayerHealth playerHealth;

    public GameObject animMutation;

    private RectTransform rectTransform;
    public PlayerHealthBarUI playerHealthBarUI;
   

    public bool readyToEvo = false;
    public PlayerController controller;

    public AudioClip flipagesfx;
    public AudioClip buttonsfx;
    public AudioSource source;
    public AudioSource sourceButton;
    public AudioSource outsourceButton;
    public AudioClip outbuttonsfx;

    public int currentMutationPage = 0;

    public FreshDataUI freshDataSave;

    public QuestTask questTask;

    private void Awake()
    {
        Animator[] allAnims = GetComponentsInChildren<Animator>(true);
        foreach (Animator anim in allAnims) anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }


    void Start()
    {
         rectTransform = canvasMinibarHealth.GetComponent<RectTransform>();
        _bookAnimator = GetComponent<Animator>();
        animator = canvasIcon.GetComponent<Animator>();
        animator.runtimeAnimatorController = playerController.so.iconUI;
        _canvasIconRect = canvasIcon.GetComponent<RectTransform>();
        FillDictionaryTmpMesh();
        UpdateStorageUI();
        SetSizeCanvasIconAndMiniBar(controller.so);
        freshDataSave = FindFirstObjectByType<FreshDataUI>(FindObjectsInactive.Include);
        
    }
    public void OnMutationClick(int indexPage)
    {
        OutButtonSFX();
        bool evoReady = CheckElements();
        if (evoReady)
        {
            foreach (var bottle in requestBottleEvo)
            {
                PlayerInventory.Instance.storagePlayer[bottle.Key] -= bottle.Value;
            }
            UpdateStorageUI();
            foreach (var item in indexPageMutation) item.SetActive(false);
            indexPageMutation[indexPage].SetActive(true);
            freshDataSave.currentMutationPage = indexPage;
        }
    }
    public void FillDictionaryTmpMesh()
    {
        //fillStorageUI.Clear();
        int count = Mathf.Min(elementsTextMesh.elements.Length, elementsTextMesh.textMeshPro.Length);

        for (int i = 0; i < count; i++) {
            BottleElement bottleElement = elementsTextMesh.elements[i];
            TextMeshProUGUI textMesh = elementsTextMesh.textMeshPro[i].GetComponent<TextMeshProUGUI>();

            if(textMesh != null) fillStorageUI[bottleElement] = textMesh;
        }
    }
    public void UpdateStorageUI()
    {
        foreach (var item in fillStorageUI)
        {
            BottleElement bottle = item.Key;
        
            item.Value.text = $"{PlayerInventory.Instance.storagePlayer[bottle]}/\u221E";
        }
    }
    public void DataNextMutation(ShowWindowUI show)
    {
        ButtonSFX();
        if (show != null)
        {
            requestBottleEvo.Clear();
            requestBottleEvo[show.requestEvoElements.elements] = show.requestEvoElements.count;
        }
    }
    public void StartFlipAnimation(int targetIndex)
    {
        if(_pendingPageIndex < targetIndex)
        {
            _bookAnimator.SetTrigger("FlipPageNext");
            ButtonSFX();
            FlipPageSFX();
        }
        else
        {
            _bookAnimator.SetTrigger("FlipPageBack");
            ButtonSFX();
            FlipPageSFX();
        }
        _pendingPageIndex = targetIndex;
    }
    public void ChangePageVisible() //Animator using by flip page
    {
        if (_pendingPageIndex >= 0 && _pendingPageIndex < pageGeneralPages.Length)
        {
            pageGeneralPages[_pendingPageIndex].SetActive(true);
        }
    }

    public void OFFAllPagesAnimator()
    {
        for (int i = 0; i < pageGeneralPages.Length; i++)
        {
            pageGeneralPages[i].SetActive(false);
        }
    }
    public void SetupSOMutation(PlayerSO playerSO)  // Button activate this method BEFORE OnMutationClick();
    {
        readyToEvo = CheckElements();
        if (readyToEvo) {
            playerHealthBarUI.OpenBook();
        playerController.SetupSO(playerSO);
        animator.runtimeAnimatorController = playerSO.iconUI;
        SetSizeCanvasIconAndMiniBar(playerSO);
        animMutation.SetActive(true);

        }
    }

    public void SetSizeCanvasIconAndMiniBar(PlayerSO playerSO)
    {
        if (playerSO.setSizeIcon.needSetSizeCanvasIcon)
        {
            _canvasIconRect.sizeDelta = playerSO.setSizeIcon.size;
            _canvasIconRect.anchoredPosition = new Vector2(playerSO.setSizeIcon.x, playerSO.setSizeIcon.y);
        }
        else
        {
            _canvasIconRect.sizeDelta = new Vector2(200, 200);
            _canvasIconRect.anchoredPosition = new Vector2(-108.5f, -173.8f);
        }

        if (playerSO.setSizeIcon.needSetSizeHealthBarMini)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, playerSO.setSizeIcon.healthy);
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(951.12f, 537.7953f);
        }
    }

    public void OnUpDamageButton()
    {
        ButtonSFX();
        if (PlayerInventory.Instance.currentExp >= 5f)
        {
            PlayerInventory.Instance.TakeExp(5f);
            playerAttack.UpDamage(1f);
        }
    }

    public void OnDownCooldownButton()
    {
        ButtonSFX();
        if (PlayerInventory.Instance.currentExp >= 5f)
        {
            float ramainToLimit = playerAttack.playerAbilities[0].cooldown - playerAttack.cooldownModifier;
            if (ramainToLimit > 0.0f)
            {
                PlayerInventory.Instance.TakeExp(5f);
                float num = Mathf.Clamp(ramainToLimit, 0f, 0.1f);
                playerAttack.DownCooldown(num);
            }
        }
    }
    public void OnUpHealth()
    {
        ButtonSFX();
        if (PlayerInventory.Instance.currentExp >= 5f)
        {
            PlayerInventory.Instance.TakeExp(5f);
            playerHealth.UpMAXHp(2f);
        }
    }
    public void OnUpMana()
    {
        ButtonSFX();
        if (PlayerInventory.Instance.currentExp >= 7f)
        {
            PlayerInventory.Instance.TakeExp(7f);
            playerHealth.UpMAXmana(2f);
        }
    }
    public void OnUpSpeed()
    {
        ButtonSFX();
        if (PlayerInventory.Instance.currentExp >= 10f)
        {
            PlayerInventory.Instance.TakeExp(10f);
            playerController.UpSpeedRun(0.4f);
        }
    }

    private bool CheckElements()
    {
        return requestBottleEvo.All(bottle => PlayerInventory.Instance.storagePlayer.TryGetValue(bottle.Key, out int a) && a >= bottle.Value);
    }

    private void FlipPageSFX()
    {
        source.PlayOneShot(flipagesfx);
    }

    private void ButtonSFX()
    {
        sourceButton.PlayOneShot(buttonsfx);
    }
    private void OutButtonSFX()
    {
        outsourceButton.PlayOneShot(outbuttonsfx);
    }
    ///SAVE
    public void LoadMutationPage(int index)
    {
        foreach (var item in indexPageMutation) item.SetActive(false);
        indexPageMutation[index].SetActive(true);
    }


    private void OnEnable()
    {
        UpdateStorageUI();
       
        if (freshDataSave == null)
            freshDataSave = FindFirstObjectByType<FreshDataUI>(FindObjectsInactive.Include);

        if (freshDataSave != null)
        {
            LoadMutationPage(freshDataSave.currentMutationPage);
        }
    }
   
}

// if I want to change at different elements for evolution I will need change it.

[System.Serializable]
public struct FillTextMeshElement
{
    public BottleElement[] elements;
    public GameObject[] textMeshPro;
}

[System.Serializable]
public struct RequestEvoElements
{
    public BottleElement elements;
    public int count;
}