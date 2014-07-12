//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Reflection;
using System.Collections.Generic;

public class UTRequiresLicenseAttribute : System.Attribute 
{
	public UTLicense license;
	
	public UTRequiresLicenseAttribute (UTLicense license)
	{
		this.license = license;
	}
	
	public static UTRequiresLicenseAttribute Get(Type type) {
		return GetAttribute<UTRequiresLicenseAttribute>(type.GetCustomAttributes);
	}

	public static UTRequiresLicenseAttribute Get(FieldInfo field) {
		return GetAttribute<UTRequiresLicenseAttribute>(field.GetCustomAttributes);
	}

	public static bool HasRequiredLicense (Type type, out string message)
	{
		return HasRequiredLicense (type.GetCustomAttributes, out message);
	}
	
	public static bool HasRequiredLicense (FieldInfo field, out string message)
	{
		return HasRequiredLicense (field.GetCustomAttributes, out message);
		
	}
	
	public delegate object[] AttributeLookup (Type type,bool inherit);
	
	private static bool HasRequiredLicense (AttributeLookup attributeLookup, out string message)
	{
		var requiredComponents = new List<String> ();
		var licenseAttribute = GetAttribute<UTRequiresLicenseAttribute> (attributeLookup);
		if (licenseAttribute != null) {
			var theLicense = licenseAttribute.license;
			if ((theLicense & UTLicense.UnityPro) != 0 && !UTils.IsUnityPro) {
				requiredComponents.Add ("Unity Pro");	
			}
			if ((theLicense & UTLicense.IosPro) != 0 && !UTils.HasAdvancedLicenseOn (UnityEditor.BuildTarget.iPhone)) {
				requiredComponents.Add ("iOS Pro");
			}
			if ((theLicense & UTLicense.AndroidPro) != 0 && !UTils.HasAdvancedLicenseOn (UnityEditor.BuildTarget.Android)) {
				requiredComponents.Add ("Android Pro");
			}
		} else {
			// legacy attributes support, this will be removed later.
#pragma warning disable 618
			var unityProAttribute = GetAttribute<UTRequiresUnityPro> (attributeLookup);
			if (unityProAttribute != null && !UTils.IsUnityPro) {
				requiredComponents.Add ("Unity Pro");	
			}
			
			var unityIosAttribute = GetAttribute<UTRequiresiOS> (attributeLookup);
			if (unityIosAttribute != null && unityIosAttribute.iOSPro && !UTils.HasAdvancedLicenseOn (UnityEditor.BuildTarget.iPhone)) {
				requiredComponents.Add ("iOS Pro");
			}

			
			var unityAndroidAttribute = GetAttribute<UTRequiresAndroid> (attributeLookup);
			if (unityAndroidAttribute != null && unityAndroidAttribute.androidPro && !UTils.HasAdvancedLicenseOn (UnityEditor.BuildTarget.Android)) {
				requiredComponents.Add ("Android Pro");
			}
#pragma warning restore 618
		}
		
		if (requiredComponents.Count > 0) {
			message = String.Join (", ", requiredComponents.ToArray ());
			return false;
		}
		message = "";
		return true;
	}

	private static T GetAttribute<T> (AttributeLookup attributeLookup)
	{
		var attributes = attributeLookup (typeof(T), true);
		if (attributes.Length == 1) {
			return (T)attributes [0];
		}
		return default(T);
	}
	
}

