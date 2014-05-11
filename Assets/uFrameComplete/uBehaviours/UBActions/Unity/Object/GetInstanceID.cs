using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the instance id of the object.")]
[UBCategory("Object")]
public class GetInstanceID : UBAction {

	[UBRequired] public UBObject _Object = new UBObject();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Object.GetValue(context).GetInstanceID());
		}

	}

	public override string ToString(){
	return string.Format("Get {0}'s InstanceID", _Object.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Object#.GetInstanceID()");
	}

}