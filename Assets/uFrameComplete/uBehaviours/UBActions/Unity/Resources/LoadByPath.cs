using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Loads an asset stored at path in the Resources folder.")]
[UBCategory("Resources")]
public class LoadByPath : UBAction {

	
	[UBRequired] public UBString _Path = new UBString();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Resources.Load(_Path.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Load Resouce {0}", _Path.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Resources.Load(#_Path#)");
	}

}