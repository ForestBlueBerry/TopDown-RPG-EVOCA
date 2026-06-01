using UnityEngine;
using TMPro;
using System.Collections;

public class IdleProgression : MonoBehaviour
{
    [Header("Settings")]
    private float interval = 20f;
    private float expAmount = 5;

    [Header("UI References")]
    public CanvasGroup expCanvasGroup;
    public TextMeshProUGUI expText;
    public float fadeSpeed = 1f;
    private Coroutine fadeCoroutine;

    public float completerewardexp;
    void Start()
    {
        if (expCanvasGroup != null) expCanvasGroup.alpha = 0;
        StartCoroutine(IdleExpRoutine(expAmount));
    }
    private IEnumerator IdleExpRoutine(float exp)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(interval);
            if (PlayerInventory.Instance != null)
            {
                PlayerInventory.Instance.GetExp(exp);
                if (expText != null) expText.text = $"+{exp} EXP";
                TriggerFade();
            }
        }
    }
    private IEnumerator FadeEffect()
    {
        expCanvasGroup.alpha = 1f;
        yield return new WaitForSecondsRealtime(2f);
        while (expCanvasGroup.alpha > 0)
        {
            expCanvasGroup.alpha -= Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }
    }
    public void LoadExpAfterRest(string saveTimeString)
    {
        if (string.IsNullOrEmpty(saveTimeString)) return;

        if (System.DateTime.TryParseExact(saveTimeString, "dd.MM.yyyy HH:mm",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out System.DateTime lastSaveTime))
        {
            System.TimeSpan offlineTime = System.DateTime.Now - lastSaveTime;
            float offlineSeconds = (float)offlineTime.TotalSeconds;

            if (offlineSeconds < 30f)
            {
                return;
            }

            int approveExp = (int)(offlineSeconds / 30f);
            approveExp = Mathf.Clamp(approveExp, 0, 50);

            if (approveExp > 0)
            {
                expText.text = $"+{approveExp} EXP Rest Reward";
                PlayerInventory.Instance.GetExp(approveExp);
                TriggerFade();
            }
        }
    }
    private void TriggerFade()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeEffect());
    }
}