using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Invert.Common;
using Invert.Common.UI;
using Invert.Common.Utilities;
using Invert.uFrame.Editor;
using UnityEditor;
using UnityEngine;

public class uFrameComponentSearchWindow : EditorWindow
{
    private string _SearchText;
    private MemberInfo[] _members;
    private Type[] _types;
    private List<MemberInfo> _quickFindMemberInfos = new List<MemberInfo>();
    public Action<uFrameComponentSearchWindow, MemberInfo> Complete { get; set; }
    public Action<uFrameComponentSearchWindow, MemberInfo> Remove { get; set; }
    public MemberInfo Selected { get; set; }
    public List<MemberInfo> SelectedMemberInfos { get; set; }

    public List<MemberInfo> QuickFindMemberInfos
    {
        get { return _quickFindMemberInfos; }
        set { _quickFindMemberInfos = value; }
    }

    [MenuItem("Tools/[u]Frame/Component Search")]
    internal static void ShowWindow(Action<uFrameComponentSearchWindow, MemberInfo> finished, Action<uFrameComponentSearchWindow, MemberInfo> remove, MemberInfo[] selectedMemberInfos, MemberInfo[] quickFinds)
    {
        var window = GetWindow<uFrameComponentSearchWindow>();
        window.title = "Add Property";
        window.minSize = new Vector2(240, 300);
        window.SelectedMemberInfos = selectedMemberInfos.ToList();
        window.Remove = remove;
        window.Complete = finished;
        window.QuickFindMemberInfos.Clear();
        window.QuickFindMemberInfos.AddRange(quickFinds);
        window.Show();
    }

    private void OnEnable()
    {
        //minSize = new Vector2(520, 400);
        //maxSize = new Vector2(520, 400);
        position = new Rect(position.x, position.y, 240, 300);
    }

    public static void DrawTitleBar(string subTitle)
    {
        //GUI.Label();
        ElementDesignerStyles.DoTilebar(subTitle);
    }

    public void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        _SearchText = GUILayout.TextField(_SearchText ?? "");
        if (EditorGUI.EndChangeCheck() || _members == null)
        {
            if (string.IsNullOrEmpty(_SearchText))
            {
                _members = QuickFindMemberInfos.Concat(SelectedMemberInfos).ToArray();
            }
            else
            {
                _members = ProjectInfo.TypeMemberSearch(_SearchText, FilterMemberType).ToArray();    
            }
            

        }
        GUILayout.Label("Search to find more...");
        foreach (var type in _members.Take(100).GroupBy(p=>p.DeclaringType).Take(4))
        {
            GUIHelpers.DoToolbar(type.Key.Name,true);
            foreach (var memberInfo in type)
            {
               
                if (GUIHelpers.DoTriggerButton(new UFStyle()
                {
                    BackgroundStyle = UBStyles.EventSmallButtonStyle,
                    Label = string.Format("{0}.{1}", memberInfo.DeclaringType.Name, memberInfo.Name),
                    ShowArrow = true,
                    FullWidth = false,
                    IconStyle = SelectedMemberInfos.Contains(memberInfo) ? UBStyles.TriggerActiveButtonStyle : UBStyles.TriggerInActiveButtonStyle
                }))
                {

                    Selected = memberInfo;
                    if (SelectedMemberInfos.Contains(memberInfo))
                    {
                        SelectedMemberInfos.Remove(memberInfo);
                        Remove(this, memberInfo);
                        EditorWindow.GetWindow<ElementsDesigner>().Diagram.Refresh();
                    }
                    else
                    {
                        SelectedMemberInfos.Add(memberInfo);
                        Complete(this, memberInfo);
                        EditorWindow.GetWindow<ElementsDesigner>().Diagram.Refresh();
                    }
                }
                
            }
            
        }
    }

    public static Type[] _AllowedTypes = new[]
    {
        typeof (int), typeof (bool), typeof (Vector2), typeof (Vector3), typeof (Rect), typeof (string), typeof (float),
        typeof (double),typeof (Quaternion)
    };

    private bool FilterMemberType(Type arg1)
    {
        if (arg1.IsSubclassOf(typeof(Enum))) return true;
        return _AllowedTypes.Contains(arg1);
    }
}