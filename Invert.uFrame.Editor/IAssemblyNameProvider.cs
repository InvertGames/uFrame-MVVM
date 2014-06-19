

namespace Invert.uFrame.Editor
{
    public interface IAssemblyNameProvider
    {
        string AssemblyName { get; }
    }
}

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Mail;
//using System.Reflection;
//using System.Text;
//using System.Text.RegularExpressions;
//using UnityEditor;
//using UnityEngine;
//using Object = UnityEngine.Object;

//public static class uFrameUtility
//{
//    private enum Case
//    {
//        PascalCase,
//        CamelCase
//    }

//    public static GameManager GCManager
//    {
//        get
//        {
//            return (GameManager)Object.FindObjectOfType(typeof(GameManager));
//        }
//    }

//    [MenuItem("Tools/[u]Frame/Create Manager", false, -10)]
//    public static void CreateManager()
//    {
//        new GameObject("_GameManager", typeof(GameManager));
//    }

//    [MenuItem("Tools/[u]Frame/Documentation", false, -5)]
//    public static void Documentation()
//    {
//        Application.OpenURL("http://invertgamestudios.com/uFrame/html/");
//    }

//    public static IEnumerable<Type> FilterToMethodsReturning(IEnumerable<Type> types, Type type)
//    {
//        foreach (var t in types)
//        {
//            foreach (var methodinfo in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
//            {
//                if (methodinfo.ReturnType == type && methodinfo.GetParameters().Length == 0)
//                {
//                    yield return t;
//                }
//            }
//        }
//    }

//    public static string FixAssetPath(this string s)
//    {
//        return s.Replace("\\", "/");
//    }

//    public static string GenerateFromTemplate(string templateName, Dictionary<string, string> vars)
//    {
//        var templateText = ((TextAsset)Resources.Load(templateName)).text;
//        return Regex.Replace(templateText, @"`(?<Word>\w+)`", (m) =>
//        {
//            var variable = m.Groups[1].Value;

//            return vars.ContainsKey(variable) ? vars[variable] : "REPLACE_ME";
//        }, RegexOptions.None);
//    }

//    public static void GenerateFromTemplateToFile(string templateName, string filename, Dictionary<string, string> vars)
//    {
//        var pathName = Path.GetDirectoryName(filename);

//        if (pathName != null && !Directory.Exists(pathName))
//        {
//            Directory.CreateDirectory(pathName);
//        }

//        var str = GenerateFromTemplate(templateName, vars);

//        if (!File.Exists(filename))
//            File.WriteAllText(filename, str);
//    }

//    public static void GenerateGenericTemplate(string templateResource, string name, string filename,
//        Dictionary<string, string> overrides = null)
//    {
//        var args = new Dictionary<string, string>()
//        {
//            { "Name", name},
//            { "SceneManager", name.Replace("SceneManager","") + "SceneManager"},
//            { "ControllerType", name.Replace("Controller","") + "Controller"},
//            { "ViewModelType",name.Replace("ViewModel","") + "ViewModel" },
//            { "ViewModelBaseType", "ViewModel" },
//            { "ViewType", name.Replace("View","") + "View" }
//        };
//        if (overrides != null)
//        {
//            foreach (var o in overrides)
//            {
//                if (args.ContainsKey(o.Key))
//                    args.Remove(o.Key);
//                args.Add(o.Key, o.Value);
//            }
//        }
//        var additional = new Dictionary<string, string>();

//        foreach (var item in args)
//        {
//            var camelkey = "Camel_" + item.Key;
//            var pascalkey = "Pascal_" + item.Key;
//            if (additional.ContainsKey(camelkey)) additional.Remove(camelkey);
//            if (additional.ContainsKey(pascalkey)) additional.Remove(pascalkey);

//            additional.Add(camelkey, ConvertCaseString(item.Value, Case.CamelCase));
//            additional.Add(pascalkey, ConvertCaseString(item.Value, Case.PascalCase));
//        }

//        foreach (var a in additional)
//        {
//            if (args.ContainsKey(a.Key))
//                args.Remove(a.Key);

//            args.Add(a.Key, a.Value);
//        }

//        GenerateFromTemplateToFile(templateResource, filename, args);
//    }

//    //public static IEnumerable<MethodInfo> GetAcceptableControllerMethods(Controller controller, Type modelType)
//    //{
//    //    var controllerType = controller.GetType();
//    //    var controllerMethods = controllerType.GetMethods();
//    //    foreach (var method in controllerMethods)
//    //    {
//    //        var firstParameter = method.GetParameters().FirstOrDefault();

//    //        if (firstParameter != null)
//    //            if (modelType != firstParameter.ParameterType && !(modelType.IsSubclassOf(firstParameter.ParameterType))) continue;
//    //        //if (method.Name == "Equals") continue;
//    //        if (method.ReturnType != typeof(void))
//    //        {
//    //            continue;
//    //        }
//    //        yield return method;
//    //    }
//    //}

//    public static IEnumerable<Type> GetDerivedTypes<T>(bool includeAbstract = false, bool includeBase = true)
//    {

//        var type = typeof(T);
//        if (includeBase)
//            yield return type;
//        if (includeAbstract)
//        {
//            foreach (var t in Assembly.GetAssembly(type)
//                .GetTypes()
//                .Where(x => x.IsSubclassOf(type)))
//            {
//                yield return t;
//            }
//        }
//        else
//        {
//            foreach (var t in Assembly.GetAssembly(type)
//                .GetTypes()
//                .Where(x => x.IsSubclassOf(type) && !x.IsAbstract))
//            {
//                yield return t;
//            }
//        }
//    }

//    public static Texture2D GetEditorUFrameResource(string name, int width, int height)
//    {
//#if !ASSEMBLY
//        var asset = AssetDatabase.LoadAssetAtPath(@"Assets/uFrameComplete/uFrame/Editor/Resources/" + name, typeof(Texture2D)) as Texture2D;
//        if (asset != null)
//        {
//            //asset._Width = _Width;
//            //asset.height = height;
//            return asset;
//        }
//        return null;
//#else
//        return LoadDllResource(resourceName, _Width, height);
//#endif
//    }

//    //public static IEnumerable<FieldInfo> GetModelProperties(this Type modelType)
//    //{
//    //    foreach (var fieldInfo in modelType.GetFields().Where(p => p.IsPublic && p.IsInitOnly))
//    //    {
//    //        if (typeof(ModelPropertyBase).IsAssignableFrom(fieldInfo.FieldType))
//    //        {
//    //            yield return fieldInfo;
//    //        }
//    //    }
//    //}

//    public static IEnumerable<MethodInfo> GetVMFactoryMethods(Type viewModelType, Type controllerType)
//    {
//        var methods = controllerType.GetMethods();
//        foreach (var method in methods)
//            if (viewModelType.IsAssignableFrom(method.ReturnType) && method.GetParameters().Length == 0)
//                yield return method;
//    }

//    public static void PopupField(string label, string value, Action<string> set, params string[] items)
//    {
//        var targetPropertyIndex = 0;
//        if (!string.IsNullOrEmpty(value))
//        {
//            var obj = items.FirstOrDefault(p => p == value);
//            if (obj != null)
//            {
//                targetPropertyIndex = Array.IndexOf(items, obj);
//            }
//        }
//        else
//        {
//            if (items != null && items.Length > 0)
//            {
//                set(items[0]);
//            }
//        }
//        var newTargetPropertyIndex = EditorGUILayout.Popup(label, targetPropertyIndex, items);
//        //if (newTargetPropertyIndex != targetPropertyIndex)
//        //{
//        if (items != null && items.Length > 0)
//        {
//            set(items[newTargetPropertyIndex]);
//        }

//        //}
//    }

//    public static void PropertiesField(SerializedObject obj, string arrayFieldName)
//    {
//        int size = obj.FindProperty(arrayFieldName + ".Array.size").intValue;

//        int newSize = EditorGUILayout.IntField(arrayFieldName + " Size", size);

//        if (newSize != size)
//            obj.FindProperty(arrayFieldName + ".Array.size").intValue = newSize;

//        EditorGUI.indentLevel = 3;

//        for (int i = 0; i < newSize; i++)
//        {
//            var prop = obj.FindProperty(string.Format("{0}.Array.data[{1}]", arrayFieldName, i));

//            EditorGUILayout.PropertyField(prop);
//        }
//    }

//    //    //}
//    //}
//    public static void SetGameObjectIcon(this GameObject obj, Texture2D icon)
//    {
//        var editorGUIUtilityType = typeof(EditorGUIUtility);
//        const BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
//        var args = new object[] { obj, icon };
//        editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
//        //var editorUtilityType = typeof(EditorUtility);
//        //editorUtilityType.InvokeMember("ForceReloadInspectors", bindingFlags, null, null, null);
//    }

//    //public static Texture2D LoadDllResource(string resourceName, int width, int height)
//    //{
//    //    // also lets you override dll resources locally for rapid iteration
//    //    Texture2D texture = (Texture2D)Resources.Load(resourceName);
//    //    if (texture != null)
//    //    {
//    //        Debug.Log("Loaded local resource: " + resourceName);
//    //        return texture;
//    //    }
//    //    // if unavailable, try assembly
//    //    Assembly myAssembly = Assembly.GetExecutingAssembly();
//    //    Stream myStream = myAssembly.GetManifestResourceStream("assemblypathhere" + resourceName + ".png");
//    //    texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
//    //    texture.LoadImage(ReadToEnd(myStream));
//    //    if (texture == null)
//    //    {
//    //        Debug.LogError("Missing Dll resource: " + resourceName);
//    //    }
//    //    return texture;
//    //}
//    //public static void PopupField(string label, SerializedProperty property, Action<SerializedProperty> set, params string[] items)
//    //{
//    //    var targetPropertyIndex = 0;
//    //    if (!string.IsNullOrEmpty(value))
//    //    {
//    //        var obj = items.FirstOrDefault(p => p == value);
//    //        if (obj != null)
//    //        {
//    //            targetPropertyIndex = Array.IndexOf(items, obj);
//    //        }
//    //    }
//    //    else
//    //    {
//    //        if (items != null && items.Length > 0)
//    //        {
//    //            set(items[0]);
//    //        }
//    //    }
//    //    var newTargetPropertyIndex = EditorGUILayout.Popup(label, targetPropertyIndex, items);
//    //    //if (newTargetPropertyIndex != targetPropertyIndex)
//    //    //{
//    //    if (items != null && items.Length > 0)
//    //    {
//    //        set(items[newTargetPropertyIndex]);
//    //    }
//    public static bool UnityHasCompileErrors()
//    {
//        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
//        Type logEntries = assembly.GetType("UnityEditorInternal.LogEntries");
//        logEntries.GetMethod("Clear").Invoke(new object(), null);

//        int count = (int)logEntries.GetMethod("GetCount").Invoke(new object(), null);

//        return count > 0;
//    }

//    /// <summary>
//    /// Converts the phrase to specified convention.
//    /// </summary>
//    /// <param name="phrase"></param>
//    /// <param name="cases">The cases.</param>
//    /// <returns>string</returns>
//    private static string ConvertCaseString(string phrase, Case cases)
//    {
        
//        string[] splittedPhrase = phrase.Split(' ', '-', '.');
//        var sb = new StringBuilder();

//        if (cases == Case.CamelCase)
//        {
//            sb.Append(splittedPhrase[0].ToLower());
//            splittedPhrase[0] = string.Empty;
//        }
//        else if (cases == Case.PascalCase)
//            sb = new StringBuilder();

//        foreach (String s in splittedPhrase)
//        {
//            char[] splittedPhraseChars = s.ToCharArray();
//            if (splittedPhraseChars.Length > 0)
//            {
//                splittedPhraseChars[0] = ((new String(splittedPhraseChars[0], 1)).ToUpper().ToCharArray())[0];
//            }
//            sb.Append(new String(splittedPhraseChars));
//        }
//        return sb.ToString();
//    }

//    // loads a png resources from the dll
//    private static byte[] ReadToEnd(Stream stream)
//    {
//        long originalPosition = stream.Position;
//        stream.Position = 0;
//        try
//        {
//            var readBuffer = new byte[4096];
//            int totalBytesRead = 0;
//            int bytesRead;
//            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
//            {
//                totalBytesRead += bytesRead;
//                if (totalBytesRead == readBuffer.Length)
//                {
//                    int nextByte = stream.ReadByte();
//                    if (nextByte != -1)
//                    {
//                        byte[] temp = new byte[readBuffer.Length * 2];
//                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
//                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
//                        readBuffer = temp;
//                        totalBytesRead++;
//                    }
//                }
//            }
//            byte[] buffer = readBuffer;
//            if (readBuffer.Length != totalBytesRead)
//            {
//                buffer = new byte[totalBytesRead];
//                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
//            }
//            return buffer;
//        }
//        finally
//        {
//            stream.Position = originalPosition;
//        }
//    }
//}
