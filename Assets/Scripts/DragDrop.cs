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
 * Handles command instructions, right, left, jumpright, jumpleft
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
    
    public Vector3 positionToMoveTo;

    [SerializeField] private BlockType blockType;

    private PlayerMovement player;
    
    private enum BlockType // create the types of blocks we are using 
    {
        Derecha,
        Izquierda,
        SaltarDer,
        SaltarIzq
    };
    

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPos = rectTransform.anchoredPosition;
    }

    private void Start()
    {
        player = PlayerMovement.instance;    
    }

    public void OnBeginDrag(PointerEventData eventData) // Function called when we begin dragging object
    {
        canvasGroup.blocksRaycasts = false; // This line makes the raycast go through this object and land on the CodeSlot
        onSlot = false;
        dragged = true;
        rectTransform.position = Input.mousePosition;
        transform.SetSiblingIndex(-1); // Move the CodeBlock to the bottom of the hierarchy 
        
        if (codeSlot != null) // Will remove CodeSlot.data if this code block has been assigned to a code slot
        {
            codeSlot.removeData();
            codeSlot = null;
        }
    }

    public void OnDrag(PointerEventData eventData) // Function called every frame while the object is being dragged
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // eventData.delta contains the amount the mouse moved since the last frame. Divided by canvas.scaleFactor to match movement with any screen size
    }

    public void OnEndDrag(PointerEventData eventData) // Function called when we end dragging object
    {
        canvasGroup.blocksRaycasts = true; // Return to true so that this object can now receive events
        if (!onSlot)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData) // Function called when mouse is pressed on top of this object
    {
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
        }
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

    public void Run() // main run function called when the green play button is pressed
    {
        // switch used to handle all of the code block cases and calling the apropriate coroutine
        switch (blockType) 
        {
            case BlockType.Derecha:
                StartCoroutine(DerechaRoutine(1));
                break;
            case BlockType.Izquierda:
                StartCoroutine(IzqRoutine(1));
                break;
            case BlockType.SaltarDer:
                StartCoroutine(SaltoDerRoutine());
                break;
            case BlockType.SaltarIzq:
                StartCoroutine(SaltoIzqRoutine());
                break;
        }
    }

    IEnumerator DerechaRoutine(float duration) // coroutine for right code block
    {
        float time = 0;

        player.sprite.flipX = false; // we makle sure not to flip sprite, because player is moving right
        while (time < duration) // while elapsed time is less than the duration of our command duration keep going
        {
            player.animation.SetBool(PlayerMovement.Walking, true); // start walking animation
            player.transform.Translate(new Vector3(8, 0, 0) * Time.deltaTime, Space.World); // every loop transale 8 spaceworld units
            time += Time.deltaTime; // increment time by the time between frames
            yield return null;
        }
        
        player.animation.SetBool(PlayerMovement.Walking, false); // stop walking animation
    }
    
    IEnumerator IzqRoutine(float duration) // coroutine for left code block same logic as right block but on opposite direction
    {
        float time = 0;

        player.sprite.flipX = true;
        while (time < duration)
        {
            player.animation.SetBool(PlayerMovement.Walking, true);
            player.transform.Translate(new Vector3(-8, 0, 0) * Time.deltaTime, Space.World);
            time += Time.deltaTime;
            yield return null;
        }
        
        player.animation.SetBool(PlayerMovement.Walking, false);
    }

    IEnumerator SaltoDerRoutine() // coroutine for jumpright code block
    {
        player.sprite.flipX = false;
        if (player.isOnGround) // if the player is on ground
        {
            player.animation.SetBool(PlayerMovement.Jumped, true); // set jumping animation to true
            player.rigidbody.AddForce(new Vector2(6, 15), ForceMode2D.Impulse); // impulse the rigidbody of the player up and to the right

            yield return null;

            player.animation.SetBool(PlayerMovement.Walking, false); // set jumping animation to false
        }
    }
    
    IEnumerator SaltoIzqRoutine() // coroutine for jumpleft code block same logic as jumpright block but on opposite direction
    {
        player.sprite.flipX = true;
        if (player.isOnGround)
        {
            player.animation.SetBool(PlayerMovement.Jumped, true); 
            player.rigidbody.AddForce(new Vector2(-6, 15), ForceMode2D.Impulse);

            yield return null;

            player.animation.SetBool(PlayerMovement.Walking, false);
        }
    }
    
}
