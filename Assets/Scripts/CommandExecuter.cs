using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.Scripting;
using UnityEngine;

public class CommandExecuter : MonoBehaviour
{
    [SerializeField] private CodeSlot head;
    [SerializeField] private Canvas blockCanvas;
    
    private PlayerMovement player;
    private bool playing = false;
    public int numberDeaths = 0;
    public bool gameOver = false;
    public int numberCommands = 0;
    public CanvasGroup blockRaycast;

    private void Start()
    {
        blockRaycast = blockCanvas.GetComponent<CanvasGroup>();
        player = PlayerMovement.instance;
    }
    public void updateList(CodeSlot slot) // Called when a CodeBlock is removed or added
    {
        CodeSlot current = slot;
        while (current.next)
        {
            // Traverse the CodeSlots (Nodes)
            current = current.next;
            current.previous.data = current.data; // Previous CodeSlot CodeBlock = the current CodeSlot CodeBlock
            if (current.previous.data) // Check if the previous CodeSlot has data
            {
                current.previous.data.GetComponent<DragDrop>().codeSlot = current.previous; // Changing the CodeBlock CodeSlot to the previous CodeSlot
                current.previous.moveCodeBlock(); // Moving CodeBlock into position
            }
        }

        current.previous.next = null; // Making the previous CodeSlot point to null
        Destroy(current.gameObject); // Destroy last CodeSlot
    }

    public IEnumerator ExecuteRoutine()
    {
        blockRaycast.blocksRaycasts = true;
        CodeSlot current = head;
        while (current.data && !gameOver)
        {
            numberCommands++;
            current.data.GetComponent<DragDrop>().Run();

            yield return new WaitForSeconds(1.5f);
            
            current = current.next;
        }

        yield return new WaitForSeconds(1f);
        
        if (!gameOver)
        {
            player.transform.position = player.startingPos;
        }
        blockRaycast.blocksRaycasts = false;
    }
    
    public void Execute()
    {
        gameOver = false;
        StartCoroutine(ExecuteRoutine());
    }
}
