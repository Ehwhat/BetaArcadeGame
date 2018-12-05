using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/UI/New Quip System")]
public class QuipSystemDefinition : ScriptableObject {

    QuipDisplayer displayer;

    public void RegisterQuipDisplayer(QuipDisplayer displayer)
    {
        this.displayer = displayer;
    }

    public void SetColour(Color c) 
    {
        if(displayer)
            displayer.ChangeColour(c);
    }

    public float SayQuip(string quip)
    {
        if(displayer)
            return displayer.SayQuip(quip, true);
        return 0;
    }

    

}
