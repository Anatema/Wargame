using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class PlayerState
{
    public string StateName;
    protected PlayerController PlayerController;
    public abstract void EnterState();
    public abstract void Update();
    public abstract void EndState();


}
