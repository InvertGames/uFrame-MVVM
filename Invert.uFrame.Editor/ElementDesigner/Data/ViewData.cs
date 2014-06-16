using System;
using System.Collections.Generic;
using System.Linq;
using Invert.uFrame.Editor.Refactoring;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ViewData : DiagramNode, ISubSystemType
{
    [SerializeField]
    private string _forAssemblyQualifiedName;
    public override string Label { get { return Name; } }

    [SerializeField]
    private List<string> _componentIdentifiers = new List<string>();

    [SerializeField]
    private string _baseViewIdentifier;

    public IEnumerable<ViewComponentData> Components
    {
        get { return Data.ViewComponents.Where(p => ComponentIdentifiers.Contains(p.Identifier)); }
    }

    public override IEnumerable<IDiagramNodeItem> Items
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

    public override RenameRefactorer CreateRenameRefactorer()
    {
        return new RenameViewRefactorer(this);
    }
    public override void EndEditing()
    {
        var oldAssemblyName = AssemblyQualifiedName;
        base.EndEditing();
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

    /// <summary>
    /// The identifier to the base view this view will derived from
    /// </summary>
    public string BaseViewIdentifier
    {
        get { return _baseViewIdentifier; }
        set { _baseViewIdentifier = value; }
    }
    /// <summary>
    /// The name of the view that this view will derive from
    /// </summary>
    public string BaseViewName
    {
        get
        {
            var baseView = BaseView;
            if (baseView != null)
            {
                return baseView.NameAsView;
            }
            var element = ViewForElement;
            if (element != null)
            {
                return element.NameAsViewBase;
            }
            return "[None]";
        }
    }
    /// <summary>
    /// The baseview class if any
    /// </summary>
    public ViewData BaseView
    {
        get
        {
            if (string.IsNullOrEmpty(BaseViewIdentifier)) return null;
            return Data.Views.FirstOrDefault(p => p.Identifier == BaseViewIdentifier);
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
    public string NameAsViewViewBase
    {
        get
        {
            return string.Format("{0}ViewBase", Name);
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
        base.RemoveFromDiagram();
        Data.Views.Remove(this);
        foreach (var source in Data.Views.Where(p=>p.ForAssemblyQualifiedName == this.AssemblyQualifiedName))
        {
            source.ForAssemblyQualifiedName = null;
        }
    }
    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        var i = target as ViewData;

        i.ForAssemblyQualifiedName = AssemblyQualifiedName;
        
    }
    public override bool CanCreateLink(IDrawable target)
    {
        return target is ViewData && target != this;
    }
    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        var vm = ViewForElement;
        //var items = new UFrameBehaviours[] { };
        //if (vm != null)
        //{
        //    items = UBAssetManager.Behaviours.OfType<UFrameBehaviours>()
        //        .Where(p => p != null && p.ViewModelTypeString == vm.ViewModelAssemblyQualifiedName).ToArray();
        //}

        //Behaviours = items.Select(p => new BehaviourSubItem() { Behaviour = p }).ToArray();

        var item = nodes.FirstOrDefault(p => p.AssemblyQualifiedName == ForAssemblyQualifiedName);
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


    public override void RemoveLink(IDiagramNode target)
    {
        var viewData = target as ViewData;
        if (viewData != null)
        {
            viewData.ForAssemblyQualifiedName = null;
            viewData.BaseViewIdentifier = null;
        }
        //viewData.BaseViewIdentifier = null;
        BaseViewIdentifier = null;
        //var elementData = target as ElementData;
        //if (target is )
    }


   
}