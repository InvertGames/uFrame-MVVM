using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class HasPlayerPrefsKey : UBConditionAction
{

	public UBString _Key = new UBString();
	public override bool PerformCondition(IUBContext context){
		return PlayerPrefs.HasKey(_Key.GetValue(context))		;

	}

}