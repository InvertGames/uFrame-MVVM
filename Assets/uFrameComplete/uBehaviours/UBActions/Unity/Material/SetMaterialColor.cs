using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The main material's color.")]
[UBCategory("Material")]
public class SetMaterialColor : UBAction {

	[UBRequired] public UBMaterial _Material = new UBMaterial();
	[UBRequired] public UBColor _Value = new UBColor();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Material.GetValue(context).color = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s color to {1}", _Material.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Material#.color = #_Value# ");
	}

}