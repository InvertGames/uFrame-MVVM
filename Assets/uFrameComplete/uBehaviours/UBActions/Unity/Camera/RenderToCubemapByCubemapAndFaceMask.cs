using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Render into a cubemap from this camera.")]
[UBCategory("Camera")]
public class RenderToCubemapByCubemapAndFaceMask : UBConditionAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	
	[UBRequired] public UBObject _Cubemap = new UBObject(typeof(RenderTexture));
	
	[UBRequired] public UBInt _FaceMask = new UBInt();
	public override bool PerformCondition(IUBContext context){
return _Camera.GetValueAs<Camera>(context).RenderToCubemap(_Cubemap.GetValueAs<RenderTexture>(context),_FaceMask.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Camera#.RenderToCubemap(#_Cubemap#, #_FaceMask#)");
	}

}