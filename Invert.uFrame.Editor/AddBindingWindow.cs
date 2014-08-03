using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Invert.Common.UI;
using Invert.uFrame.Code.Bindings;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using UnityEditor;
using UnityEngine;

public class AddBindingWindow : SearchableScrollWindow
{
    private ViewData _ViewData;
    private MethodInfo[] _MemberMethods;
    private List<UFStyle> _addedGenerators = new List<UFStyle>();
    public UFStyle[] Items { get; set; }

    public ElementsDesigner ElementsDesigner
    {
        get { return EditorWindow.GetWindow<ElementsDesigner>(); }
    }

    //public List<UFStyle> AddedGenerators
    //{
    //    get { return _addedGenerators; }
    //    set { _addedGenerators = value; }
    //}

    public static void Init(string title, ViewData data)
    {
        // Get existing open window or if none, make a new one:
        var window = (AddBindingWindow)GetWindow(typeof(AddBindingWindow));
        window.title = title;
        window._ViewData = data;
        window.ApplySearch();
        window.minSize = new Vector2(500,300);
        window.Show();


    }

    public IBindingGenerator LastSelected { get; set; }

    public override void OnGUI()
    {
        if (_ViewData != null && _ViewData.Data == null)
        {
            this.Close();
            return;
        }
        if (_ViewData == null)
        {

            if (ElementsDesigner != null && ElementsDesigner.Diagram != null)
            {

                _ViewData = ElementsDesigner.Diagram.SelectedData as ViewData;
            }
            if (_ViewData == null)
            {
                EditorGUILayout.HelpBox("Selected a view first.", MessageType.Info);
                return;
            }
            else
            {
                ApplySearch();
            }

        }
        else
        {


        }
        if (_ViewData != null && _ViewData.ViewForElement == null)
        {
            EditorGUILayout.HelpBox("This view must be associated with an element in order to add bindings.",
                MessageType.Error);
        }
        else if (_ViewData != null)
        {
            CallOnGui();
            
        }



    }

    public void CallOnGui()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width / 3f));
        base.OnGUI();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        GUIHelpers.DoToolbar(_ViewData.Name + " Preview",true,null,null,null,null,null,false);

        if (LastSelected != null)
            EditorGUILayout.TextArea(GetViewPreview(),GUILayout.Height(Screen.height - 150f));

        
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
      
    }

    public void Apply()
    {
        //_ViewData.NewBindings.AddRange(AddedGenerators.Select(p=>p.Tag as IBindingGenerator));
    }

    public override void OnGUIScrollView()
    {

        foreach (var group in Items.Where(p => !_ViewData.NewBindings.Contains(p.Tag as IBindingGenerator) && p.Enabled == true).GroupBy(p => p.Group))
        {
            GUIHelpers.DoToolbar(group.Key);
            foreach (var item in group)
            {
                if (GUIHelpers.DoTriggerButton(item))
                {
                    LastSelected = item.Tag as IBindingGenerator;
                
                    _ViewData.NewBindings.Add(LastSelected);
                    ElementsDesigner.Diagram.Refresh();
                    ElementsDesigner.Repaint();
                }
            }
        }
        GUIHelpers.DoToolbar("Bindings To Add");
        foreach (var item in _ViewData.NewBindings.ToArray())
        {
            if (GUIHelpers.DoTriggerButton(new UFStyle() { Label = item.MethodName, IsWindow = true,FullWidth = true}))
            {
                LastSelected = item;
                _ViewData.NewBindings.Remove(item);
                ElementsDesigner.Diagram.Refresh();
                ElementsDesigner.Repaint();
            }
        }

        GUIHelpers.DoToolbar("Existing");
        foreach (var item in Items.Where(p => !_ViewData.NewBindings.Contains(p.Tag as IBindingGenerator) && !p.Enabled))
        {
            if (GUIHelpers.DoTriggerButton(item))
            {
                
            }
        }

    }

    public string GetViewPreview()
    {
        
        var refactorContext = new RefactorContext(_ViewData.BindingInsertMethodRefactorer);
        var settings = ElementsDesigner.Diagram.Data.Settings;
        var pathStrategy = settings.CodePathStrategy;
        var viewFilePath = System.IO.Path.Combine(settings.CodePathStrategy.AssetPath, pathStrategy.GetEditableViewFilename(_ViewData));
        return refactorContext.RefactorFile(viewFilePath, false);
    }
    protected override void ApplySearch()
    {
        if (_ViewData == null) return;
        _MemberMethods = _ViewData.BindingMethods.ToArray();
        Generators = uFrameEditor.GetBindingGeneratorsFor(_ViewData.ViewForElement,true,false,true);
        
        //Where(p => _MemberMethods.FirstOrDefault(x => x.Name == p.MethodName) != null)
        Items = Generators.Select(p => new UFStyle()
        {
            Label = p.MethodName,
            Tag = p,
            //SubLabel = p.Item.Name,
            Group = p.Item.Node.Name,
            FullWidth = true,
            IsWindow = true,
            Enabled = _MemberMethods.FirstOrDefault(x => x.Name == p.MethodName) == null
        }).ToArray();
    }
    public IEnumerable<IBindingGenerator> Generators { get; set; }
}