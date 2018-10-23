using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankController : ScriptableObject {

    public abstract object Process(TankManager manager, object state = null);
	
}

