using UnityEngine;
using TMPro;
public class MiniInventoryUI : MonoBehaviour
{
    public TextMeshProUGUI outtextMeshProUGUI;
    private TextMeshProUGUI currenttextMeshProUGUI;

    void Awake()
    {
        currenttextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        currenttextMeshProUGUI.text = outtextMeshProUGUI.text;
    }

    private void OnEnable()
    {
        currenttextMeshProUGUI.text = outtextMeshProUGUI.text;
    }
}
