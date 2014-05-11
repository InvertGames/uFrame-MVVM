using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class DeletePlayerPrefsKey : UBAction
{

	public UBString _Key = new UBString();
	protected override void PerformExecute(IUBContext context){
		PlayerPrefs.DeleteKey(_Key.GetValue(context))		;
	}

}