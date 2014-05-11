using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Finds a game object by name and returns it.")]
[UBCategory("GameObject")]
public class Find : UBAction
{


    [UBRequired]
    public UBString _Name = new UBString();
    [UBRequireVariable]
    [UBRequired]
    public UBGameObject _Result = new UBGameObject();
    protected override void PerformExecute(IUBContext context)
    {
        if (_Result != null)
        {
            _Result.SetValue(context, GameObject.Find(_Name.GetValue(context)));
        }

    }

    public override string ToString()
    {
        return string.Format("Find {0} GameObject", _Name.ToString(RootContainer));
    }

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("GameObject.Find(#_Name#)");
    }

}