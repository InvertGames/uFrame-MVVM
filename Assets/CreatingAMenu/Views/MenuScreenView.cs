using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public abstract partial class MenuScreenView {
    
    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(true);
    }
    public override void ActiveChanged(bool value)
    {
        base.ActiveChanged(value);
        gameObject.SetActive(value);
    }
}
