using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class DamageNumberPool : MonoBehaviour 
{
    public static DamageNumberPool Instance;
    public GameObject textPrefab;
    private Queue<ShowDamageNumberPref> pool = new Queue<ShowDamageNumberPref>();
    public int poolsize;
  
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolsize; i++) {
            GameObject cozyNum = GameObject.Instantiate(textPrefab);
            ShowDamageNumberPref textMesh = cozyNum.GetComponent<ShowDamageNumberPref>();
            cozyNum.SetActive(false);
            pool.Enqueue(textMesh);
        }
    }
    public void OnShowDamage(Vector3 point ,float amount,Color color)
    {
        if (pool.Count <= 0) return;

        ShowDamageNumberPref textMesh = pool.Dequeue();

        textMesh.SetTextMeshPro(amount, color);
        textMesh.gameObject.SetActive(true);
        textMesh.transform.position = point;
        StartCoroutine(RetunIntoPool(textMesh));
    }

    public void OnShowElement(Vector3 point, Color color)
    {
        if (pool.Count < 0) return;

        ShowDamageNumberPref textMesh = pool.Dequeue();

        textMesh.SetTextMeshProForElement(color);
        textMesh.gameObject.SetActive(true);
        textMesh.transform.position = point;
        StartCoroutine(RetunIntoPool(textMesh));
    }

    public void OnShowExp(Vector3 point, float amount, Color color)
    {
        if (pool.Count < 0) return;

        ShowDamageNumberPref textMesh = pool.Dequeue();

        textMesh.SetTextMeshProExpa(amount, color);
        textMesh.gameObject.SetActive(true);
        textMesh.transform.position = point;
        StartCoroutine(RetunIntoPool(textMesh));
    }
    IEnumerator RetunIntoPool(ShowDamageNumberPref num)
    {
        yield return new WaitForSeconds(1f);
        num.gameObject.SetActive(false);
        pool.Enqueue(num);
    }
}