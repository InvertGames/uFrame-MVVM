using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Transforms position from screen space into viewport space.")]
[UBCategory("Camera")]
public class ScreenToViewportPoint : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	
	[UBRequired] public UBVector3 _Position = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Camera.GetValueAs<Camera>(context).ScreenToViewportPoint(_Position.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Get {0}'s Viewport Point", _Position.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.ScreenToViewportPoint(#_Position#)");
	}

}