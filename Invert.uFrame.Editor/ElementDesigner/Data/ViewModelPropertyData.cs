using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using UnityEngine;

[Serializable]
public class ViewModelPropertyData : DiagramNodeItem, IViewModelItem
{
    public override void Serialize(JSONClass cls)
    {
        base.Serialize(cls);
        cls.Add("ItemType", new JSONData(_type));
        cls.Add("IsRealTime", new JSONData(_isRealTimeProperty));
    }

    public override void Deserialize(JSONClass cls)
    {
        base.Deserialize(cls);
        _type = cls["ItemType"].Value;
        _isRealTimeProperty = cls["IsRealTime"].AsBool;
    }

    [SerializeField]
    private string _type = string.Empty;

    [SerializeField]
    private bool _isRealTimeProperty;

    public Type Type
    {
        get
        {
            if (_type == null)
            {
                return typeof(string);
            }
            return Type.GetType(_type);
        }
        set { _type = value.AssemblyQualifiedName; }
    }

    public object DefaultValue { get; set; }

    public string NameAsChangedMethod
    {
        get { return string.Format("{0}Changed", Name); }
    }
    public string NameAsTwoWayMethod
    {
        get { return string.Format("Get{0}TwoWayValue", Name); }
    }
    public override string Label
    {
        get { return RelatedTypeName + ": " + Name; }
    }

    public override RenameRefactorer CreateRenameRefactorer()
    {
        return new RenamePropertyRefactorer(this);
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        var element = target as IDiagramNode;
        if (element != null)
        {
            RelatedType = element.AssemblyQualifiedName;
        }
    }

    public override bool CanCreateLink(IDrawable target)
    {
        return target is ElementDataBase || target is EnumData;
    }

    public override void RemoveLink(IDiagramNode target)
    {
        RelatedType = typeof(string).AssemblyQualifiedName;
    }

    public string RelatedType
    {
        get { return _type; }
        set { _type = value; }
    }

    public bool IsRealTimeProperty
    {
        get { return _isRealTimeProperty; }
        set { _isRealTimeProperty = value; }
    }


    public string RelatedTypeName
    {
        get
        {

            return RelatedType.Split(',').FirstOrDefault() ?? "No Type";
        }
    }

    public bool AllowEmptyRelatedType
    {
        get { return false; }
    }

    public IEnumerable<string> BindingMethodNames
    {
        get
        {
            yield return NameAsChangedMethod;
        }
    }

    public string FieldName
    {
        get
        {
            return string.Format("_{0}Property", Name);
        }
    }

    public string ViewFieldName
    {
        get
        {
            return string.Format("_{0}", Name);
        }
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] diagramNode)
    {
        foreach (var viewModelData in diagramNode)
        {
            if (viewModelData.Name == null) continue;
            if (viewModelData.Name == RelatedTypeName)
            {
                yield return new AssociationLink()
                {
                    Item = this,
                    Element = viewModelData
                };
            }
        }
    }

    public override string Highlighter
    {
        get { return null; }
    }

    public override string FullLabel { get { return RelatedTypeName + Name; } }

    public override bool IsSelectable { get { return true; } }

    public override void Remove(IDiagramNode diagramNode)
    {
        var data = diagramNode as ElementDataBase;
        data.Properties.Remove(this);
        data.Dirty = true;
    }


    public string NameAsPrefabBindingOption
    {
        get { return string.Format("_{0}Prefab", Name); }
    }

    public string NameAsTwoWayBindingOption
    {
        get { return string.Format("_{0}IsTwoWay", Name); }
    }
    public string NameAsBindingOption
    {
        get { return string.Format("_Bind{0}", Name); }
    }

   
}