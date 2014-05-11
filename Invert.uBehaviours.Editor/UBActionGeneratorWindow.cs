using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UBehaviours.Actions;
using UnityEditor;
using UnityEngine;

public class UBActionGeneratorWindow : EditorWindow
{
    public string _Code = string.Empty;
    public string _InputText = "UnityEngine.Transform";
    public Vector2 _ScrollPosition;

    private ClassGenerator _currentGenerator;

    private string _currentPath;

    private TypeActionsGenerator _generator;

    public static string SelectedAssetPath
    {
        get
        {
            string path = "Assets";

            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);

                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }

                break;
            }
            return path;
        }
    }

    public static string SelectedPath
    {
        get
        {
            return Path.Combine(Application.dataPath.Replace("Assets", ""), SelectedAssetPath).Replace("\\", "//").Replace("//", "/");
        }
    }

    //[MenuItem("Tools/uBehaviours/Generate All", false, 1)]
    //public static void GeneralAll()
    //{
    //    var type = new TypeActionsGenerator();
    //    type.AddType(typeof(Transform),false);
    //    type.AddType(typeof(Camera), false);
    //    type.AddType(typeof(Mathf), false);
    //    type.AddType(typeof(GameObject), false);
    //    type.AddType(typeof(Component), false);
    //    type.AddType(typeof(Network), false);
    //    type.AddType(typeof(Convert), false);
    //    type.AddType(typeof(Rigidbody), false);
    //    type.AddType(typeof(Application), false);
    //    type.AddType(typeof(Component), false);
    //    type.AddType(typeof(Input), false);
    //    type.AddType(typeof(GUI), false);
    //    type.AddType(typeof(GUIText), false);
    //    type.AddType(typeof(MasterServer), false);
    //    type.AddType(typeof(Physics), false);
    //    type.AddType(typeof(Physics2D), false);
    //    type.AddType(typeof(UnityEngine.Random), false);

    //    type.AddType(typeof(UnityEngine.Object), false);
    //    type.AddType(typeof(UBMath), false);
    //    type.AddType(typeof(UBRandom), false);
    //    //type.AddType(typeof(PlayerPrefs));
    //    type.AddType(typeof(Resources), false);
    //    //type.AddType(typeof(iTween));
    //    type.AddType(typeof(Time), false);
    //    type.AddType(typeof(Vector3), false);
    //    type.AddType(typeof(Vector2), false);
    //    type.AddType(typeof(Rect), false);

    //    type.GenerateAll();
    //    AssetDatabase.Refresh();
    //}

    [MenuItem("Tools/uBehaviours/Action Generator", false, 1)]
    public static void Init()
    {
        Init(null);
    }

    [MenuItem("Tools/uBehaviours/Action Generator", false, 1)]
    public static void Init(MemberInfo memberInfo)
    {
        var window = (UBActionGeneratorWindow)GetWindow(typeof(UBActionGeneratorWindow));
        window.title = "UBAction Generator";

        if (memberInfo != null)
        {
            window._InputText = memberInfo.DeclaringType.Name;
            window._generator = new TypeActionsGenerator();

            if (memberInfo is PropertyInfo)
            {
                //window._generator.Generators.OfType<Propertyac>()
                window._generator.AddProperty(memberInfo as PropertyInfo).ToArray();
            }
            else if (memberInfo is MethodInfo)
            {
                window._generator.AddMethod(memberInfo as MethodInfo).ToArray();
            }
            window._currentGenerator = window._generator.Generators.FirstOrDefault();
            if (window._currentGenerator != null)
                window._Code = window._currentGenerator.ToString();
        }
        window.Show();
    }

    public Type FindType(string name)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var t = assembly.GetType(name);
            if (t != null)
            {
                return t;
            }
        }
        return null;
    }

    public IEnumerable<MethodInfo> GetMembers(Type type, BindingFlags flags = BindingFlags.Default)
    {
        // var type = typeof(GameObject);

        var declaredMembers = type.GetMethods().ToArray();

        foreach (var dm in declaredMembers)
        {
            var parameters = dm.GetParameters();
            if (dm.Name.StartsWith("get_") || dm.Name.StartsWith("set_")) continue;
            if (dm.ReturnType != typeof(void) && !UBGeneratorHelper.AllowedParameterTypes.Contains(dm.ReturnType)) continue;
            if (!parameters.All(p => UBGeneratorHelper.AllowedParameterTypes.Contains(p.ParameterType))) continue;
            //if (dm.get)
            yield return dm;
        }
    }

    public IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags flags = BindingFlags.Default)
    {
        // var type = typeof(GameObject);

        var declaredMembers = type.GetProperties(flags).ToArray();

        foreach (var dm in declaredMembers)
        {
            if (!UBGeneratorHelper.AllowedParameterTypes.Contains(dm.PropertyType)) continue;
            //if (dm.get)
            yield return dm;
        }
    }

    public void OnGUI()
    {
        var newText = GUILayout.TextField(_InputText ?? "");

        if (newText != _InputText || _generator == null)
        {
            _InputText = newText;

            if (string.IsNullOrEmpty(_InputText)) return;

            var t = FindType(_InputText);
            if (t == null) return;
            _generator = new TypeActionsGenerator(t);
        }

        if (_generator == null) return;

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width / 3f));
        GUILayout.Box("Files To Generate", UBStyles.CommandBarOpenStyle);
        _ScrollPosition = EditorGUILayout.BeginScrollView(_ScrollPosition);
        foreach (var generator in _generator.Generators)
        {
            if (GUILayout.Button(generator.Filename, UBStyles.ButtonStyle))
            {
                _currentGenerator = generator;
                _Code = generator.ToString();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width - (Screen.width / 3f)));
        GUILayout.Box("Generated Code", UBStyles.CommandBarOpenStyle);
        GUILayout.TextArea(_Code, GUILayout.Height(Screen.height - 110));

        // Generate buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate All"))
        {
            _generator.GenerateAll();
            AssetDatabase.Refresh();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate File"))
        {
            _currentGenerator.ToFile();
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    //public void CompileSnippet()
    //{
    //    var snippet = "DateTime.Now + TimeSpan.FromDays(30 * 365)";
    //    var template = "using System;\npublic class Snippet {{\n\tpublic static void main(string[] args) {{\n\t\tConsole.WriteLine({0});\n\t}}\n}}";

    //    var code = string.Format(template, snippet);

    //    var provider = new Microsoft.CSharp.CSharpCodeProvider();

    //    var parameters = new System.CodeDom.Compiler.CompilerParameters { GenerateExecutable = false, GenerateInMemory = true };

    //    var results = provider.CompileAssemblyFromSource(parameters, code);
    //    if (results.Errors.Count > 0)
    //    {
    //        foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
    //        {
    //            Console.Error.WriteLine("Line number {0}, Error Number: {1}, '{2};\r\n\r\n", error.Line, error.ErrorNumber, error.ErrorText);
    //        }
    //    }
    //    else
    //    {
    //        var type = results.CompiledAssembly.GetType("Snippet");
    //        var method = type.GetMethod("main");
    //        method.Invoke(null, new object[] { new string[0] });
    //    }
    //}

}