using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UBehaviours.Actions;
using UnityEditor;
using UnityEngine;

public class UBActionsWindow : EditorWindow
{
    public string _AssetPath;
    public UBActionSheet _Context;
    public int _SelectedIndex;
    private static IEnumerable<IGrouping<string, Type>> _actionGroups;
    private bool _allowInstanceOnly;
    private MemberInfo[] _members;
    private Action<string, string> _onAdd;
    private Vector2 _scrollPosition;
    private string _SearchText = "";

    public string SearchText
    {
        get { return EditorPrefs.GetString("UBActionsSearchText", string.Empty); }
        set { EditorPrefs.SetString("UBActionsSearchText", value); }
    }

    public static void Init(Action<string, string> onAdd, bool allowInstanceOnly = false)
    {
        // Get existing open window or if none, make a new one:
        var window = (UBActionsWindow)GetWindow(typeof(UBActionsWindow));
        _actionGroups = ActionSheetHelpers.GetDerivedTypes<UBAction>(false, false).GroupBy(p => OnKeySelector(p)).OrderBy(p => p.Key).ToArray();
        window.title = "uBehaviour Actions";
        window._allowInstanceOnly = allowInstanceOnly;
        window._onAdd = onAdd;
        //window.Show();
        GUI.FocusControl("SearchText");
    }

    public string GetMemberName(MemberInfo member)
    {
        if (member is MethodInfo)
        {
            var methodInfo = member as MethodInfo;
            var pNames = methodInfo.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name).ToArray();
            return string.Format("{0}({1})", methodInfo.Name, string.Join(", ", pNames));
        }
        else
        {
            return member.Name;
        }
    }

    public void OnGUI()
    {
        GUI.SetNextControlName("SearchText");
        var newText = GUILayout.TextField(SearchText ?? "");
        if (newText != SearchText || _members == null)
        {
            _members = UBUtils.TypeMemberSearch(newText).Take(100).ToArray();
            SearchText = newText;
        }
        var upperSearchText = SearchText.ToUpper();
        if (_actionGroups == null) return;
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        foreach (var group in _actionGroups)
        {
            var filteredItems = group.Where(p => p.Name.ToUpper().Contains(upperSearchText)).OrderBy(p => p.Name).ToArray();
            if (filteredItems.Length < 1) continue;

            GUILayout.Box(group.Key, UBStyles.CommandBarOpenStyle);

            foreach (var item in filteredItems)
            {
                if (!_allowInstanceOnly && item.GetCustomAttributes(typeof(InstanceOnlyAttribute), true).Length > 0)
                    continue;

                var help = item.GetCustomAttributes(typeof(UBHelpAttribute), true).FirstOrDefault() as UBHelpAttribute;
                var helpText = help == null ? string.Empty : help.Text;
                //var rect = GUILayoutUtility.GetRect(GUIContent.none, UBStyles.CommandBarClosedStyle, GUILayout.Height(50));

                //GUI.Box(rect, string.Empty, UBStyles.CommandBarClosedStyle);
                //var labelRect = new Rect(rect);
                //labelRect.x += 10;
                //labelRect.y += 10;
                //var style = new GUIStyle(EditorStyles.largeLabel);
                //style.fontStyle = FontStyle.Bold;
                //style.fontSize = 14;
                //GUI.Label(labelRect, item.Name, style);
                //if (help != null)
                //{
                //    labelRect.y += 20;
                //    style.fontSize = 10;
                //    style.fontStyle = FontStyle.Normal;
                //    style.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                //    GUI.Label(labelRect, help.Text, style);
                //}

                if (GUILayout.Button(new GUIContent(item.Name, helpText), EditorStyles.toolbarButton))
                {
                    _onAdd(item.Name, item.AssemblyQualifiedName);
                    Close();
                }
            }
        }
        GUILayout.Box("Generatable Actions:", UBStyles.CommandBarOpenStyle);
        if (!string.IsNullOrEmpty(SearchText))
            foreach (var member in _members.OrderBy(p => p.DeclaringType.Name + p.Name))
            {
                if (member.DeclaringType == null) continue;
                if (GUILayout.Button(member.DeclaringType.Name + "." + GetMemberName(member), EditorStyles.toolbarButton))
                {
                    UBActionGeneratorWindow.Init(member);
                    Close();
                }
            }
        EditorGUILayout.EndScrollView();
        if (GUI.GetNameOfFocusedControl() == string.Empty)
        {
            GUI.FocusControl("SearchText");
        }
    }

    private static string OnKeySelector(Type p)
    {
        var customAttribute = p.GetCustomAttributes(typeof(UBCategoryAttribute), true);

        if (customAttribute.Length > 0)
        {
            return ((UBCategoryAttribute)customAttribute[0]).Name;
        }
        return "Misc";
    }
}