using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class CommandExecuter : MonoBehaviour
{
    [SerializeField] private CodeSlot head;
    private bool playing = false;
    public bool gameOver = false;

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
        CodeSlot current = head;
        while (current.data && !gameOver)
        {
            current.data.GetComponent<DragDrop>().Run();

            yield return new WaitForSeconds(1f);
            
            current = current.next;
        }
        playing = false;
    }

    public void Execute()
    {
        gameOver = false;
        if (!playing)
        {
            StartCoroutine(ExecuteRoutine());
            playing = true;
        }
    }
    
}
