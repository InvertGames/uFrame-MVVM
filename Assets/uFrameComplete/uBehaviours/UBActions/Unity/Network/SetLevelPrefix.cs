using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the level prefix which will then be prefixed to all network ViewID numbers.")]
[UBCategory("Network")]
public class SetLevelPrefix : UBAction {

	
	[UBRequired] public UBInt _Prefix = new UBInt();
	protected override void PerformExecute(IUBContext context){
		Network.SetLevelPrefix(_Prefix.GetValue(context));
	}

	public override string ToString(){
	    return string.Format("Set Level Prefix to {0}", _Prefix.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.SetLevelPrefix(#_Prefix#)");
	}

}