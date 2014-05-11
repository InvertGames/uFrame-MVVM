using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the component of Type type if the game object has one attached, null if it doesn't. You can access both builtin components or scripts with this function.")]
[UBCategory("GameObject")]
public class GetComponentByType : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBString _Type = new UBString();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Component));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _GameObject.GetValue(context).GetComponent(_Type.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s GetComponent w/ {1}", _GameObject.ToString(RootContainer), _Type.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.GetComponent(#_Type#)");
	}

}