using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Where on the screen is the camera rendered in pixel coordinates.")]
[UBCategory("Camera")]
public class GetCameraPixelRect : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequireVariable] [UBRequired] public UBRect _Result = new UBRect();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Camera.GetValueAs<Camera>(context).pixelRect);
		}

	}

	public override string ToString(){
	return string.Format("Get pixelRect from {0}", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Camera#.pixelRect");
	}

}