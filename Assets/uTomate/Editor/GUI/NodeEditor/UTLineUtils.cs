//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Helper class for drawing connector lines.
/// </summary>
public class UTLineUtils
{

	public static Texture2D lineTex;
 	
	public static void DrawLine (Vector2 pointA, Vector2 pointB)
	{
		DrawLine (pointA, pointB, Color.black, 1.5f);
	}
		
	public static void DrawLine (Vector2 pointA, Vector2 pointB, Color color, float width)
	{
		var start = new Vector3 (pointA.x, pointA.y);
		var end = new Vector3 (pointB.x, pointB.y);
		
		var startHandle = start; //new Vector2(Mathf.Max(pointA.x, pointB.x), Mathf.Min(pointA.y, pointB.y));
		var endHandle = end; //new Vector2(Mathf.Max(pointA.x, pointB.x), Mathf.Max (pointA.y, pointB.y));
		Handles.DrawBezier (start, end, startHandle, endHandle, color, UTEditorResources.LineTexture, width);
	}
}
