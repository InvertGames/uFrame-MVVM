using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The main material's color.")]
[UBCategory("Material")]
public class GetMaterialColor : UBAction {

	[UBRequired] public UBMaterial _Material = new UBMaterial();
	[UBRequireVariable] [UBRequired] public UBColor _Result = new UBColor();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Material.GetValue(context).color);
		}

	}

	public override string ToString(){
	return string.Format("Get color from {0}", _Material.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Material#.color");
	}

}