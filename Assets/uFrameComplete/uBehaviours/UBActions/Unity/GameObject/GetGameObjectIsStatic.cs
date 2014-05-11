using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GameObject")]
public class GetGameObjectIsStatic : UBConditionAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	public override bool PerformCondition(IUBContext context){
return _GameObject.GetValue(context).isStatic		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_GameObject#.isStatic");
	}

}