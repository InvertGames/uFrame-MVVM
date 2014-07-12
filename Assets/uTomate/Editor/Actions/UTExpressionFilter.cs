using System;
using UnityEngine;

public class UTExpressionFilter : UTFilter
{
	
	private string[] expressions;
	private string gameObjectProperty;
	private UTContext context;
	
	public UTExpressionFilter (string[] expressions, string gameObjectProperty, UTContext context)
	{
		this.expressions = expressions;
		this.gameObjectProperty = gameObjectProperty;
		this.context = context;
	}
	
	public bool Accept (object o)
	{
		GameObject go = o as GameObject;
		if (go == null) {
			return false;
		}
		
		if (expressions == null || expressions.Length == 0) {
			return false;
		}
		
		context[gameObjectProperty] = go;
		
		var result = false;
		foreach (var expression in expressions) {
			var evalResult = context.Evaluate(expression);
			
			if (UTPreferences.DebugMode) {
				Debug.Log("Expression " + expression + " returned with value " + evalResult + " for game object " + go);
			}
			
			if (evalResult is bool && ((bool)evalResult)) {
				result = true;
				break;
			}
		}
		
		context.Unset(gameObjectProperty);
		return result;
	}
}

