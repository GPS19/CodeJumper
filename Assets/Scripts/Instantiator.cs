using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script to handle the instantiation of every new code block
 */

public class Instantiator : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject codeBlock;

    private void Awake()
    {
        GameObject block = Instantiate(codeBlock, transform.position, Quaternion.identity); // instantiate new code block when the program starts
        block.transform.SetParent(transform.parent); // the new codeblock is set as the child of original code block
    }

    public void OnPointerDown(PointerEventData eventData) // Function called when mouse is pressed on top of this object
    {
        GameObject block = Instantiate(codeBlock, new Vector3(eventData.pressPosition.x, eventData.pressPosition.y, 0), Quaternion.identity); // when the mouse is pressed on top of a codeblock a new code block is instantiated on the same position
        block.transform.SetParent(transform.parent); // the new codeblock is set as the child of original code block
    }
}
