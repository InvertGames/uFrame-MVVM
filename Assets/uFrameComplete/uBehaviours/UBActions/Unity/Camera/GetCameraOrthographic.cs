using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Is the camera orthographic (true) or perspective (false)?")]
[UBCategory("Camera")]
public class GetCameraOrthographic : UBConditionAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	public override bool PerformCondition(IUBContext context){
return _Camera.GetValueAs<Camera>(context).orthographic		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Camera#.orthographic");
	}

}