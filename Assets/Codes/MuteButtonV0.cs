using UnityEngine;
using UnityEngine.EventSystems;

public class MuteButtonV0 : MonoBehaviour, IPointerClickHandler
{
    private bool muted;
    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite mutedSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        muted = !muted;
        AudioListener.volume = muted ? 0 : 1;
        spriteRenderer.sprite = muted ? mutedSprite : defaultSprite;
    }
}
