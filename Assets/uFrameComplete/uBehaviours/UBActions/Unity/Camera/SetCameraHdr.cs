using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Camera")]
public class SetCameraHdr : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Camera.GetValueAs<Camera>(context).hdr = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s hdr to {1}", _Camera.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.hdr = #_Value# ");
	}

}