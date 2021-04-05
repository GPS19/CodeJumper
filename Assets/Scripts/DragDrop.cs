using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script to handle drag and drop actions.
 * Mainly used on CodeBlock objects.
 */

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler, IDropHandler
{
    private Canvas canvas;
    [SerializeField] private GameObject codeBlock;
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 initialPos;
    private bool onSlot;
    private bool dragged = false;
    public CodeSlot codeSlot = null;

    [SerializeField] private BlockType blockType;

    private PlayerMovement player;
    //public Transform backgroundLayer1;
    //public float backgroundMoveScale = 1f;
    
    private enum BlockType
    {
        Derecha,
        Izquierda,
        Saltar,
    };
    

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPos = rectTransform.anchoredPosition;
        Debug.Log(initialPos);
    }

    private void Start()
    {
        player = PlayerMovement.instance;    
    }

    public void OnBeginDrag(PointerEventData eventData) // Function called when we begin dragging object
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false; // This line makes the raycast go through this object and land on the CodeSlot
        onSlot = false;
        dragged = true;
        rectTransform.position = Input.mousePosition;
        transform.SetSiblingIndex(-1); // Move the CodeBlock to the bottom of the hierarchy 
        
        if (codeSlot != null) // Will remove CodeSlot.data if this code block has been assigned to a code slot
        {
            Debug.Log("Check");
            codeSlot.removeData();
            codeSlot = null;
        }
    }

    public void OnDrag(PointerEventData eventData) // Function called every frame while the object is being dragged
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // eventData.delta contains the amount the mouse moved since the last frame. Divided by canvas.scaleFactor to match movement with any screen size
    }

    public void OnEndDrag(PointerEventData eventData) // Function called when we end dragging object
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true; // Return to true so that this object can now receive events
        if (!onSlot)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData) // Function called when mouse is pressed on top of this object
    {
        Debug.Log("OnPointerDown Drag");
        if (!onSlot)
        {
            GameObject block = Instantiate(codeBlock, transform.position, Quaternion.identity); // Instantiating new CodeBlock
            block.transform.SetParent(transform.parent); // Setting new CodeBlock as a child of the canvas
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!onSlot && !dragged) 
        {
            Destroy(gameObject); // Destroy CodeBlock if it was placed on an invalid position
            Debug.Log("OnPointerUp");
        }
        Debug.Log("OnPointerUp");
        dragged = false;
    }

    public void setOnSlot(bool _onSlot) // Set if CodeBlock is on a CodeSlot
    {
        onSlot = _onSlot;
    }

    public void OnDrop(PointerEventData eventData) // Used to swap CodeBlock for another CodeBlock inside a CodeSlot
    {
        if (onSlot)
        {
            codeSlot.data = eventData.pointerDrag; // Set CodeSlot data to dragged CodeBlock
            codeSlot.moveCodeBlock(); // Set new CodeBlock into CodeSlot
            codeSlot.data.GetComponent<DragDrop>().setOnSlot(true); 
            codeSlot.data.GetComponent<DragDrop>().codeSlot = codeSlot; // Setting new CodeBlock's CodeSlot
                
            Destroy(gameObject); // Destroy previous CodeBlock 
        }
    }

    public void Run()
    {
        switch (blockType)
        {
            case BlockType.Derecha:
                Debug.Log("Derecha");
                StartCoroutine(DerechaRoutine());
                break;
            case BlockType.Izquierda:
                Debug.Log("Izquierda");
                StartCoroutine(IzqRoutine());
                break;
            case BlockType.Saltar:
                Debug.Log("Saltar");
                StartCoroutine(SaltoRoutine());
                break;
        }
    }

    IEnumerator DerechaRoutine()
    {
        player.sprite.flipX = false;
        for (int i = 0; i < 50; i++)
        {
            player.animation.SetBool(PlayerMovement.Walking, true);
            player.transform.Translate(new Vector3(5, 0, 0) * player.speed * Time.deltaTime);
            
            //backgroundLayer1.Translate(new Vector3(-1, 0, 0) * backgroundMoveScale * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        player.animation.SetBool(PlayerMovement.Walking, false);
    }
    
    IEnumerator IzqRoutine()
    {
        player.sprite.flipX = true;
        for (int i = 0; i < 50; i++)
        {
            player.animation.SetBool(PlayerMovement.Walking, true);
            player.transform.Translate(new Vector3(-5, 0, 0) * player.speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        player.animation.SetBool(PlayerMovement.Walking, false);
    }

    IEnumerator SaltoRoutine()
    {
        if (player.isOnGround)
        {
            player.animation.SetBool(PlayerMovement.Jumped, true); 
            player.rigidbody.AddForce(Vector3.up * player.jumpHeight, ForceMode2D.Impulse);

            yield return new WaitForEndOfFrame();

            player.animation.SetBool(PlayerMovement.Walking, false);
        }
    }
    
}
