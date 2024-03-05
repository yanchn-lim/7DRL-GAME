using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Patterns;
namespace DataStructure
{
    public class NodeObject : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public MapNode Node { get; set; }
        public Sprite[] spriteArray;
        Image image;
        Animator animator;

        //private float circleSpeed = 0.1f;
        Color disableColor = new Color(0, 0, 0, 0.6f);
        Color enableColor = new Color(0, 0, 0, 1f);
        bool activated = false;

        private void Awake()
        {
            //getting the components
            animator = gameObject.GetComponent<Animator>();
            image = gameObject.GetComponent<Image>();
        }

        private void OnEnable()
        {
            //if (Node.IsAccesible)
            //{
            //    //animator.Play("NodeAnimation");
            //}
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
            if (!Node.IsAccesible && !activated)
            {
                image.color = enableColor;
            }
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
            //animator.Play("NodeAnimation");
            image.color = Color.red;
        }

        // sets the status of the node to inaccessible and display it accordingly
        public void MakeInAccessible()
        {
            Node.IsAccesible = false;
            //animator.Play("NoAnim");
            image.color = disableColor;
        }

        // sets the appropriate sprite for the encounter
        public void SetSprite()
        {
            //switch (Node.EncounterType)
            //{

            //}
        }

        // animation for circling the node
        // done by having the circle image set to radial and filling it in gradually
        private IEnumerator AnimateSelect()
        {
            yield return null;
        }

        // starts the encounter event after finish setting up the node
        private IEnumerator StartEncounter()
        {
            //animator.Play("NoAnim");
            activated = true;
            Node.IsAccesible = false;
            
            yield return StartCoroutine(AnimateSelect()); //waits for the circling to be finished

            // this event starts the encounter and
            // makes the other nodes in the same depth to be inaccessible
            // and also make the next depth's node that is connected to this node be accessible
            EventManager.Instance.TriggerEvent<Node>(EventName.MAP_NODE_CLICKED, Node);
        }


    }
}