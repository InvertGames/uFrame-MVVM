using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"This is used to render parts of the scene selectively.")]
[UBCategory("Camera")]
public class SetCameraCullingMask : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Camera.GetValueAs<Camera>(context).cullingMask = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s cullingMask to {1}", _Camera.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.cullingMask = #_Value# ");
	}

}