using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"How and if camera generates a depth texture.")]
[UBCategory("Camera")]
public class GetCameraDepthTextureMode : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(DepthTextureMode));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Camera.GetValueAs<Camera>(context).depthTextureMode);
		}

	}

	public override string ToString(){
	return string.Format("Get depthTextureMode from {0}", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Camera#.depthTextureMode");
	}

}