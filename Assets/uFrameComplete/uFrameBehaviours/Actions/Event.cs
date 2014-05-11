using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("ViewBase")]
public class ViewEvent : UBAction
{

    public UBObject _ViewBase = new UBObject(typeof(ViewBase));
    public UBString _Eventname = new UBString();
    protected override void PerformExecute(IUBContext context)
    {

    }

}