using System;
using UnityEngine;

public class UTNameFilter : UTFilter
{
	
	private string[] names;
	
	public UTNameFilter (string[] names)
	{
		this.names = names;
	}
	
	public bool Accept (object o)
	{
		GameObject go = o as GameObject;
		if (go == null) {
			return false;
		}
		
		if (names == null || names.Length == 0) {
			return false;
		}
		
		foreach (var name in names) {
			if (string.Equals (go.name, name, StringComparison.InvariantCultureIgnoreCase)) {
				return true;
			}
		}
		return false;
	}
}

