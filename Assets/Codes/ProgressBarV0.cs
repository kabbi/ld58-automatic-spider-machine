using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ProgressBarV0 : MonoBehaviour
{
    public float progress;
    public Transform marker;
    public SpriteRenderer bar;
    public float maxBarHeight = 5;
    public float maxMarkerTop = 10;
    private Vector3 markerInitialPosition;
    private Vector2 barInitialSize;

    void Start()
    {
        markerInitialPosition = marker.position;
        barInitialSize = bar.size;
    }

    void Update()
    {
        marker.position = new Vector3(markerInitialPosition.x, markerInitialPosition.y + progress * maxMarkerTop);
        bar.size = new Vector2(barInitialSize.x, progress * maxBarHeight);
    }
}
