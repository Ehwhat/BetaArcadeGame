using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Singletons/CharacterDialogSystem")]
public class CharacterDialogSystem : ScriptableObject {

    [System.NonSerialized]
    static int currentPriority;
    [System.NonSerialized]
    static Coroutine currentChain;

    public QuipSystemDefinition[] displayers = new QuipSystemDefinition[4];

    [RuntimeInitializeOnLoadMethod]
    private static void OnInit()
    {
        currentChain = null;
        currentPriority = -1;
    }

    public void SayDialogFor(int playerID,CharacterDefinition character, DialogContext context, string conditional = "")
    {
        CharacterDialogDefinition dialogDefinition = character.dialogDefinition;
        List<CharacterDialog> relevantDialog = dialogDefinition.dialogDictionary[context];

        Debug.Log(relevantDialog.Count);

        for (int i = 0; i < relevantDialog.Count; i++)
        {
            if (relevantDialog[i].useDialogConditional && relevantDialog[i].dialogConditional.ToLower() != conditional.ToLower()) 
            {
                relevantDialog.RemoveAt(i);
            }
        }

        if (relevantDialog.Count > 0)
        {
            CharacterDialog nextDialog = relevantDialog[Random.Range(0, relevantDialog.Count - 1)];
            if (nextDialog.priority >= currentPriority)
            {
                if (currentChain != null)
                {
                    CoroutineServer.StopCoroutine(currentChain);
                }
                currentPriority = nextDialog.priority;
                currentChain = CoroutineServer.StartCoroutine(StartDialogChain(playerID, nextDialog));
            }

        }
    }

    private IEnumerator StartDialogChain(int player, CharacterDialog nextDialog)
    {
        while (nextDialog != null)
        {
            string dialog = nextDialog.dialogText;
            float delay = displayers[player].SayQuip(dialog);
            yield return new WaitForSeconds(delay);
            nextDialog = nextDialog.chainedDialog;
        }
        currentChain = null;
        currentPriority = -1;
    }

}
