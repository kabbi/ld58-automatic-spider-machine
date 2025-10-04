using UnityEngine;
using TMPro;

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

    void Awake()
    {
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
        fearProgressBar.progress = currentNumberOfSpiders / maxNumberOfSpiders;
        moneyLabel.text = $"{currentMoney.ToString("#,##0")}$";
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
}
