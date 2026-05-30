
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Http.Headers;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem;

public class PlayerHealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerAttack playerAttack;
    public Image fill;
    public Image fillMana;
    public TextMeshProUGUI textHp;
    public TextMeshProUGUI textMana;
    public Animator animator;
    public Image AttentionMana;
    public float timerAttention;
    public Image fillMiniHp;
    [Header("BookUI")]
    public TextMeshProUGUI textHpForBook;
    public TextMeshProUGUI textManaForBook;
    [Header("LineExpBookUI")]
    public Image fillExp;
    public TextMeshProUGUI textExp;
    [Header("LineDamage,Speed,CooldownBookUI")]
    public TextMeshProUGUI textDamage;
    public TextMeshProUGUI textCooldown;
    public TextMeshProUGUI textSpeed;
    public PlayerController playerController;
    [Header("BookUI")]
    public GameObject book;
    private BookUI bookUI;
    public GameObject map;
    public GameObject mapCamera;
    public AudioSource buttonsourceSFX;
    public AudioClip buttonSFX;
    private bool maybeinteruptedUI = true;
    public GameObject exitUI;

    void Start()
    {
        playerAttack.OnSignAttentionMana += OnAttentionSigh;
        playerHealth.OnShowHPforUI += UpdateCurrentHp;
        playerHealth.OnShowManaforUI += UpdateCurrentMana;
        PlayerInventory.Instance.OnShowExpForUI += UpdateCurrentExp;
        playerAttack.OnDamageUIBook += UpdateDamage;
        playerAttack.OnCooldownUIBook += UpdateCooldown;
        playerController.OnSpeedRunUIBook += UpdateSpeed;

        bookUI = book.GetComponent<BookUI>();
    }
    private void Update()
    {
        if (timerAttention > 0f) {
            timerAttention -= Time.deltaTime;
            if(timerAttention < 0.2f ) AttentionMana.enabled = false;
        }

        if (Keyboard.current.escapeKey.isPressed && book.activeInHierarchy)
        {
            ButtonSFX();
            maybeinteruptedUI = true;
            book.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Keyboard.current.escapeKey.isPressed && map.activeInHierarchy)
        {
            ButtonSFX();
            maybeinteruptedUI = true;
            mapCamera.SetActive(false);
            map.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void UpdateCurrentHp(float currentHp,float maxHp)
    {
        fill.fillAmount = currentHp / maxHp;
        fillMiniHp.fillAmount = currentHp / maxHp;

        textHp.text = $"{currentHp}/{maxHp}";
        textHpForBook.text = $"{currentHp}/{maxHp}";
    }
    private void UpdateCurrentMana(float currentMana, float maxMana)
    {
        fillMana.fillAmount = currentMana / maxMana;

        textMana.text = $"{currentMana}/{maxMana}";
        textManaForBook.text =$"{currentMana}/{maxMana}";
    }

    private void UpdateCurrentExp(float currentExp, float maxExp)
    {
        fillExp.fillAmount = currentExp / 100f;
        textExp.text = $"{currentExp}/\u221E";
    }



    private void OnDestroy()
    {
            playerAttack.OnSignAttentionMana -= OnAttentionSigh;
            playerHealth.OnShowHPforUI -= UpdateCurrentHp;
            playerHealth.OnShowManaforUI -= UpdateCurrentMana;
            PlayerInventory.Instance.OnShowExpForUI -= UpdateCurrentExp;
            playerAttack.OnDamageUIBook -= UpdateDamage;
            playerAttack.OnCooldownUIBook -= UpdateCooldown;
            playerController.OnSpeedRunUIBook -= UpdateSpeed;
    }

    private void OnAttentionSigh(bool enable)
    {
        if (!AttentionMana.enabled && timerAttention <=0f) { 
            AttentionMana.enabled = true;
            timerAttention = 3f;
        } 

    }
    private void UpdateDamage(float currentDamage)
    {
        textDamage.text = $"{currentDamage}";
    }
    private void UpdateCooldown(float currentCooldown)
    {
        textCooldown.text = $"{currentCooldown:F1}";
    }
    private void UpdateSpeed(float currentSpeed)
    {
        textSpeed.text = $"{currentSpeed}";
    }

    public void OpenBook()
    {
        if (bookUI.readyToEvo && book.activeInHierarchy ) //  evo anim
        {
            //maybeinteruptedUI = true;
            ButtonSFX();
            book.SetActive(false);
            StartCoroutine(FrozenTimeForMutation());
            return;
        }
        

        if (book.activeInHierarchy)
        {
            maybeinteruptedUI = true;
            ButtonSFX();
            book.SetActive(false);
            Time.timeScale = 1f;
        } 
        else {
            if (maybeinteruptedUI && !exitUI.activeInHierarchy)
            {
                maybeinteruptedUI = false;
                ButtonSFX();
                book.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }


    public void OpenMap()
    {
        if (mapCamera.activeInHierarchy && map.activeInHierarchy)
        {
            maybeinteruptedUI =true;
            ButtonSFX();
            mapCamera.SetActive(false);
            map.SetActive(false);
        }
        else
        {
            if (maybeinteruptedUI && !exitUI.activeInHierarchy)
            {
                maybeinteruptedUI = false;
                ButtonSFX();
                mapCamera.SetActive(true);
                map.SetActive(true);
            }
        }
    }
    private IEnumerator FrozenTimeForMutation()
    {
        Time.timeScale = 0f;
        playerController.directAnimator = Vector2.zero;

        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1f;
        maybeinteruptedUI = true;
        bookUI.readyToEvo = false; 
    }
    private void ButtonSFX()
    {
        buttonsourceSFX.PlayOneShot(buttonSFX);
    }
}
