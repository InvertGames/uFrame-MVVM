//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Reflection;
using UnityEditor;

/// <summary>
/// Documentation annotation that can be used to document the properties of actions.
/// </summary>
[AttributeUsageAttribute(AttributeTargets.Class|AttributeTargets.Field)]
public class UTDoc : System.Attribute
{
	/// <summary>
	/// The title of the the property. If not specified, the title will be derived from the field name.
	/// </summary>
	public string title;
	/// <summary>
	/// The description of the property. This will be displayed as tooltip in the inspector.
	/// </summary>
	public string description;
	/// <summary>
	/// A help URL. If specified, a help icon will be displayed in the inspector. When the user clicks the
	/// help icon, the url will be opened in the default browser.
	/// </summary>
	public string helpUrl;
	
	public UTDoc () {
	}
	
	
	private UTDoc(string title, string description) : this(title, description, null) {
	}
	
	private UTDoc(string title, string description, string helpUrl) {
		this.title = title;
		this.description = description;
		this.helpUrl = helpUrl;
	}
	
	/// <summary>
	/// Gets the UTDoc attached to the given field.
	/// </summary>
	/// <returns>
	/// The UTDoc or a new UTDoc using a  default title and description, if there is no UTDoc on the field.
	/// </returns>
	public static UTDoc GetFor(FieldInfo field) {
		var docs = field.GetCustomAttributes(typeof(UTDoc), false);		
		return CheckAndFix(docs, ObjectNames.NicifyVariableName(field.Name));
	}
	
	
	/// <summary>
	/// Gets the UTDoc attached to the given type.
	/// </summary>
	/// <returns>
	/// The UTDoc or a new UTDoc using a  default title and description, if there is no UTDoc on the type.
	/// </returns>
	public static UTDoc GetFor(Type type) {
		var docs = type.GetCustomAttributes(typeof(UTDoc), false);	
		return CheckAndFix(docs, ObjectNames.NicifyVariableName(type.Name));
	}
	

	private static UTDoc CheckAndFix(object[] docs, string defaultTitle) {
		if (docs.Length == 1) {
			var doc = (UTDoc) docs[0];
			if (String.IsNullOrEmpty(doc.title)) {
				doc.title = defaultTitle;
			}
			return doc;
		}
		return new UTDoc(defaultTitle ,"");
	}

}

