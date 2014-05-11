using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The tag of this game object.")]
[UBCategory("GameObject")]
public class GetGameObjectTag : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _GameObject.GetValue(context).tag);
		}

	}

	public override string ToString(){
	return string.Format("Get tag from {0}", _GameObject.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_GameObject#.tag");
	}

}