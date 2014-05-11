using System.Collections.Generic;
using UnityEngine;

public abstract class UBInstanceTrigger : UBTrigger
{
    [SerializeField]
    private UBActionSheet _sheet;

    public virtual string DisplayName
    {
        get;
        set;
    }

    public override UBActionSheet Sheet
    {
        get { return _sheet; }
        set { _sheet = value; }
    }

    protected UBInstanceTrigger()
    {
        
    }
}