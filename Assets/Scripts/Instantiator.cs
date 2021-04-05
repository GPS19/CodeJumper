using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Instantiator : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject codeBlock;

    private void Awake()
    {
        GameObject block = Instantiate(codeBlock, transform.position, Quaternion.identity);
        block.transform.SetParent(transform.parent);
    }

    public void OnPointerDown(PointerEventData eventData) // Function called when mouse is pressed on top of this object
    {
        Debug.Log("OnPointerDown");
        GameObject block = Instantiate(codeBlock, new Vector3(eventData.pressPosition.x, eventData.pressPosition.y, 0), Quaternion.identity);
        block.transform.SetParent(transform.parent);
    }
}
