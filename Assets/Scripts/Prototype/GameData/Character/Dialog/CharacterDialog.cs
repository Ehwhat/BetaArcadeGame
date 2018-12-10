using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Characters/Dialog")]
[System.Serializable]
public class CharacterDialog : ScriptableObject {

    public CharacterDefinition speaker;
    public DialogContext dialogContext = DialogContext.GameStart;
    public int priority = 0;
    public bool useDialogConditional = false;
    public string dialogConditional = "";

    [TextArea]
    public string dialogText = "Dialog is empty!";

    public CharacterDialog chainedDialog;
}
