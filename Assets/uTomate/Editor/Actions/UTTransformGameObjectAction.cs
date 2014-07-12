using UnityEngine;
using System.Collections;
using UnityEditor;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Transform Game Object", description="Transform game object.")]
[UTDefaultAction]
public class UTTransformGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object that should be transformed.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="Should the position be set to an absolute value?")]
	[UTInspectorHint(order=1)]
	public UTBool positionAbsolute;
	[UTDoc(description="The position to which the object should be moved.")]
	[UTInspectorHint(order=2)]
	public UTVector3 position;
	[UTDoc(description="Should the rotation be set to an absolute value?")]
	[UTInspectorHint(order=3)]
	public UTBool rotationAbsolute;
	[UTDoc(description="The rotation to which the object should be rotated.")]
	[UTInspectorHint(required=true, order=4)]
	public UTVector3 rotation;
	[UTDoc(description="Should the scale be set to an absolute value?")]
	[UTInspectorHint(order=5)]
	public UTBool scaleAbsolute;
	[UTDoc(description="The scale to which the object should be scaled.")]
	[UTInspectorHint(required=true, order=6)]
	public UTVector3 scale;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object to transform.", this);
		}

		if (positionAbsolute.EvaluateIn (context)) {
			theGameObject.transform.position = position.EvaluateIn (context);
		} else {
			theGameObject.transform.position += position.EvaluateIn (context);
		}

		if (rotationAbsolute.EvaluateIn (context)) {
			theGameObject.transform.localRotation = Quaternion.Euler (rotation.EvaluateIn (context));
		} else {
			theGameObject.transform.localRotation = Quaternion.Euler (theGameObject.transform.localEulerAngles + rotation.EvaluateIn (context));
		}
		
		if (scaleAbsolute.EvaluateIn (context)) {
			theGameObject.transform.localScale = scale.EvaluateIn (context);
		} else {
			theGameObject.transform.localScale = Vector3.Scale (theGameObject.transform.localScale, scale.EvaluateIn (context));
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Transform Game Object",  false, 520)]
	public static void AddAction ()
	{
		var theAction = Create<UTTransformGameObjectAction> ();
		
		var theScale = new UTVector3 ();
		theScale.Value = Vector3.one;
		theScale.UseExpression = false;
		theAction.scale = theScale;
	}


}
