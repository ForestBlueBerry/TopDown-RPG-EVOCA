using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowWindowUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public CanvasGroup canvasWindowHint;
    public RequestEvoElements requestEvoElements;
   [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(canvasWindowHint == null) return;
        if(requestEvoElements.count > 0) m_TextMeshProUGUI.text = $"{PlayerInventory.Instance.storagePlayer[requestEvoElements.elements]}/\u221E";
        canvasWindowHint.alpha = 1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canvasWindowHint == null) return;
        canvasWindowHint.alpha = 0f;
       
    }

    private void OnEnable()
    {
        canvasWindowHint.alpha = 0f;
    }


}

