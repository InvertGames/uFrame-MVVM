using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Clones the object original and returns the clone.")]
[UBCategory("Object")]
public class InstantiateByOriginal : UBAction {

	
	[UBRequired] public UBObject _Original = new UBObject();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Object.Instantiate(_Original.GetValue(context)));
		}

	}

	public override string ToString(){
	    if (RootContainer == null)
	    {
	        Debug.Log("ORIGINAL IS NULL");
	    }
	return string.Format("Call {0}'s Instantiate w/ {1}", "Object", _Original.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Object.Instantiate(#_Original#)");
	}

}