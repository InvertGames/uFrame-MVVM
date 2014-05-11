using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GUI")]
public class ScrollTowards : UBConditionAction {

	[UBRequired] public UBRect _Position = new UBRect();
	[UBRequired] public UBFloat _MaxDelta = new UBFloat();
	public override bool PerformCondition(IUBContext context){
return GUI.ScrollTowards(_Position.GetValue(context),_MaxDelta.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("GUI.ScrollTowards(#_Position#, #_MaxDelta#)");
	}

}