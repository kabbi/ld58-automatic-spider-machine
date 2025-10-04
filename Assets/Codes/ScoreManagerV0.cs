using UnityEngine;
using TMPro;
using System.Collections;
using System;
using NUnit.Framework;

public class ScoreManagerV0 : MonoBehaviour
{
    public static ScoreManagerV0 Instance { get; private set; }
    public float currentNumberOfSpiders = 0;
    public float currentMoney = 100;
    public float maxNumberOfSpiders = 100;
    public float asmFeedPrice = 10;
    public ProgressBarV0 fearProgressBar;
    public TextMeshProUGUI moneyLabel;
    public SpiderSpawnerV0 asm;
    private float lastMoney;

    void Awake()
    {
        lastMoney = currentMoney;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateGUI()
    {
        StopAllCoroutines();
        float delta = currentMoney - lastMoney;
        bool hasDelta = Math.Abs(delta) > 0.01;
        string deltaText = hasDelta ? $"{(delta > 0 ? "+" : "")}{delta.ToString("#,##")}$" : "";

        fearProgressBar.progress = currentNumberOfSpiders / maxNumberOfSpiders;
        moneyLabel.text = $"{currentMoney.ToString("#,##0")}$ {deltaText}";

        if (hasDelta)
        {
            StartCoroutine(ResetDelta());
        }
    }

    public void FeedASM()
    {
        if (currentMoney < asmFeedPrice)
        {
            return;
        }
        currentMoney -= asmFeedPrice;
        asm.FeedMoney();
        UpdateGUI();
    }

    IEnumerator ResetDelta()
    {
        yield return new WaitForSeconds(0.9f);
        lastMoney = currentMoney;
        UpdateGUI();
    }
}
