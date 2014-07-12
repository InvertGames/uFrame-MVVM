using System;
using UnityEngine;

public class UTTagFilter : UTFilter
{
	
	private string[] tags;
	
	public UTTagFilter (string[] tags)
	{
		this.tags = tags;
	}
	
	public bool Accept (object o)
	{
		GameObject go = o as GameObject;
		if (go == null) {
			return false;
		}
		
		if (tags == null || tags.Length == 0) {
			return false;
		}
		
		foreach (var tag in tags) {
			if (string.Equals (go.tag, tag, StringComparison.InvariantCultureIgnoreCase)) {
				return true;
			}
		}
		return false;
	}
}

