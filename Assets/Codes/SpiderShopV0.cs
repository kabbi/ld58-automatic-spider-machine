using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpiderShopV0 : MonoBehaviour, IDropHandler
{
    public float startingPrice;
    public float factorPerSold = 0.8f;
    private Dictionary<int, int> soldByLevel = new();

    public void OnDrop(PointerEventData data)
    {
        SpiderControllerV0 spider = data.pointerDrag.GetComponent<SpiderControllerV0>();
        if (!spider)
        {
            return;
        }

        soldByLevel.TryGetValue(spider.level, out int currentCount);
        soldByLevel[spider.level] = currentCount + 1;

        float price = startingPrice * spider.level * (float)Math.Pow(factorPerSold, soldByLevel[spider.level] - 1);
        ScoreManagerV0.Instance.currentMoney += price;
        ScoreManagerV0.Instance.UpdateGUI();

        Destroy(spider.gameObject);
    }
}
