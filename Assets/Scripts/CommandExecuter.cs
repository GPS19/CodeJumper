using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.Scripting;
using UnityEngine;

/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script to excute all of the players 
 * instructions
 */

public class CommandExecuter : MonoBehaviour
{
    [SerializeField] private CodeSlot head;
    [SerializeField] private Canvas blockCanvas;
    
    private PlayerMovement player;
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
        blockRaycast.blocksRaycasts = true; // When movement commands are being executed, we block the raycast so that the user can't alter the commands
        CodeSlot current = head;
        while (current.data && !gameOver) // While there are commands to run and the player hasn't lost, commands keep being executed
        {
            numberCommands++; // Track number of comands used
            current.data.GetComponent<DragDrop>().Run(); // Get command to be executed

            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds to execute next command
            
            current = current.next; // make current command = next command
        }

        yield return new WaitForSeconds(1f); // wait for one second
        
        if (!gameOver) // if the player didnt reach the goal with the number of commands used
        {
            player.transform.position = player.startingPos; // reset characters position
            numberCommands = 0; // reset command count
            numberDeaths++; // increment number of deaths
        }
        blockRaycast.blocksRaycasts = false; // when all of the commands have been executed and all cases have been handled, stop blocking raycasts so the user can keep playing
    }
    
    public void Execute() // main function, called when green play button is pressed, execute all user commands
    {
        gameOver = false;
        StartCoroutine(ExecuteRoutine());
    }
}
