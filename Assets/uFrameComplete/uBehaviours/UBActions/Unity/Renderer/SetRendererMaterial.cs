using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The material of this object.")]
[UBCategory("Renderer")]
public class SetRendererMaterial : UBAction {

	[UBRequired] public UBObject _Renderer = new UBObject(typeof(Renderer));
	[UBRequired] public UBMaterial _Value = new UBMaterial();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Renderer.GetValueAs<Renderer>(context).material = _Value.GetValue(context);
		}

	}

	public override string ToString()
	{
	    //return "Set Material";
	return string.Format("Set {0}'s material to {1}", _Renderer.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Renderer#.material = #_Value# ");
	}

}