using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true if there is any collider intersecting the line between start and end.")]
[UBCategory("Physics")]
public class LinecastByStartAndEnd : UBConditionAction {

	
	[UBRequired] public UBVector3 _Start = new UBVector3();
	
	[UBRequired] public UBVector3 _End = new UBVector3();
	public override bool PerformCondition(IUBContext context){
return Physics.Linecast(_Start.GetValue(context),_End.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Physics.Linecast(#_Start#, #_End#)");
	}

}