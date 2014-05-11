using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Is this game object tagged with tag?")]
[UBCategory("GameObject")]
public class CompareTag : UBConditionAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBString _Tag = new UBString();
	public override bool PerformCondition(IUBContext context){
return _GameObject.GetValue(context).CompareTag(_Tag.GetValue(context))		;

	}

    public override string ToString()
    {
        return string.Format("Compare {0}'s tag with {1}",_GameObject.ToString(RootContainer),_Tag.ToString(RootContainer));
    }

    public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_GameObject#.CompareTag(#_Tag#)");
	}

}