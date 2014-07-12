using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;
using UObject = UnityEngine.Object;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTInspectorGroups(groups=new string[]{"General", "Filter Settings"})]
[UTDoc(title="Find Game Objects", description="Finds game objects according to a set filter.")]
[UTDefaultAction]
public class UTFindGameObjectsAction : UTAction
{
	
	[UTDoc(description="The mode for finding game objects.")]
	[UTInspectorHint(required=true, group="General", order=1)]
	public UTFindGameObjectMode mode;
	[UTDoc(description="The name of the input property which contains the game objects to be filtered. Leave empty to use all game objects of the current scene as input.")]
	[UTInspectorHint(group="General", order=2)]
	public UTString inputProperty;
	[UTDoc(description="The name of the output property")]
	[UTInspectorHint(required=true, group="General", order=3)]
	public UTString outputProperty;
	[UTDoc(description="Finds game objects that do not match the given criteria.")]
	[UTInspectorHint (group="General", order=3)]
	public UTBool findNonMatching;
	[UTDoc(description="The list of tags. Game objects having any of these tags will be found.")]
	[UTInspectorHint( arrayNotEmpty = true, group="Filter Settings", order=1)]
	public UTTag[] tags;	
	[UTDoc(description="The list of names. Game objects having any of these names will be found.")]
	[UTInspectorHint( arrayNotEmpty = true, group="Filter Settings", order=2)]
	public UTString[] names;
	[UTDoc(description="The list of layers. Game objects having any of these layers will be found.")]
	[UTInspectorHint( arrayNotEmpty = true, group="Filter Settings", order=3)]
	public UTLayer[] layers;
	[UTDoc(description="The list of static flags. Game objects having any of these static flags will be found.")]
	[UTInspectorHint( arrayNotEmpty = true, group="Filter Settings", order=4)]
	public UTStaticEditorFlags[] staticFlags;
	[UTDoc(description="The active status. Game objects having the same status will be found.")]
	[UTInspectorHint(required=true, group="Filter Settings", order=5)]
	public UTBool activeStatus;
	[UTDoc(description="The list of prefabs. Game objects having any of these prefabs will be found.")]
	[UTInspectorHint( arrayNotEmpty = true, group="Filter Settings", order=6)]
	public UTGameObject[] prefabs;
	[UTDoc(description="The list of components. Game objects having any of these components will be found.")]
	[UTInspectorHint( arrayNotEmpty = true, required=true, group="Filter Settings", order=7, baseType=typeof(Component))]
	public UTType[] components;
	[UTDoc(description="The name of the current game object in the context.")]
	[UTInspectorHint( required=true, group="Filter Settings", order=8)]
	public UTString currentGameObjectProperty;
	[UTDoc(description="The list of expressions. Game objects for wich any of these expressions yields true will be found.")]
	[UTInspectorHint( arrayNotEmpty = true, required=true, group="Filter Settings", containsExpression=true, order=9)]
	public UTString[] expressions;
	[UTDoc(description="The position at which the game object should be found.")]
	[UTInspectorHint(required=true, group="Filter Settings", order=10)]
	public UTVector3 position;
	[UTDoc(description="The distance around the position in which the game object should be found. If this is 0, the game object must be at the exact position (not recommended).")]
	[UTInspectorHint(required=true, group="Filter Settings", order=11)]
	public UTFloat distance;
	
	public override IEnumerator Execute (UTContext context)
	{
		string theOutputProperty = outputProperty.EvaluateIn (context);
		if (string.IsNullOrEmpty (theOutputProperty)) {
			throw new UTFailBuildException ("output property is required.", this);
		}
		
		IEnumerable objects;
		string theInputProperty = inputProperty.EvaluateIn (context);
		if (string.IsNullOrEmpty (theInputProperty)) {
			objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		} else {
			var inputList = context [theInputProperty];
			objects = inputList as IEnumerable;
			if (objects == null) { 
				if (inputList == null) {
					throw new UTFailBuildException ("Property '" + theInputProperty + "' has a null value. Cannot use this as input.", this);
				}
				throw new UTFailBuildException ("Property '" + theInputProperty + "' is of type '" + inputList.GetType () + "'. Cannot use this as input.", this);
			}
		}		 
		
		var filter = GetFilterForMode (context);
		
		var doFindNonMatching = findNonMatching.EvaluateIn(context);
		
		IList result = new ArrayList ();
		var objCount = 0;
		foreach (UObject o in objects) {
			if (o.hideFlags == HideFlags.HideAndDontSave || o.hideFlags == HideFlags.DontSave ) {
				 // skip objects that wouldn't be persisted anyways..
				continue;
			}
			
		
			var prefabType = PrefabUtility.GetPrefabType(o);
			
			if (prefabType == PrefabType.ModelPrefab || prefabType == PrefabType.Prefab) {
				continue; // don't grab any assets.
			}
			
			objCount++;
			
			if (filter.Accept (o) != doFindNonMatching) {
				result.Add (o);
			}
		}
		
		if (UTPreferences.DebugMode) {
			Debug.Log("Filtered " + result.Count + " game objects from " + objCount + ".", this);
		}
		context [theOutputProperty] = result;
		yield return "";
		
	}
	
	private UTFilter GetFilterForMode (UTContext context)
	{
		switch (mode) {
		case UTFindGameObjectMode.ByTag:
			return GetByTagFilter (context);
		case UTFindGameObjectMode.ByName:
			return GetByNameFilter (context);
		case UTFindGameObjectMode.ByLayer:
			return GetByLayerFilter (context);
		case UTFindGameObjectMode.IsStatic:
			return GetIsStaticFilter (context);
		case UTFindGameObjectMode.IsActive:
			return GetIsActiveFilter (context);
		case UTFindGameObjectMode.ByPrefab:
			return GetByPrefabFilter (context);
		case UTFindGameObjectMode.ByComponent:
			return GetByComponentFilter (context);
		case UTFindGameObjectMode.ByExpression:
			return GetByExpressionFilter(context);
		case UTFindGameObjectMode.ByPosition:
			return GetByPositionFilter(context);
		}
		throw new UTFailBuildException ("Unsupported filter type.", this);
	}
	
	private UTFilter GetByTagFilter (UTContext context)
	{
		if (tags.Length == 0) {
			throw new UTFailBuildException ("You need to specify at least one tag to search for.", this);
		}
		return new UTTagFilter (EvaluateAll (tags, context));
	}
	
	private UTFilter GetByNameFilter (UTContext context) {
		if (names.Length == 0) {
			throw new UTFailBuildException ("You need to specify at least one name to search for.", this);
		}
		return new UTNameFilter (EvaluateAll (names, context));
	}
	
	private UTFilter GetByLayerFilter (UTContext context) {
		if (layers.Length == 0) {
			throw new UTFailBuildException ("You need to specify at least one layer to search for.", this);
		}
		return new UTLayerFilter (EvaluateAll (layers, context));
	}
	
	private UTFilter GetIsStaticFilter (UTContext context) {
		if (staticFlags.Length == 0) {
			throw new UTFailBuildException ("You need to specify at least one static flag combination to search for.", this);
		}
		return new UTStaticFlagFilter (EvaluateAll (staticFlags, context));
	}
	
	private UTFilter GetIsActiveFilter (UTContext context) {
		return new UTActiveStatusFilter (activeStatus.EvaluateIn (context));
	}
	
	private UTFilter GetByPrefabFilter (UTContext context) {
		if (prefabs.Length == 0) {
			throw new UTFailBuildException ("You need to specify at least one prefab to search for.", this);
		}
		return new UTPrefabFilter (EvaluateAll (prefabs, context));
	}
	
	private UTFilter GetByComponentFilter (UTContext context) {
		if (components.Length == 0) {
			throw new UTFailBuildException ("You need to specify at least one component to search for.", this);
		}
		return new UTComponentFilter (EvaluateAll (components, context));
	}
	
	private UTFilter GetByExpressionFilter (UTContext context) {
		if (expressions.Length == 0) {
			throw new UTFailBuildException ("You need to specify at least one expression to search for.", this);
		}
		
		string gameObjectProperty = currentGameObjectProperty.EvaluateIn(context);
		if (string.IsNullOrEmpty(gameObjectProperty)) {
			throw new UTFailBuildException("You need to specify the name of the game object property.", this);
		}
		return new UTExpressionFilter (EvaluateAll (expressions, context), gameObjectProperty, context);
	}
	
	private UTFilter GetByPositionFilter(UTContext context) {
		var thePosition = position.EvaluateIn(context);
		var theDistance = distance.EvaluateIn(context);
		
		return new UTPositionFilter(thePosition, theDistance);
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Find Game Objects", false, 390)]
	public static void AddAction ()
	{
		var result = Create<UTFindGameObjectsAction> ();
		result.distance = new UTFloat();
		result.distance.Value = 0.1f;
	}
	
}
