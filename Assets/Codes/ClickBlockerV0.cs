using UnityEngine;
using UnityEngine.EventSystems;

public class ClickBlockerV0 : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.Use();
    }
}
