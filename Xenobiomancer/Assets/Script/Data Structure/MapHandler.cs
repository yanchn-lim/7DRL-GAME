using UnityEngine;
using EventManagerYC;

public class MapHandler : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public GameObject map;
    private EventManager eventManager = EventManager.Instance;

    private void Awake()
    {
        //subscribing to the relevant events
        //eventManager.AddListener(Event.MAP_NODE_CLICKED, ToggleMap);
        //eventManager.AddListener(Event.RAND_EVENT_END, ToggleMap); 
    }

    private void OpenMap()
    {
        map.SetActive(true);
    }

    private void CloseMap()
    {
        map.SetActive(false); 
    }

    //method used by the map button to open and closes the map
    public void ToggleMap()
    {
        if (map.activeInHierarchy)
        {
            CloseMap();
        }
        else if (!map.activeInHierarchy)
        {
            OpenMap();
        }
    }

}
