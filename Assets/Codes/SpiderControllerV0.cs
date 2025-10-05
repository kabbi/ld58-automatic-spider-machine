using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SpiderControllerV0 : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    enum State
    {
        ControlledChilling,
        ControlledWalking,
        WalkingToHidingSpot,
        Hidden,
        Dragged
    }

    private State currentState;
    public Vector2 bounds;
    public float chillTime;
    public float walkSpeed;
    private Vector3 walkTarget;
    private HideSpotV0 hidingTarget;
    private SpriteRenderer sprite;
    private Animator animator;
    public TextMeshPro label;
    public int level = 0;

    void Start()
    {
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
            }
        }
        animator.speed = currentState == State.WalkingToHidingSpot ? 1 : 0;
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
        sprite.sortingLayerName = "Drag";
        GetComponent<Collider2D>().enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
        level += 1;
        label.text = $"lvl {level + 1}";
    }

    public void Shoo(HideSpotV0 nextHidingSpot)
    {
        hidingTarget.spidersHere.Remove(this);
        hidingTarget = nextHidingSpot;
        currentState = State.WalkingToHidingSpot;
    }
}
