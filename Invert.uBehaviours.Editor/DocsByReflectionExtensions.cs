using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;

/// <summary>
/// Utility class to provide documentation for various types where available with the assembly
/// </summary>
public static class DocsService
{
    /// <summary>
    /// Gets the summary from the &lt;summary&gt; tag in documentation comments of the given Type.
    /// </summary>
    public static string GetSummary(this System.Type type)
    {
        XmlElement xml = GetXmlFromType(type);
        var summary = xml["summary"];
        return summary != null ? summary.InnerText.Trim() : type.Name;
    }

    /// <summary>
    /// Gets the summary from the &lt;summary&gt; tag in documentation comments of the given Type.
    /// </summary>
    public static string GetSummary(this MethodInfo method)
    {
        XmlElement xml = GetXmlFromMember(method, false);
        if (xml == null) return null;
        var summary = xml["summary"];
        return summary != null ? summary.InnerText.Trim() : method.Name;
    }

    /// <summary>
    /// Gets the summary from the &lt;summary&gt; tag in documentation comments of the given Type.
    /// </summary>
    public static string GetSummary(this ParameterInfo parameterInfo)
    {
        XmlElement xml = GetXmlFromParameter(parameterInfo, false);
        if (xml == null) return null;
        return xml.InnerText.Trim();
    }

    /// <summary>
    /// Gets the summary from the &lt;summary&gt; tag in documentation comments of the given Type.
    /// </summary>
    public static string GetSummary(this PropertyInfo propertyInfo)
    {
        XmlElement xml = GetXmlFromMember(propertyInfo, false);
        if (xml == null) return null;
        var summary = xml["summary"];
        return summary != null ? summary.InnerText.Trim() : propertyInfo.Name;
    }

    ///// <summary>
    ///// Gets the summary from the &lt;summary&gt; tag in documentation comments of the given Type.
    ///// </summary>
    //public static string GetSummary(this System.Type type)
    //{
    //    XmlElement xml = GetXmlFromType(type);
    //    var summary = xml["summary"];
    //    return summary != null ? summary.InnerText.Trim() : type.Name;
    //}

    #region Fields

    /// <summary>
    /// A cache used to remember Xml documentation for assemblies
    /// </summary>
    private static Dictionary<Assembly, XmlDocument> s_cache = new Dictionary<Assembly, XmlDocument>();

    /// <summary>
    /// A cache used to store failure exceptions for assembly lookups
    /// </summary>
    private static Dictionary<Assembly, Exception> s_failCache = new Dictionary<Assembly, Exception>();

    #endregion Fields

    #region Public methods

    /// <summary>
    /// Obtains the documentation file for the specified assembly
    /// </summary>
    /// <param name="assembly">The assembly to find the XML document for</param>
    /// <param name="throwError">If should throw error when documentation is not found. Default is true.</param>
    /// <returns>The XML document</returns>
    /// <remarks>This version uses a cache to preserve the assemblies, so that
    /// the XML file is not loaded and parsed on every single lookup</remarks>
    [SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails"), SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
    public static XmlDocument GetXmlFromAssembly(Assembly assembly, bool throwError = true)
    {
        s_failCache.Clear();
        if (s_failCache.ContainsKey(assembly))
        {
            if (throwError)
            {
                throw s_failCache[assembly];
            }

            return null;
        }

        try
        {
            if (!s_cache.ContainsKey(assembly))
            {
                // load the docuemnt into the cache
                s_cache[assembly] = GetXmlFromAssemblyNonCached(assembly);
            }

            return s_cache[assembly];
        }
        catch (Exception exception)
        {
            s_failCache[assembly] = exception;

            if (throwError)
            {
                throw exception;
            }
        }

        return null;
    }

    /// <summary>
    /// Provides the documentation comments for a specific method
    /// </summary>
    /// <param name="method">The MethodInfo (reflection data ) of the member to find documentation for</param>
    /// <param name="throwError">If should throw error when documentation is not found. Default is true.</param>
    /// <returns>The XML fragment describing the method</returns>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
    public static XmlElement GetXmlFromMember(MethodBase method, bool throwError = true)
    {
        if (method == null)
        {
            throw new ArgumentNullException("method");
        }

        // Calculate the parameter string as this is in the member name in the XML
        string parametersString = "";

        foreach (ParameterInfo parameterInfo in method.GetParameters())
        {
            if (parametersString.Length > 0)
            {
                parametersString += ",";
            }

            parametersString += GetTypeFullNameForXmlDoc(parameterInfo.ParameterType, true);
        }

        //AL: 15.04.2008 ==> BUG-FIX remove “()” if parametersString is empty
        if (parametersString.Length > 0)
            return GetXmlFromName(method.DeclaringType, 'M', method.Name + "(" + parametersString + ")", throwError);
        else
            return GetXmlFromName(method.DeclaringType, 'M', method.Name, throwError);
    }

    /// <summary>
    /// Provides the documentation comments for a specific member.
    /// </summary>
    /// <param name="member">The MemberInfo (reflection data) or the member to find documentation for</param>
    /// <param name="throwError">If should throw error when documentation is not found. Default is true.</param>
    /// <returns>The XML fragment describing the member</returns>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
    public static XmlElement GetXmlFromMember(MemberInfo member, bool throwError = true)
    {
        if (member == null)
        {
            throw new ArgumentNullException("member");
        }

        // First character [0] of member type is prefix character in the name in the XML
        return GetXmlFromName(member.DeclaringType, member.MemberType.ToString()[0], member.Name, throwError);
    }

    /// <summary>
    /// Provides the documentation comments for a specific parameter.
    /// </summary>
    /// <param name="parameter">The ParameterInfo (reflection data) to find documentation for.</param>
    /// <param name="throwError">If should throw error when documentation is not found. Default is true.</param>
    /// <returns>The XML fragment describing the paramter.</returns>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
    public static XmlElement GetXmlFromParameter(ParameterInfo parameter, bool throwError = true)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException("parameter");
        }

        var method = parameter.Member as MethodBase;
        var memberDoc = method == null ? GetXmlFromMember(parameter.Member, throwError) : GetXmlFromMember(method, throwError);

        if (memberDoc == null)
        {
            return null;
        }

        return memberDoc.SelectSingleNode(String.Format(CultureInfo.InvariantCulture, "param[@name='{0}']", parameter.Name)) as XmlElement;
    }

    /// <summary>
    /// Provides the documentation comments for a specific type
    /// </summary>
    /// <param name="type">Type to find the documentation for</param>
    /// <param name="throwError">If should throw error when documentation is not found. Default is true.</param>
    /// <returns>The XML fragment that describes the type</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
    public static XmlElement GetXmlFromType(Type type, bool throwError = true)
    {
        // Prefix in type names is T
        return GetXmlFromName(type, 'T', "", throwError);
    }

    #endregion Public methods

    #region Private methods

    public static string GetAssemblyDocFileNameFromCodeBase(string assemblyCodeBase)
    {
        if (String.IsNullOrEmpty(assemblyCodeBase))
        {
            throw new ArgumentNullException("assemblyCodeBase");
        }

        var prefix = "file:///";

        if (assemblyCodeBase.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            var filePath = assemblyCodeBase.Substring(prefix.Length);

            if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
            {
                filePath = "/" + filePath;
            }

            //var file = Path.ChangeExtension(filePath, ".xml").Replace("/", "\\");
            //if (File.Exists(file))
            //{
            //    return file;
            //}
            return Application.dataPath.Replace("/", "\\").Replace("\\Assets", "\\Assets\\XmlDocs\\") + Path.ChangeExtension(Path.GetFileName(filePath), ".xml");
        }
        else
        {
            throw new Exception("Could not ascertain assembly filename from assembly code base '{0}'.");
        }
    }

    /// <summary>
    /// Gets the type's full name prepared for xml documentation format.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="isMethodParameter">If the type is being used has a method parameter.</param>
    /// <returns>The full name.</returns>
    private static string GetTypeFullNameForXmlDoc(Type type, bool isMethodParameter = false)
    {
        if (type.MemberType == MemberTypes.TypeInfo && type.IsGenericType && (!type.IsClass || isMethodParameter))
        {
            return String.Format(CultureInfo.InvariantCulture,
                "{0}{{{1}}}",
                string.IsNullOrEmpty(type.Namespace) ? "" : type.Namespace + "." +
                type.Name.Replace("`1", ""),
                GetTypeFullNameForXmlDoc(type.GetGenericArguments().FirstOrDefault()));
        }
        else if (type.IsNested)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}{1}.{2}", string.IsNullOrEmpty(type.Namespace) ? "" : type.Namespace + ".", type.DeclaringType.Name, type.Name);
        }
        else
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}{1}", string.IsNullOrEmpty(type.Namespace) ? "" : type.Namespace + ".", type.Name);
        }
    }

    /// <summary>
    /// Loads and parses the documentation file for the specified assembly
    /// </summary>
    /// <param name="assembly">The assembly to find the XML document for</param>
    /// <returns>The XML document</returns>
    private static XmlDocument GetXmlFromAssemblyNonCached(Assembly assembly)
    {
        string filePath = GetAssemblyDocFileNameFromCodeBase(assembly.CodeBase);

        try
        {
            using (var streamReader = new StreamReader(filePath.Replace("/", "\\")))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(streamReader);
                return xmlDocument;
            }
        }
        catch (DirectoryNotFoundException directoryException)
        {
            var msg = String.Format(CultureInfo.InvariantCulture, "Error trying to locate the XML documentation file on folder {0}.", filePath);
            throw new Exception(msg, directoryException);
        }
        catch (FileNotFoundException exception)
        {
            throw new Exception("XML documentation not present (make sure it is turned on in project properties when building)", exception);
        }
        catch (Exception ex)
        {
            throw new Exception("Error trying to get documentation filer for assembly code base '{0}'.", ex);
        }
    }

    /// <summary>
    /// Obtains the XML Element that describes a reflection element by searching the
    /// members for a member that has a name that describes the element.
    /// </summary>
    /// <param name="type">The type or parent type, used to fetch the assembly</param>
    /// <param name="prefix">The prefix as seen in the name attribute in the documentation XML</param>
    /// <param name="name">Where relevant, the full name qualifier for the element</param>
    /// <param name="throwError">If should throw error when documentation is not found. Default is true.</param>
    /// <returns>The member that has a name that describes the specified reflection element</returns>
    private static XmlElement GetXmlFromName(Type type, char prefix, string name, bool throwError)
    {
        string fullName = GetTypeFullNameForXmlDoc(type);

        if (String.IsNullOrEmpty(name))
        {
            fullName = String.Format(CultureInfo.InvariantCulture, "{0}:{1}", prefix, fullName);
        }
        else
        {
            fullName = String.Format(CultureInfo.InvariantCulture, "{0}:{1}.{2}", prefix, fullName, name);
        }
        fullName = fullName.ToUpper();
        XmlDocument xmlDocument = GetXmlFromAssembly(type.Assembly, throwError);
        XmlElement matchedElement = null;

        if (xmlDocument != null)
        {
            var members = xmlDocument["doc"]["members"].ChildNodes;
            foreach (XmlElement xmlElement in members.OfType<XmlElement>())
            {
                try
                {
                    var memberName = xmlElement.Attributes["name"].Value.ToString();

                    if (memberName.ToUpper() == fullName)
                    {
                        if (matchedElement != null)
                        {
                            throw new Exception("Multiple matches to query", null);
                        }

                        matchedElement = xmlElement;
                    }
                }
                catch (Exception ex)
                {
                    //UnityEngine.Debug.Log(ex);
                }
            }
        }

        if (matchedElement == null && throwError)
        {
            throw new Exception("Could not find documentation for specified element", null);
        }

        return matchedElement;
    }

    #endregion Private methods
}

internal static class DocsByReflectionExtensions
{
    ///// <summary>
    ///// Gets the summary from the &lt;summary&gt; tag in documentation comments of the given Type.
    ///// </summary>
    //public static string GetSummary(this System.Type type)
    //{
    //    XmlElement xml = DocsByReflection.XMLFromType(type);
    //    var summary = xml["summary"];
    //    return summary != null ? summary.InnerText.Trim() : type.Name;
    //}
}

//Except where stated all code and programs in this project are the copyright of Jim Blackler, 2008.
//jimblackler@gmail.com
//
//This is free software. Libraries and programs are distributed under the terms of the GNU Lesser
//General Public License. Please see the files COPYING and COPYING.LESSER.