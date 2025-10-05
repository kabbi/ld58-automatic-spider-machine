using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class ScoreManagerV0 : MonoBehaviour
{
    public static ScoreManagerV0 Instance { get; private set; }
    public float currentNumberOfSpiders = 0;
    public float currentMoney = 100;
    public float maxNumberOfSpiders = 100;
    public float asmFeedPrice = 10;
    public InputAction leftClick;
    public HideSpotV0[] allHidingSpots;
    public ProgressBarV0 fearProgressBar;
    public TextMeshPro moneyLabel;
    public TextMeshPro moneyDeltaLabel;
    public Texture2D pressedCursor;
    public Texture2D defaultCursor;
    public SpiderSpawnerV0 asm;
    private float lastMoney;

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

    void Start()
    {
        lastMoney = currentMoney;
        allHidingSpots = FindObjectsByType<HideSpotV0>(FindObjectsSortMode.None);
    }


    private void OnEnable()
    {
        leftClick.Enable();
        leftClick.started += OnClickStart;
        leftClick.canceled += OnClickEnd;
    }

    private void OnDisable()
    {
        leftClick.started -= OnClickStart;
        leftClick.canceled -= OnClickEnd;
        leftClick.Disable();
    }

    public void UpdateGUI()
    {
        StopAllCoroutines();
        float delta = currentMoney - lastMoney;
        bool hasDelta = Math.Abs(delta) > 0.01;

        fearProgressBar.progress = currentNumberOfSpiders / maxNumberOfSpiders;
        moneyLabel.text = $"{currentMoney.ToString("#,##0")}$";
        moneyDeltaLabel.text = $"{(delta > 0 ? "+" : "")}{delta.ToString("#,##")}$";
        moneyDeltaLabel.gameObject.SetActive(hasDelta);

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

    public HideSpotV0 GetRandomHidingSpot()
    {
        return allHidingSpots[UnityEngine.Random.Range(0, allHidingSpots.Length)];
    }

    IEnumerator ResetDelta()
    {
        yield return new WaitForSeconds(0.9f);
        lastMoney = currentMoney;
        UpdateGUI();
    }

    void OnClickStart(InputAction.CallbackContext context)
    {
        Cursor.SetCursor(pressedCursor, Vector2.zero, CursorMode.Auto);
    }

    void OnClickEnd(InputAction.CallbackContext context)
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
