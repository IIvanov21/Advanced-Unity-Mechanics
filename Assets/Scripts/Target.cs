using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //An event which is accessible in the inspector
    public event Action<Target> OnDestroyed;

    //Simply on destroy invoke OnDestroyed event
    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);    
    }
}
