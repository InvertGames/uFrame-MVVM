using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Render into a cubemap from this camera.")]
[UBCategory("Camera")]
public class RenderToCubemapByCubemap : UBConditionAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	
	[UBRequired] public UBObject _Cubemap = new UBObject(typeof(RenderTexture));
	public override bool PerformCondition(IUBContext context){
return _Camera.GetValueAs<Camera>(context).RenderToCubemap(_Cubemap.GetValueAs<RenderTexture>(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Camera#.RenderToCubemap(#_Cubemap#)");
	}

}