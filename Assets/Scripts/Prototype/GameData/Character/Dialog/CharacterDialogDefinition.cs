using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogContext
{
    GameStart,
    WeaponPickup,
    TakingDamage,
    Win,
    Lose
}

[System.Serializable]
public class CharacterDialogList {
    public List<CharacterDialog> list = new List<CharacterDialog>();
}

[CreateAssetMenu(menuName = "Tanks/Characters/Dialog Definition")]
[System.Serializable]
public class CharacterDialogDefinition : ScriptableObject, ISerializationCallbackReceiver {

    public Dictionary<DialogContext, List<CharacterDialog>> dialogDictionary = new Dictionary<DialogContext, List<CharacterDialog>>();

    public static string PROPERTY_EDITOR_DIALOG_LIST = "editorDialogLists";
    [SerializeField]
    private List<CharacterDialogList> editorDialogLists = new List<CharacterDialogList>();

    public void OnEnable()
    {
        var values = System.Enum.GetValues(typeof(DialogContext));
        for (int i = 0; i < values.Length; i++)
        {
            dialogDictionary.Add((DialogContext)i, new List<CharacterDialog>());
        }
    }

    public string GetCharacterDialog(DialogContext context, string conditional = "")
    {
        return "";
    }

    public void OnAfterDeserialize()
    {
        dialogDictionary = new Dictionary<DialogContext, List<CharacterDialog>>();
        for (int i = 0; i < editorDialogLists.Count; i++)
        {
            dialogDictionary.Add((DialogContext)i, new List<CharacterDialog>());
            for (int j = 0; j < editorDialogLists[i].list.Count; j++)
            {
                dialogDictionary[(DialogContext)i].Add(editorDialogLists[i].list[j]);
            }
            
        }
    }

    public void OnBeforeSerialize()
    {
        editorDialogLists = new List<CharacterDialogList>();
        for (int i = 0; i < dialogDictionary.Keys.Count; i++)
        {
            editorDialogLists.Add(new CharacterDialogList());
        }
        foreach (var pair in dialogDictionary)
        {
            for (int i = 0; i < pair.Value.Count; i++)
            {
                editorDialogLists[(int)pair.Key].list.Add(pair.Value[i]);
            }
        }
    }
}
