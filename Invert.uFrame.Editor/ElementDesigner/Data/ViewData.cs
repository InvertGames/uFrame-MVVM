using System;
using System.Collections.Generic;
using System.Linq;
using Invert.uFrame.Editor.Refactoring;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ViewData : DiagramItem, ISubSystemType
{
    [SerializeField]
    private string _forAssemblyQualifiedName;
    public override string Label { get { return Name; } }

    [SerializeField]
    private List<string> _componentIdentifiers = new List<string>();

    public IEnumerable<ViewComponentData> Components
    {
        get { return Data.ViewComponents.Where(p => ComponentIdentifiers.Contains(p.Identifier)); }
    }

    public override IEnumerable<IDiagramSubItem> Items
    {
        get
        {
            yield break;
            //if (Behaviours == null)
            //    yield break;

            //foreach (var behaviourSubItem in Behaviours)
            //{
            //    yield return behaviourSubItem;
            //}
        }
    }

    [DiagramContextMenu("Open Code")]
    public void OpenView(IElementsDataRepository repository)
    {
        var filename = repository.GetViewCustomFilename(this.Name);

        var scriptAsset = AssetDatabase.LoadAssetAtPath(filename, typeof(TextAsset));

        AssetDatabase.OpenAsset(scriptAsset);
    }
    public override RenameRefactorer CreateRenameRefactorer()
    {
        return new RenameViewRefactorer(this);
    }
    public override void EndEditing(IElementsDataRepository repository)
    {
        var oldAssemblyName = AssemblyQualifiedName;
        base.EndEditing(repository);
        foreach (var v in Data.Views.Where(p => p.ForAssemblyQualifiedName == oldAssemblyName))
        {
            v.ForAssemblyQualifiedName = AssemblyQualifiedName;
        }
    }

    public ElementDataBase ViewForElement
    {
        get
        {
            return Data.ViewModels.FirstOrDefault(p => p.AssemblyQualifiedName == ForAssemblyQualifiedName);
        }
    }

    public string ForAssemblyQualifiedName
    {
        get { return _forAssemblyQualifiedName; }
        set { _forAssemblyQualifiedName = value; }
    }


    public string NameAsView
    {
        get
        {
            return string.Format("{0}", Name);
        }
    }
    public string NameAsViewBase
    {
        get
        {
            return string.Format("{0}Base", Name);
        }
    }
    public string ViewAssemblyQualifiedName
    {
        get
        {
            return UFrameAssetManager.DesignerVMAssemblyName.Replace("ViewModel", NameAsView);
        }
    }
    public Type CurrentViewType
    {
        get
        {
            return Type.GetType(ViewAssemblyQualifiedName);
        }
    }

    public override void RemoveFromDiagram()
    {
        Data.Views.Remove(this);
        foreach (var source in Data.Views.Where(p=>p.ForAssemblyQualifiedName == this.AssemblyQualifiedName))
        {
            source.ForAssemblyQualifiedName = null;
        }
    }
    public override void CreateLink(IDiagramItem container, IDrawable target)
    {
        var i = target as ViewData;

        i.ForAssemblyQualifiedName = AssemblyQualifiedName;
        
    }
    public override bool CanCreateLink(IDrawable target)
    {
        return target is ViewData && target != this;
    }
    public override IEnumerable<IDiagramLink> GetLinks(IDiagramItem[] elementDesignerData)
    {
        var vm = ViewForElement;
        //var items = new UFrameBehaviours[] { };
        //if (vm != null)
        //{
        //    items = UBAssetManager.Behaviours.OfType<UFrameBehaviours>()
        //        .Where(p => p != null && p.ViewModelTypeString == vm.ViewModelAssemblyQualifiedName).ToArray();
        //}

        //Behaviours = items.Select(p => new BehaviourSubItem() { Behaviour = p }).ToArray();

        var item = elementDesignerData.FirstOrDefault(p => p.AssemblyQualifiedName == ForAssemblyQualifiedName);
        if (item != null)
        {
            yield return new ViewLink()
            {
                Element = item,
                Data = this
            };
        }
    }

    //public BehaviourSubItem[] Behaviours { get; set; }

    public List<string> ComponentIdentifiers
    {
        get { return _componentIdentifiers; }
        set { _componentIdentifiers = value; }
    }

    [DiagramContextMenu("Add To/Scene",0)]
    public void AddViewToScene()
    {
        if (this.CurrentViewType == null)
        {
            EditorUtility.DisplayDialog("Can't add to scene", "The diagram must be saved and have no compiler errors.",
                "OK");
            return;
        }

        GameObject obj = new GameObject(this.Name);
        obj.AddComponent(CurrentViewType);

    }
    [DiagramContextMenu("Add To/Selection", 1)]
    public void AddToSelection()
    {
        var selection = Selection.activeObject as GameObject;

        if (selection != null)
        {
            if (this.CurrentViewType == null)
            {
                EditorUtility.DisplayDialog("Can't add to selection", "The diagram must be saved and have no compiler errors.",
                    "OK");
                return;
            }
            selection.AddComponent(CurrentViewType);
        }

    }
    public override void RemoveLink(IDiagramItem target)
    {
        var viewData = target as ViewData;
        if (viewData != null)
        {
            viewData.ForAssemblyQualifiedName = null;
        }
        //var elementData = target as ElementData;
        //if (target is )
    }


   
}