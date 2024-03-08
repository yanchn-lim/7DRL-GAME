using UnityEngine;
using Patterns;
using DataStructure;

public class MapHandler : MonoBehaviour
{
    private EventManager eventManager = EventManager.Instance;

    private void Awake()
    {
        //subscribing to the relevant events
        eventManager.AddListener(EventName.MAP_NODE_CLICKED, ToggleMap);
        //eventManager.AddListener(Event.RAND_EVENT_END, ToggleMap); 
    }

    private void OpenMap()
    {
    }

    private void CloseMap()
    {
    }

    //method used by the map button to open and closes the map
    public void ToggleMap()
    {
        Debug.Log("F0MW9E8NUWETQV90NU34TB-IM90N WEG [AOIK");
    }

}
