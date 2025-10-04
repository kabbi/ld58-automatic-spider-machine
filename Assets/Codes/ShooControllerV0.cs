using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShooControllerV0 : MonoBehaviour, IPointerClickHandler
{
    public HideSpotV0[] shooPoints;

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
    }
}
