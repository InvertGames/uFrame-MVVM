using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GUIText")]
public class GetGUITextRichText : UBConditionAction {

	[UBRequired] public UBObject _GUIText = new UBObject(typeof(GUIText));
	public override bool PerformCondition(IUBContext context){
return _GUIText.GetValueAs<GUIText>(context).richText		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_GUIText#.richText");
	}

}