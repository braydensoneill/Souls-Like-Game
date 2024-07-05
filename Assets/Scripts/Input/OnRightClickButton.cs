using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnRightClickButton : MonoBehaviour, IPointerClickHandler
{
    public Button button;
    public UnityEngine.Events.UnityEvent onRightClick;

    void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick.Invoke();
        }
    }
}
