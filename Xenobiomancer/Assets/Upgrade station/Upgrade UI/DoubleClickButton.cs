using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DoubleClickButton : MonoBehaviour, IPointerUpHandler ,IPointerEnterHandler , IPointerExitHandler
{
    private int timeClick = 0;
    private bool cusorNearButton;

    public abstract void ClickFirstTime();
    public abstract void ClickSecondTime();

    public virtual void ExitToggleState() { }

    public void Update()
    {
        if(Input.GetMouseButtonUp(0) && !cusorNearButton)
        {
            timeClick = 0;
            ExitToggleState();
        }
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        cusorNearButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cusorNearButton= false;
    }

    //when the player click finish
    public void OnPointerUp(PointerEventData eventData)
    {
        if(timeClick == 0)
        {
            ClickFirstTime();
            timeClick++;
        }
        else if(timeClick == 1)
        {
            ClickSecondTime();
            timeClick = 0;//reset back
        }
    }
}
