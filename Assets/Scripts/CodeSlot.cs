using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script to handle code slots
 * data they carry and reference to one another
 *
 */

public class CodeSlot : MonoBehaviour, IDropHandler
{
    
    [SerializeField] private GameObject codeSlot;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CommandExecuter commandExecuter;
    
    private bool beingUsed = false;

    public CodeSlot previous = null; // previous = previous CodeSlot
    public CodeSlot next = null; // next = next CodeSlot
    public GameObject data = null; // data = CodeBlock

    public void OnDrop(PointerEventData eventData) // Function called when a draggable object is dropped in here
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            if (!beingUsed)
            {

                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                    GetComponent<RectTransform>().anchoredPosition; // If the item dragged is not null, we get the RectTransform of the object that is being dragged and we make it equal to the RectTransform of the CodeSlot
                eventData.pointerDrag.GetComponent<DragDrop>().setOnSlot(true);
                
                GameObject slot = Instantiate(codeSlot, transform.position, Quaternion.identity); // Creating new CodeSlot
                slot.transform.SetParent(transform.parent); // Setting new CodeSlot as child of Canvas
                slot.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -100); // Update position of new CodeSlot
                slot.transform.SetSiblingIndex(1); // Moving new CodeSlot up in the hierarchy so that CodeBlocks can be seen in front of it

                // Setting node connections
                next = slot.GetComponent<CodeSlot>(); // Making reference to next slot
                next.previous = this; // Setting new CodeSlot's previous reference to this CodeSlot
                next.commandExecuter = commandExecuter; // Passing the commandExecuter to next CodeSlot
                
                data = eventData.pointerDrag.gameObject; // Getting data for the CodeBlock that is in this CodeSlot
                data.GetComponent<DragDrop>().codeSlot = this; // Setting data for the CodeBlock that is in this CodeSlot

            }
        }
    }

    public void removeData() // Called when a CodeBlock is removed from a CodeSlot
    {
        data = null;
        commandExecuter.updateList(this); // Call updateList, updateList updates the structure of the terminal
    }

    public void moveCodeBlock() // Moves CodeBlock into CodeSlot
    {
        data.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
    }
    
}
