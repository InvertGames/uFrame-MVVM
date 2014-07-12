//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//
using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

/// <summary>
///  Helper class for calling internal/invisible Unity API via reflection.
/// </summary>
public class UTInternalCall
{
	
	/// <summary>
	/// Unity's script assemblies. Used for calling into scripts when they are not visible (for whatever reasons).
	/// </summary>
	private static string[] ScriptAssemblies = new string[] {
		"Assembly-UnityScript-Editor",
		"Assembly-UnityScript-firstpass",
		"Assembly-UnityScript",
		"Assembly-CSharp-Editor",
		"Assembly-CSharp"
	};
	
	public static object Get(object obj, string propertyName) {
		Type type = obj.GetType();

		var prop = type.GetProperty (propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
		if (prop == null) {
			Debug.LogWarning ("Internal property " + propertyName + " does not exist.", null);
			return null;
		}
		if (!prop.CanRead) {
			Debug.LogWarning ("Internal property " + propertyName + " has no read operation.", null);
			return null;
		}
		return prop.GetValue (obj, null);
	}

	public static object GetStatic (string typeName, string propertyName)
	{
		Type type = GetType (typeName);
		if (type == null) {
			Debug.LogWarning ("No such type " + type, null);
			return  null;
		}
		
		var prop = type.GetProperty (propertyName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		if (prop == null) {
			Debug.LogWarning ("Internal property " + propertyName + " does not exist.", null);
			return null;
		}
		if (!prop.CanRead) {
			Debug.LogWarning ("Internal property " + propertyName + " has no read operation.", null);
			return null;
		}
		return prop.GetValue (null, null);
	}
	

	public static void Set(object obj, string propertyName, object value) {
		Type type = obj.GetType();

		var prop = type.GetProperty (propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
		if (prop == null) {
			Debug.LogWarning ("Internal property " + propertyName + " does not exist.", null);
			return ;
		}
		if (!prop.CanWrite) {
			Debug.LogWarning ("Internal property " + propertyName + " has no write operation.", null);
			return ;
		}
		prop.SetValue (obj, value, null);
	}

	/// <summary>
	/// Invokes a static internal method by reflection.
	/// </summary>
	/// <returns>
	/// The result of invoking the method.
	/// </returns>
	/// <param name='typeName'>
	/// Type name on which the method should be invoked.
	/// </param>
	/// <param name='methodName'>
	/// Method name to invoke.
	/// </param>
	/// <param name='parameters'>
	/// Parameters for the method.
	/// </param>
	/// <exception cref='UTFailBuildException'>
	/// Is thrown if the method cannot be successfully invoked.
	/// </exception>
	public static object InvokeStatic (string typeName, string methodName, params object[] parameters)
	{
		Type type = GetType (typeName);
		
		if (type == null) {
			Debug.LogWarning ("No such type " + type, null);
			return null;
		}
		var method = type.GetMethod (methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		if (method == null) {
			Debug.LogWarning ("Internal method: " + typeName + "." + methodName + " does not exist.", null);
			return null;
		}
		return method.Invoke (null, parameters);	
	}
	

	public static object Invoke (object obj, string methodName, params object[] parameters)
	{
		Type type = obj.GetType();

		var method = type.GetMethod (methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
		if (method == null) {
			Debug.LogWarning ("Internal method: " + type.Name + "." + methodName + " does not exist.", null);
			return null;
		}
		return method.Invoke (obj, parameters);
	}

	/// <summary>
	/// Creates an instance of an object by reflection.
	/// </summary>
	/// <returns>
	/// The instance.
	/// </returns>
	/// <param name='typeName'>
	/// Type name of the class to instanciate.
	/// </param>
	/// <param name='parameters'>
	/// Parameters to the constructor.
	/// </param>
	public static object CreateInstance(string typeName, params object[]parameters)
	{
		Type type = GetType(typeName);
		if (type == null) {
			Debug.LogWarning("No such type " + type, null);
			return null;
		}
		return Activator.CreateInstance(type, parameters);
	}

	public static object GetStaticField (string typeName, string fieldName)
	{
		Type type = GetType (typeName);

		if (type == null) {
			Debug.LogWarning ("No such type " + type, null);
			return null;
		}

		var fld = type.GetField (fieldName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		return fld.GetValue (null);
	}

	public static object GetField (object obj, string fieldName)
	{
		Type type = obj.GetType ();
		var fld = type.GetField (fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		if (fld == null) {
			Debug.LogWarning("No such field: " + fieldName);
			return null;
		}
		return fld.GetValue (obj);
	}

	public static void SetField (object obj, string fieldName,object value)
	{
		Type type = obj.GetType ();
		var fld = type.GetField (fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		if (fld == null) {
			Debug.LogWarning("No such field: " + fieldName);
			return;
		}
		fld.SetValue (obj, value);
	}

	public static object EnumValue(string typeName, string enumConstant) {
		Type type = GetType(typeName);
		if (type == null) {
			Debug.LogWarning("No such type " + typeName , null);
			return null;
		}
		return Enum.Parse(type, enumConstant);
	}

	public static void AddDelegateStatic (string typeName, string eventName, Delegate delegateImplementation)
	{
		Type type = GetType (typeName);

		if (type == null) {
			Debug.LogWarning ("No such type " + type, null);
		}

		var dlg = type.GetField (eventName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		var theDelegate = (Delegate)dlg.GetValue (null);
		var combinedDelegate = Delegate.Combine (theDelegate, delegateImplementation);
		dlg.SetValue (null, combinedDelegate);
		// var evt = type.GetEvent(eventName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		// evt.GetAddMethod().Invoke(null, new object[]{delegateImplementation});
	}

	public static void RemoveDelegateStatic (string typeName, string eventName, Delegate delegateImplementation)
	{
		Type type = GetType (typeName);

		if (type == null) {
			Debug.LogWarning ("No such type " + type, null);
		}

//		var evt = type.GetEvent(eventName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		// evt.GetRemoveMethod().Invoke(null, new object[]{delegateImplementation});
		var dlg = type.GetField (eventName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
		var theDelegate = (Delegate)dlg.GetValue (null);
		var combinedDelegate = Delegate.Remove (theDelegate, delegateImplementation);
		dlg.SetValue (null, combinedDelegate);
	}

	/// <summary>
	/// Tries to load an internal type. Based on code from
	/// http://answers.unity3d.com/questions/206665/typegettypestring-does-not-work-in-unity.html
	/// </summary>
	/// <returns>
	/// The type or null if no such type could be found.
	/// </returns>
	/// <param name='typeName'>
	/// Type name.
	/// </param>
	public static Type GetType (string typeName)
	{

		// Try Type.GetType() first. This will work with types defined
		// by the Mono runtime, in the same assembly as the caller, etc.
		var type = Type.GetType (typeName);

		// If it worked, then we're done here
		if (type != null) {
			return type;
		}
		

		// If we still haven't found the proper type, we can enumerate all of the 
		// loaded assemblies and see if any of them define the type
		var currentAssembly = Assembly.GetExecutingAssembly ();
		var referencedAssemblies = currentAssembly.GetReferencedAssemblies ();
		foreach (var assemblyName in referencedAssemblies) {
			// Load the referenced assembly
			var assembly = Assembly.Load (assemblyName);
			if (assembly != null) {
				// See if that assembly defines the named type
				type = assembly.GetType (typeName);
				if (type != null) {
					return type;
				}
			}
		}
		
		// try the script assemblies
		foreach (var scriptAssembly in ScriptAssemblies) {
			try {
				var theScriptAssembly = Assembly.Load (scriptAssembly);
				if (theScriptAssembly != null) {
					type = theScriptAssembly.GetType (typeName);
					if (type != null) {
						return type;
					}
				} 
			} catch (Exception) {
				// ignore
			}
		}
		
		
		// If the TypeName is a full name, then we can try loading the defining assembly directly
		if (typeName.Contains (".")) {

			// Get the name of the assembly (Assumption is that we are using 
			// fully-qualified type names)
			var assemblyName = typeName.Substring (0, typeName.IndexOf ('.'));

			// Attempt to load the indicated Assembly
			Assembly assembly;
			try {
				assembly = Assembly.Load (assemblyName);
				if (assembly == null) {
					return null;
				}
			} catch (FileNotFoundException) {
				return null;
			}
			// Ask that assembly to return the proper Type
			type = assembly.GetType (typeName);
			if (type != null) {
				return type;
			}

		}

		
		// The type just couldn't be found...
		return null;

	}
	
	/// <summary>
	/// Returns a list with all public members of a given type (properties and fields).
	/// </summary>
	public static MemberInfo[] PublicMembersOf (Type type)
	{
		var props = type.GetProperties (BindingFlags.Public | BindingFlags.Instance);
		var fields = type.GetFields (BindingFlags.Public | BindingFlags.Instance);
		var resultList = new List<MemberInfo> ();
		resultList.AddRange (Array.FindAll(props, item => item.GetIndexParameters().Length == 0 ));
		resultList.AddRange (fields);
		return resultList.ToArray ();
	}
	
	/// <summary>
	/// Determines whether the member specified by the given member info can be read by reflection.
	/// </summary> 
	public static bool IsReadable (MemberInfo info)
	{
		if (info is FieldInfo) {
			return true;
		}
		
		if (info is PropertyInfo) {
			return ((PropertyInfo)info).CanRead;
		}
		
		return false;
	}

	/// <summary>
	/// Determines whether the member specified by the given member info can be written by reflection.
	/// </summary> 
	public static bool IsWritable (MemberInfo info)
	{
		if (info is FieldInfo) {
			return true;
		}
		
		if (info is PropertyInfo) {
			return ((PropertyInfo)info).CanWrite;
		}
		
		return false;
	}
	
	/// <summary>
	/// Determines whether the type given has sub properties.
	/// </summary>
	public static bool HasMembers (Type type)
	{
		return !type.IsPrimitive && !type.IsEnum && !type.IsArray && type != typeof(string);
	}
	
	/// <summary>
	/// Gets the data type of the member.
	/// </summary>
	public static Type GetMemberType (MemberInfo member)
	{
		switch (member.MemberType) {
		case MemberTypes.Event:
			return ((EventInfo)member).EventHandlerType;
		case MemberTypes.Field:
			return ((FieldInfo)member).FieldType;
		case MemberTypes.Property:
			return ((PropertyInfo)member).PropertyType;
		default:
			throw new ArgumentException ("Unsupported type of member " + member.MemberType);
		}
	}
	
	public static void SetMemberValue(object root, MemberInfo[] path, object value) {
		// FieldInfo contains GetValueDirect and SetValueDirect which returns a reference to the value even vor structs.
		// But PropertyInfo doesn't contain such methods. Therefore we have to read the values and write them back later.
		// This is necessary for struct types.
		
		object[] stack = new object[path.Length + 1];
		stack[0] = root;
		
		// read chained property values as long as it is not the last property
		for(int i = 0; i < path.Length - 1; i++) {
			stack[i + 1] = GetMemberValue(stack[i], path[i]);	
		}
		
		// push the new value to the stack
		stack[path.Length] = value;
		
		// write the values from the stack back in the chained properties
		for(int i = path.Length; i > 0; i--) {
			SetMemberValue(stack[i - 1], path[i - 1], stack[i]);
		}
	}
	
	
	public static object GetMemberValue(object source, MemberInfo member) {
		switch (member.MemberType) {
		case MemberTypes.Field:
			return ((FieldInfo)member).GetValue(source);
		case MemberTypes.Property:
			return ((PropertyInfo)member).GetValue(source, null);
		default:
			throw new ArgumentException ("Unsupported type of member " + member.MemberType);
		}
	}

	public static void SetMemberValue(object target, MemberInfo member, object value) {
		switch (member.MemberType) {
		case MemberTypes.Field:
			((FieldInfo)member).SetValue(target, value);
			return;
		case MemberTypes.Property:
			((PropertyInfo)member).SetValue(target, value, null);
			return;
		default:
			throw new ArgumentException ("Unsupported type of member " + member.MemberType);
		}
	}

}
