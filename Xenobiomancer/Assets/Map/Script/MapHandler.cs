using UnityEngine;
using Patterns;
using DataStructure;
using UnityEngine.SceneManagement;

public class MapHandler : MonoBehaviour
{
    private EventManager eventManager = EventManager.Instance;

    [SerializeField] private GameObject mapCanvas;

    private void Awake()
    {
        //subscribing to the relevant events
        eventManager.AddListener(EventName.MAP_NODE_CLICKED, CloseMap);
        eventManager.AddListener(EventName.LEVEL_COMPLETED, OpenMap);
        //eventManager.AddListener(Event.RAND_EVENT_END, ToggleMap); 
    }

    private void OpenMap()
    {
        mapCanvas.SetActive(true);
    }

    private void CloseMap()
    {
        mapCanvas.SetActive(false);
    }

    //method used by the map button to open and closes the map
    public void ToggleMap()
    {
        
       
    }

}
