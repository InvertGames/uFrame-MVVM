using System;
using UnityEngine;

public class UTLayerFilter : UTFilter
{
	
	private int[] layers;
	
	public UTLayerFilter (int[] layers)
	{
		this.layers = layers;
	}
	
	public bool Accept (object o)
	{
		GameObject go = o as GameObject;
		if (go == null) {
			return false;
		}
		
		if (layers == null || layers.Length == 0) {
			return false;
		}
		
		foreach (var layer in layers) {
			if (go.layer == layer) {
				return true;
			}
		}
		return false;
	}
}

