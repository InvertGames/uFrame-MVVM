using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class SetPlayerPrefsFloat : UBAction
{

	public UBString _Key = new UBString();
	public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		PlayerPrefs.SetFloat(_Key.GetValue(context),_Value.GetValue(context))		;
	}

}