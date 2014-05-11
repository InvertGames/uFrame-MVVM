using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Creates a game object with a primitive mesh renderer and appropriate collider.")]
[UBCategory("GameObject")]
public class CreatePrimitive : UBAction {

	
	[UBRequired] public UBEnum _Type = new UBEnum(typeof(PrimitiveType));
	[UBRequireVariable] [UBRequired] public UBGameObject _Result = new UBGameObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GameObject.CreatePrimitive(((PrimitiveType)_Type.GetIntValue(context))));
		}

	}

	public override string ToString(){
        return string.Format("Create Primitive {0}", _Type.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GameObject.CreatePrimitive(#_Type#)");
	}

}