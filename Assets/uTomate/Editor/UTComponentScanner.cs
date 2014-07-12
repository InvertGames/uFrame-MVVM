//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Reflection scanning class which caches it's result.
/// 
/// TODO: Excessive caching might produce memory leaks. We need to find a way to clean caches from time to time.
/// </summary>
public class UTComponentScanner
{
	/// <summary>
	/// Finds the components annotated with the given annotations. Components are instanciated and returned as keys
	/// of the result dictionary. The values of the result dictionary contain the respective annotations which identified
	/// the components.
	/// </summary>
	/// <returns>
	/// The components annotated with the given annotation type.
	/// </returns>
	/// <typeparam name='ExpectedType'>
	/// The expected type of the components.
	/// </typeparam>
	/// <typeparam name='AnnotationType'>
	/// The annotation identifying the components.
	/// </typeparam>
	public static Dictionary<ExpectedType, AnnotationType> FindComponentsAnnotatedWith<ExpectedType, AnnotationType> ()
	{
		Dictionary<ExpectedType,AnnotationType> result = new Dictionary<ExpectedType, AnnotationType> ();
		var referencedAssemblies = (Assembly[])UTInternalCall.GetStatic ("UnityEditor.EditorAssemblies", "loadedAssemblies");
		foreach (var referencedAssembly in referencedAssemblies) {
			var allTypes = referencedAssembly.GetTypes ();
			foreach (var theType in allTypes) {
				var attrs = theType.GetCustomAttributes (typeof(AnnotationType), false);	
				if (attrs.Length == 1) {
					if (!theType.IsSubclassOf (typeof(ExpectedType)) && theType.GetInterface (typeof(ExpectedType).Name) == null) {
						Debug.LogWarning ("Class " + theType.FullName + " is not a subclass/implementation of " + typeof(ExpectedType).FullName + ". Ignoring it.");
						continue;
					}
					ExpectedType theInstance = (ExpectedType)Activator.CreateInstance (theType);
					result.Add (theInstance, (AnnotationType)attrs [0]);
				}
			}
		}
		return result;
	}
	
	private static Dictionary<Type, UTCompatibleTypesResult> compatibleTypeCache = new Dictionary<Type, UTCompatibleTypesResult> ();
	
	public static UTCompatibleTypesResult FindCompatibleTypes (Type baseType)
	{
		if (compatibleTypeCache.ContainsKey (baseType)) {
			return compatibleTypeCache [baseType];
		}
		
		List<Type> types = new List<Type> ();
		var referencedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies ();

		foreach (var referencedAssembly in referencedAssemblies) {
			var allTypes = referencedAssembly.GetTypes ();
			foreach (var theType in allTypes) {
				if (baseType.IsAssignableFrom (theType)) {
					types.Add (theType);	
				}
			}
		}
		
		var result = new UTCompatibleTypesResult (types);
		compatibleTypeCache.Add (baseType, result);
		return result;		
	}
	
	private static Dictionary<Type, UTMemberListResult> memberListCache = new Dictionary<Type, UTMemberListResult> ();
	
	public static UTMemberListResult FindPublicWritableMembersOf (Type type)
	{
		if (memberListCache.ContainsKey (type)) {
			return memberListCache [type];
		}
		
		var allMembers = UTInternalCall.PublicMembersOf (type);
		UTMemberListResult result = new UTMemberListResult (Array.FindAll (allMembers, item => UTInternalCall.IsWritable (item)));
		memberListCache.Add (type, result);
		return result;
	}
	
	private static Dictionary<string, MemberInfo[]> propertyPathCache = new Dictionary<string, MemberInfo[]> ();
	
	/// <summary>
	/// Finds a list of memberinfo objects that form a property path. Returns null if no matching path could be found.
	/// </summary>
	public static MemberInfo[] FindPropertyPath (Type startingPoint, string propertyPath)
	{
		string key = startingPoint.FullName + ":" + propertyPath;
		if (propertyPathCache.ContainsKey (key)) {
			return propertyPathCache [key];
		}
		
		string[] propertyNames = propertyPath.Split('.');
		var referenceType = startingPoint;
		MemberInfo[] result = new MemberInfo[propertyNames.Length];
		int index = 0;
		foreach(var propertyName in propertyNames) {
			var matches = referenceType.GetMember(propertyName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance);
			if (matches.Length == 1) {
				result[index] = matches[0];
				referenceType = UTInternalCall.GetMemberType(matches[0]);
			}
			else {
				result = null;
				break;
			}
			index++;
		}
		
		propertyPathCache.Add (key, result);
		return result;
	}
}

