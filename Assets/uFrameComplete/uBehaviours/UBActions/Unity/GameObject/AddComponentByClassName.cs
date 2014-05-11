using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Adds a component class named className to the game object.")]
[UBCategory("GameObject")]
public class AddComponentByClassName : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBString _ClassName = new UBString();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Component));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _GameObject.GetValue(context).AddComponent(_ClassName.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s AddComponent w/ {1}", _GameObject.ToString(RootContainer), _ClassName.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.AddComponent(#_ClassName#)");
	}

}