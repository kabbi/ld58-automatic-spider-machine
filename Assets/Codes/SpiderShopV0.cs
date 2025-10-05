using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpiderShopV0 : MonoBehaviour, IDropHandler
{
    public float startingPrice = 10;
    public float factorPerSold = 0.8f;
    public TextMeshPro[] priceLabels;
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

        float startingPrice = spider.GetLevelConfig().startingPrice;
        float price = startingPrice * (float)Math.Pow(factorPerSold, soldByLevel[spider.level] - 1);

        if (price < 0.5)
        {
            return;
        }

        ScoreManagerV0.Instance.currentMoney += price;
        ScoreManagerV0.Instance.UpdateGUI();

        float nextPrice = startingPrice * (float)Math.Pow(factorPerSold, soldByLevel[spider.level]);
        priceLabels[spider.level].text = $"{nextPrice:#}$";

        Destroy(spider.gameObject);
    }
}
