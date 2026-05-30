using UnityEngine;

public class HealthBarShaderController : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _propBlock;
    private int _fillID;

    [SerializeField] private string shaderReferenceName = "_HealthFill";

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _propBlock = new MaterialPropertyBlock();
        GetComponent<Renderer>().sortingOrder = 9; 
      
        _fillID = Shader.PropertyToID(shaderReferenceName);
    }


    public void SetHealth(float current, float max)
    {
        if (_meshRenderer == null) return;

        float percent = Mathf.Clamp01(current / max);

        _meshRenderer.GetPropertyBlock(_propBlock);

        _propBlock.SetFloat(_fillID, percent);

        _meshRenderer.SetPropertyBlock(_propBlock);
    }
}