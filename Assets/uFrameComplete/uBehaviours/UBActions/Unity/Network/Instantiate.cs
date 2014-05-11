using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Network instantiate a prefab.")]
[UBCategory("Network")]
public class Instantiate : UBAction {

	
	[UBRequired] public UBObject _Prefab = new UBObject();
	
	[UBRequired] public UBVector3 _Position = new UBVector3();
	
	[UBRequired] public UBQuaternion _Rotation = new UBQuaternion();
	
	[UBRequired] public UBInt _Group = new UBInt();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.Instantiate(_Prefab.GetValue(context),_Position.GetValue(context),_Rotation.GetValue(context),_Group.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Network Instantiate {0}", _Prefab.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.Instantiate(#_Prefab#, #_Position#, #_Rotation#, #_Group#)");
	}

}