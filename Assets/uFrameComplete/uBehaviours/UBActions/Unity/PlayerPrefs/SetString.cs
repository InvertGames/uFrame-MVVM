using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class SetPlayerPrefsString : UBAction
{
	public UBString _Key = new UBString();
	public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		PlayerPrefs.SetString(_Key.GetValue(context),_Value.GetValue(context))		;
	}
}