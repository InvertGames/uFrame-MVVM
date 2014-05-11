using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true during the frame the user pressed down the virtual button identified by buttonName.")]
[UBCategory("Input")]
public class GetButtonDown : UBConditionAction {

	
	[UBRequired] public UBString _ButtonName = new UBString();
	public override bool PerformCondition(IUBContext context){
        return Input.GetButtonDown(_ButtonName.GetValue(context))		;

	}

    public override string ToString()
    {
        return string.Format("Is {0} Button Down.", _ButtonName.ToString(RootContainer));
    }

    public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetButtonDown(#_ButtonName#)");
	}

}