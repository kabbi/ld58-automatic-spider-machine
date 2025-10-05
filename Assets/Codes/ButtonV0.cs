using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonV0 : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent unityEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        unityEvent.Invoke();
    }
}
