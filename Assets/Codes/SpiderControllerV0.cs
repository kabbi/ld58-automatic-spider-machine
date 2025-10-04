using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SpiderControllerV0 : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    enum State
    {
        ControlledChilling,
        ControlledWalking,
        Dragged
    }

    private State currentState;
    public Vector2 bounds;
    public float chillTime;
    public float walkSpeed;
    private Vector3 walkTarget;
    private SpriteRenderer sprite;
    public TextMeshPro label;
    public int level = 0;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Run());
        ScoreManagerV0.Instance.currentNumberOfSpiders += 1;
        ScoreManagerV0.Instance.UpdateGUI();
    }

    void Stop()
    {
        ScoreManagerV0.Instance.currentNumberOfSpiders -= 1;
        ScoreManagerV0.Instance.UpdateGUI();
    }

    void Update()
    {
    }

    IEnumerator Run()
    {
        while (true)
        {
            if (currentState == State.Dragged)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            currentState = State.ControlledChilling;
            yield return new WaitForSeconds(chillTime);
            currentState = State.ControlledWalking;
            walkTarget = new Vector2(Random.value * bounds.x * 2 - bounds.x, Random.value * bounds.y * 2 - bounds.y);
            while (true)
            {
                if (currentState != State.ControlledWalking)
                {
                    break;
                }
                Vector3 direction = (walkTarget - transform.position).normalized * walkSpeed * Time.deltaTime;
                transform.position += direction;
                if (Vector2.Distance(transform.position, walkTarget) < 0.1)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
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
        sprite.sortingLayerName = "Drag";
        GetComponent<Collider2D>().enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Collider2D>().enabled = true;
        currentState = State.ControlledChilling;
        sprite.sortingLayerName = "Spider";

        if (eventData.pointerEnter != null)
        {
            var dropHandler = eventData.pointerEnter.GetComponent<IDropHandler>();
            if (dropHandler != null)
            {
                ExecuteEvents.Execute(eventData.pointerEnter, eventData, ExecuteEvents.dropHandler);
                return;
            }
        }

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
}
