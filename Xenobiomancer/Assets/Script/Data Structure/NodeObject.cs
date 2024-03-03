using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EventManagerYC;
public class NodeObject : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Node Node { get; set; }
    public Sprite[] spriteArray;
    private Image image;
    private Animator animator;

    private float circleSpeed = 0.1f;
    private Color disableColor = new Color(0, 0, 0, 0.6f);
    private Color enableColor = new Color(0, 0, 0, 1f);
    private bool activated = false;

    private void Awake()
    {
        //getting the components
        animator = this.gameObject.GetComponent<Animator>();
        image = this.gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (Node.IsAccesible)
        {
            animator.Play("NodeAnimation");
        }
    }

    /* Triggered when node is clicked
     * First, it checks if the node is not null and is accessible
     * before starting the encounter
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Node != null && Node.IsAccesible)
        {
            Debug.Log("ID : " + Node.Id + " Encounter : " + Node.EncounterType + " DEPTH : " + Node.Depth);
            StartCoroutine(StartEncounter());
        }
    }

    //sets the colour to the highlighted colour
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = enableColor;
    }

    //sets the colour to the unhighlighted colour
    public void OnPointerExit(PointerEventData eventData)
    {
        //only set if it is inaccessible and not activated
        if (!Node.IsAccesible && !activated)
        {
            image.color = disableColor;
        }
    }

    // sets the status of the node to accessible and play the animation and highlight it
    public void MakeAccessible()
    {
        Node.IsAccesible = true;
        animator.Play("NodeAnimation");
        image.color = enableColor;
    }

    // sets the status of the node to inaccessible and display it accordingly
    public void MakeInAccessible()
    {
        Node.IsAccesible = false;
        animator.Play("NoAnim");
        image.color = disableColor;
    }

    // sets the appropriate sprite for the encounter
    public void SetSprite()
    {
        switch (Node.EncounterType)
        {
            case Node.Encounter.ENEMY:
                image.sprite = spriteArray[0];
                break;
            case Node.Encounter.ELITE:
                image.sprite = spriteArray[1];
                break;
            case Node.Encounter.EVENT:
                image.sprite = spriteArray[2];
                break;
            case Node.Encounter.REST:
                image.sprite = spriteArray[3];
                break;;
            case Node.Encounter.BOSS:
                image.sprite = spriteArray[5];
                break;
        }
    }

    // animation for circling the node
    // done by having the circle image set to radial and filling it in gradually
    private IEnumerator CircleNode()
    {
        Image circle = this.transform.Find("ink-swirl").GetComponent<Image>();

        for (float i = 0; i < 1; i+= circleSpeed)
        {
            circle.fillAmount += circleSpeed;
            yield return new WaitForSeconds(0.02f);
        }    
    }

    // starts the encounter event after finish setting up the node
    private IEnumerator StartEncounter()
    {
        animator.Play("NoAnim");
        activated = true;
        Node.IsAccesible = false;
        yield return StartCoroutine(CircleNode()); //waits for the circling to be finished

        // this event starts the encounter and
        // makes the other nodes in the same depth to be inaccessible
        // and also make the next depth's node that is connected to this node be accessible
        //EventManager.Instance.TriggerEvent<Node>(Event.MAP_NODE_CLICKED, Node);
    }


}
