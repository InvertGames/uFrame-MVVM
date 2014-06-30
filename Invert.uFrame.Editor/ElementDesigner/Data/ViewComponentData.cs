using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ViewComponentData : DiagramNode
{
    [SerializeField]
    private string _baseIdentifier;

    [SerializeField]
    private string _elementIdentifier;

    public IEnumerable<ViewComponentData> AllBaseTypes
    {
        get
        {
            var t = this.Base;
            while (t != null)
            {
                yield return t;
                t = t.Base;
            }
        }
    }

    public ViewComponentData Base
    {
        get { return Data.ViewComponents.FirstOrDefault(p => p.Identifier == this.BaseIdentifier); }
    }

    public string BaseIdentifier
    {
        get { return _baseIdentifier; }
        set
        {
            _baseIdentifier = value;
            Dirty = true;
        }
    }

    public override IEnumerable<IDiagramNodeItem> ContainedItems
    {
        get { yield break; }
        set { }
    }

    public Type CurrentType
    {
        get { return Type.GetType(AssemblyQualifiedName); }
    }

    public ElementData Element
    {
        get
        {
            if (Base != null)
            {
                return Base.Element;
            }
            return Data.Elements.FirstOrDefault(p => p.Identifier == ElementIdentifier);
        }
    }

    public string ElementIdentifier
    {
        get { return _elementIdentifier; }
        set { _elementIdentifier = value; }
    }

    public override IEnumerable<IDiagramNodeItem> Items
    {
        get { yield break; }
    }

    public override string Label
    {
        get { return Name; }
    }

    public override bool CanCreateLink(IDrawable target)
    {
        var viewComponent = target as ViewComponentData;
        if (viewComponent != null)
        {
            if (Element == null)
            {
                return false;
            }

            if (viewComponent.Identifier == this.BaseIdentifier)
            {
                return false;
            }
            if (AllBaseTypes.Any(p => p.Identifier == viewComponent.Identifier))
            {
                return false;
            }
        }
        var elementData = target as ElementData;
        if (elementData != null)
        {
            if (Base != null && Base.Element != null)
            {
                return false;
            }
        }
        return target is ViewComponentData || target is ElementData;
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        var viewComponentData = target as ViewComponentData;
        if (viewComponentData != null)
        {
            viewComponentData.BaseIdentifier = this.Identifier;
            viewComponentData.ElementIdentifier = null;
        }
        var element = target as ElementData;
        if (element != null)
            ElementIdentifier = element.Identifier;
    }

    public override RenameRefactorer CreateRenameRefactorer()
    {
        return new RenameViewComponentRefactorer(this);
    }

    public override void Deserialize(JSONClass cls)
    {
        base.Deserialize(cls);
        _elementIdentifier = cls["ElementIdentifier"].Value;
        _baseIdentifier = cls["BaseIdentifier"].Value;
    }

    public override bool EndEditing()
    {
        return base.EndEditing();
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        foreach (var diagramItem in nodes.OfType<ViewComponentData>().Where(p => p.BaseIdentifier == Identifier))
        {
            yield return new ViewComponentLink()
            {
                Base = this,
                Derived = diagramItem
            };
        }

        var element = nodes.FirstOrDefault(p => p.Identifier == ElementIdentifier);
        if (element != null)
        {
            if (element != this && element.Identifier != this.Identifier)
            {
                yield return new GenericLink()
                {
                    Element = element,
                    Node = this
                };
            }
        }
    }

    [DiagramContextMenu("Open Code")]
    public void OpenViewComponent(IElementsDataRepository repository)
    {
        var gameObject = new GameObject();
        var behaviour = gameObject.AddComponent(this.Name) as MonoBehaviour;
        if (behaviour != null)
        {
            var monoScript = MonoScript.FromMonoBehaviour(behaviour);
            AssetDatabase.OpenAsset(monoScript);
        }
        GameObject.DestroyImmediate(gameObject);
    }

    public override void RemoveFromDiagram()
    {
        base.RemoveFromDiagram();
        Data.RemoveNode(this);
        foreach (var viewComponentData in Data.ViewComponents)
        {
            if (viewComponentData.BaseIdentifier == this.Identifier)
            {
                viewComponentData.BaseIdentifier = null;
            }
        }
    }

    public override void RemoveLink(IDiagramNode target)
    {
        var viewComponent = target as ViewComponentData;
        if (viewComponent != null)
        {
            viewComponent.BaseIdentifier = null;
            viewComponent.Dirty = true;
        }
    }

    public override void Serialize(JSONClass cls)
    {
        base.Serialize(cls);
        cls.Add("ElementIdentifier", new JSONData(_elementIdentifier));
        cls.Add("BaseIdentifier", new JSONData(_baseIdentifier));
    }
}