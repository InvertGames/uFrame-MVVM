using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Camera")]
public class GetCameraUseOcclusionCulling : UBConditionAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	public override bool PerformCondition(IUBContext context){
return _Camera.GetValueAs<Camera>(context).useOcclusionCulling		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Camera#.useOcclusionCulling");
	}

}