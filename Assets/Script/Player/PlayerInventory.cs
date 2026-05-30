using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public BookUI bookUI;

    public Dictionary<BottleElement, int> storagePlayer = new Dictionary<BottleElement, int>();

    public Action<float, float> OnShowExpForUI;
    public float currentExp ;
    public float maxExp = 5000f;
    public Action<BottleElement>OnCheckProgressQuest;

    void Awake()
    {
        Instance = this;
        FullInventory();

    }

    private void Start()
    {
        currentExp =0f;
        OnShowExpForUI?.Invoke(currentExp, maxExp);
    }
    public void GetBottle(BottleElement bottle)
    {
        if (storagePlayer.ContainsKey(bottle))
        {
            storagePlayer[bottle] += 1;
        }
        else
        {
            storagePlayer.Add(bottle, 1);
        }
        OnCheckProgressQuest?.Invoke(bottle);
       if (bookUI != null) bookUI.UpdateStorageUI();
    }
    public void FullInventory()
    {
        foreach (BottleElement type in System.Enum.GetValues(typeof(BottleElement)))
        {
            storagePlayer[type] = 0;
        }
        if (bookUI != null) bookUI.UpdateStorageUI();
    }
    public void GetExp(float exp)
    {
        if(currentExp+exp <= maxExp)
        {
            currentExp += exp;
            OnShowExpForUI?.Invoke(currentExp, maxExp);
        }
    }

    public void TakeExp(float takeExp)
    {
        currentExp -= takeExp;
        OnShowExpForUI?.Invoke(currentExp,maxExp);
    }



    ///SAVE
    public List<BottleElement> SaveStorageBottleKey()
    {
        List< BottleElement > storageKey = new List< BottleElement >();
        foreach(var n in storagePlayer)
        {
            storageKey.Add(n.Key);
        }
        return storageKey;
    }
    public List<int> SaveStorageBottleValue()
    {
        List<int> storageValue = new List<int>();
        foreach (var v in storagePlayer)
        {
            storageValue.Add(v.Value);
        }
        return storageValue;
    }

    public void LoadStorage(List<BottleElement> storageKey, List<int> storageValue)
    {
        storagePlayer.Clear();
       for (int i = 0; i < storageKey.Count; i++)
        {
            storagePlayer.Add(storageKey[i], storageValue[i]);
        }
    }
}
