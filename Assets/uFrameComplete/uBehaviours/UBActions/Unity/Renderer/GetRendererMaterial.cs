using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The material of this object.")]
[UBCategory("Renderer")]
public class GetRendererMaterial : UBAction {

	[UBRequired] public UBObject _Renderer = new UBObject(typeof(Renderer));
	[UBRequireVariable] [UBRequired] public UBMaterial _Result = new UBMaterial();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Renderer.GetValueAs<Renderer>(context).material);
		}

	}

	public override string ToString(){
	return string.Format("Get material from {0}", _Renderer.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Renderer#.material");
	}

}