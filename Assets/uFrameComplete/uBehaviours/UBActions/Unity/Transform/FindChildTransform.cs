using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Finds a child by name and returns it.")]
[UBCategory("Transform")]
public class FindChildTransform : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBString _Name = new UBString();
	[UBRequireVariable] [UBRequired] public UBTransform _Result = new UBTransform();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).Find(_Name.GetValue(context)));
		}

	}

    public override string ToString()
    {
        return string.Format("Find {0} in {1}", _Name.ToString(RootContainer), _Transform.ToString(RootContainer));
    }

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.Find(#_Name#)");
	}

}