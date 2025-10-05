using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;



[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SpiderControllerV0 : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Serializable]
    public struct SpiderLevelConfig
    {
        public int level;
        public float startingPrice;
        public RuntimeAnimatorController overrides;
    }

    enum State
    {
        WalkingToHidingSpot,
        Hidden,
        Dragged
    }

    private State currentState;
    public float hideTime;
    public float walkSpeed;
    private float lastHidden;
    private HideSpotV0 hidingTarget;
    private SpriteRenderer sprite;
    private Animator animator;
    public TextMeshPro label;
    public SpiderLevelConfig[] levels;
    private SpiderLevelConfig currentLevelConfig;
    public int level = 0;

    void Start()
    {
        currentLevelConfig = levels[0];
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hidingTarget = ScoreManagerV0.Instance.GetRandomHidingSpot();
        currentState = State.WalkingToHidingSpot;
    }

    void OnEnable()
    {
        ScoreManagerV0.Instance.currentNumberOfSpiders += 1;
        ScoreManagerV0.Instance.UpdateGUI();
    }

    void OnDisable()
    {
        ScoreManagerV0.Instance.currentNumberOfSpiders -= 1;
        ScoreManagerV0.Instance.UpdateGUI();
    }

    void Update()
    {
        if (currentState == State.WalkingToHidingSpot && hidingTarget)
        {
            Vector3 direction = (hidingTarget.transform.position - transform.position).normalized * walkSpeed * Time.deltaTime;
            transform.position += direction;
            if (Vector2.Distance(transform.position, hidingTarget.transform.position) < 0.1)
            {
                hidingTarget.spidersHere.Add(this);
                currentState = State.Hidden;
                animator.SetFloat("speed", 0);
                lastHidden = Time.time;
            }
        }
        if (currentState == State.Hidden && hidingTarget && Time.time - lastHidden > hideTime)
        {
            hidingTarget.spidersHere.Remove(this);
            hidingTarget = ScoreManagerV0.Instance.GetRandomHidingSpot();
            currentState = State.WalkingToHidingSpot;
            animator.SetFloat("speed", 1);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0;
        transform.position = worldPoint;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentState = State.Dragged;
        animator.SetBool("drag", true);
        sprite.sortingLayerName = "Drag";
        GetComponent<Collider2D>().enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        animator.SetBool("drag", false);
        animator.SetFloat("speed", 1);
        sprite.sortingLayerName = "Spider";
        currentState = State.WalkingToHidingSpot;
        GetComponent<Collider2D>().enabled = true;

        // if (eventData.pointerEnter != null)
        // {
        //     var dropHandler = eventData.pointerEnter.GetComponent<IDropHandler>();
        //     if (dropHandler != null)
        //     {
        //         ExecuteEvents.Execute(eventData.pointerEnter, eventData, ExecuteEvents.dropHandler);
        //         return;
        //     }
        // }

        List<Collider2D> otherSpiders = new();
        Physics2D.OverlapCollider(GetComponent<Collider2D>(), otherSpiders);
        Collider2D otherSpiderCollider = otherSpiders.Find(collider =>
        {
            var spider = collider.GetComponent<SpiderControllerV0>();
            return spider && spider.level == level;
        });
        if (!otherSpiderCollider)
        {
            return;
        }

        SpiderControllerV0 otherSpider = otherSpiderCollider.GetComponent<SpiderControllerV0>();
        Destroy(otherSpider.gameObject);
        if (level >= levels.Length)
        {
            return;
        }

        level += 1;
        currentLevelConfig = levels[level];
        label.text = $"lvl {level + 1}";
        animator.runtimeAnimatorController = currentLevelConfig.overrides;
        ScoreManagerV0.Instance.MarkSpiderDiscovered(this);
    }

    public void Shoo(HideSpotV0 nextHidingSpot)
    {
        hidingTarget.spidersHere.Remove(this);
        hidingTarget = nextHidingSpot;
        currentState = State.WalkingToHidingSpot;
        animator.SetFloat("speed", 1);
    }

    public SpiderLevelConfig GetLevelConfig()
    {
        return currentLevelConfig;
    }
}
