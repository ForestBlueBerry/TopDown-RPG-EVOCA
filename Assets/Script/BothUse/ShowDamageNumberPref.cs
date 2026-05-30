using UnityEngine;
using TMPro;

public class ShowDamageNumberPref : MonoBehaviour
{
    public TextMeshPro textMeshPro;

    private float gravity = 3.5f;
    private float verticalVelocity; 
    private float horizontalSpeed; 
    private float lifeTime = 1f; 
    private float timer;

    public void SetTextMeshPro(float number, Color color)
    {
        textMeshPro.text = $"-{number.ToString()}";
        textMeshPro.color = color;
        transform.localScale = Vector3.one;
        timer = lifeTime;
        verticalVelocity = Random.Range(2.5f,3.0f);
        horizontalSpeed = Random.Range(-1.5f, 1.5f);
    }
    public void SetTextMeshProForElement(Color color)
    {
        textMeshPro.text = $"+element";
        textMeshPro.color = color;
        transform.localScale = Vector3.one;
        timer = lifeTime;
        verticalVelocity = Random.Range(2.5f, 3.0f);
        horizontalSpeed = Random.Range(-1.5f, 1.5f);
    }
    public void SetTextMeshProExpa(float number, Color color)
    {
        textMeshPro.text = $"+{number.ToString()}";
        textMeshPro.color = color;
        transform.localScale = Vector3.one;
        timer = lifeTime;
        verticalVelocity = Random.Range(2.5f, 3.0f);
        horizontalSpeed = Random.Range(-1.5f, 1.5f);
    }
    private void Update()
    {
        if (timer <= 0) return;
        verticalVelocity -= gravity * Time.deltaTime;
        Vector3 move = new Vector3(horizontalSpeed, verticalVelocity, 0);
        transform.position += move * Time.deltaTime;
        timer -= Time.deltaTime;
    }
}