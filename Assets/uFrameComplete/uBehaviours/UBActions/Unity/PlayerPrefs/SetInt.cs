using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class SetPlayerPrefsInt : UBAction
{

	public UBString _Key = new UBString();
	public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		PlayerPrefs.SetInt(_Key.GetValue(context),_Value.GetValue(context))		;
	}

}