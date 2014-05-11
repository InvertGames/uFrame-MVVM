using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The name of the object.")]
[UBCategory("Object")]
public class GetObjectName : UBAction {

	[UBRequired] public UBObject _Object = new UBObject();
	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Object.GetValue(context).name);
		}

	}

	public override string ToString(){
	return string.Format("Get {0}'s Object Name", _Object.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Object#.name");
	}

}