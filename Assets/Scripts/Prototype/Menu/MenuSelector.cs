using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuSelector : MonoBehaviour
{
    public MenuSelector nextSelector;
    public MenuSelector previousSelector;

    public virtual void OnSelected() { }
    public virtual void OnDeselected() { }

    public abstract void NextOption();
    public abstract void PreviousOption();
}

public abstract class MenuSelector<T> : MenuSelector {

    
    public abstract T GetResult();
	
}
