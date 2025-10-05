using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class ShooControllerV0 : MonoBehaviour, IPointerClickHandler
{
    public HideSpotV0[] shooPoints;
    public GameObject effectObject;
    public Sprite activeSprite;
    public float activeTime = 2;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    private AudioSource audioSource;
    public AudioClip soundEffect;
    public bool shake;
    public float shakeAmplitude = 0.2f;
    public float shakeDuration = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<HideSpotV0> otherHidingSpots = new();
        foreach (var hidingSpot in ScoreManagerV0.Instance.allHidingSpots)
        {
            if (!shooPoints.Contains(hidingSpot))
            {
                otherHidingSpots.Add(hidingSpot);
            }
        }
        foreach (var hidingSpot in shooPoints)
        {
            List<SpiderControllerV0> spiders = new(hidingSpot.spidersHere);
            foreach (var spider in spiders)
            {
                HideSpotV0 nextHidingSpot = otherHidingSpots[UnityEngine.Random.Range(0, otherHidingSpots.Count)];
                spider.Shoo(nextHidingSpot);
            }
        }
        StopAllCoroutines();
        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        audioSource.PlayOneShot(soundEffect);
        if (shake)
        {
            StartCoroutine(Shake());
        }
        if (effectObject)
        {
            effectObject.SetActive(true);
        }
        if (activeSprite)
        {
            spriteRenderer.sprite = activeSprite;
        }
        yield return new WaitForSeconds(activeTime);
        if (effectObject)
        {
            effectObject.SetActive(false);
        }
        if (activeSprite)
        {
            spriteRenderer.sprite = originalSprite;
        }
    }

    IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * shakeAmplitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * shakeAmplitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
