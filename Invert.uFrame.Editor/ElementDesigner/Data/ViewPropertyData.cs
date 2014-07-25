using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Invert.uFrame.Editor;
using UnityEngine;

public class ViewPropertyData : DiagramNodeItem
{
    private string _componentTypeName = "";
    private string _componentTypeShortName = "";

    private string _componentProperty = "";
    private Type _componentAssemblyType = null;

    public override void Serialize(JSONClass cls)
    {
        base.Serialize(cls);
        cls.Add("ComponentType", new JSONData(_componentTypeName ?? string.Empty));
        cls.Add("ComponentProperty", new JSONData(_componentProperty ?? string.Empty));
    }

    public override void Deserialize(JSONClass cls)
    {
        base.Deserialize(cls);
        if (cls["ComponentType"] != null)
        ComponentAssemblyType = uFrameEditor.FindType(cls["ComponentType"].Value);
        if (cls["ComponentProperty"] != null)
        ComponentProperty = cls["ComponentProperty"].Value;

    }

    public override string FullLabel
    {
        get { return Label; }
    }

    public override string Name
    {
        get { return Label; }
        set
        {
            // nothing here
        }
    }

    public override string Label
    {
        get
        {
            return string.Format("{0}.{1}",_componentTypeShortName ?? string.Empty,ComponentProperty);
        }
    }

    public string Expression
    {
        get { return string.Format("{0}.{1}", NameAsCachedProperty, ComponentProperty); }
    }

    public string NameAsCachedProperty
    {
        get
        {
            var name = ComponentTypeName.Split('.').Last();

            return name.Substring(0, 1).ToUpper() + name.Substring(1);
        }
    }
    public string ComponentTypeName
    {
        get { return _componentTypeName; }
        set
        {
            _componentTypeName = value;
            ComponentAssemblyType = uFrameEditor.FindType(_componentTypeName);
            //if (_componentAssemblyType != null)
            //{
            //    _componentTypeShortName = ComponentAssemblyType.Name;
            //}
        }
    }

    public Type ComponentAssemblyType
    {
        get
        {
            if (_componentAssemblyType == null)
            {
                ComponentAssemblyType = typeof (Transform);
            }
            return _componentAssemblyType;
        }
        set
        {
            _componentAssemblyType = value ?? typeof(Transform);
            if (value != null)
            {
                _componentTypeName = value.FullName;
                _componentTypeShortName = value.Name;
            }
            
            
        }
    }

    public Type MemberType
    {
        get
        {
            var mi = MemberInfo;
            if (mi == null) return null;
            var propertyInfo = mi as PropertyInfo;
            if (propertyInfo == null)
            {
                var fieldInfo = mi as FieldInfo;
                if (fieldInfo == null) return null;
                return fieldInfo.FieldType;
            }
            return propertyInfo.PropertyType;
        }
    }
    public MemberInfo MemberInfo
    {
        get
        {
            var cat = ComponentAssemblyType;
            if (cat == null) return null;
            return cat.GetMember(ComponentProperty).FirstOrDefault();
        }
    }

    public string ComponentProperty
    {
        get
        {
            return _componentProperty;
        }
        set
        {
            _componentProperty = value;
        }
    }

    public string NameAsProperty
    {
        get
        {
            return string.Format("{0}{1}", ComponentProperty.Substring(0, 1).ToUpper(), ComponentProperty.Substring(1));
        }
    }

    public string NameAsField
    {
        get
        {
            return string.Format("_{0}{1}", ComponentProperty.Substring(0, 1).ToLower(), ComponentProperty.Substring(1)); 
        }
    }
    public override bool CanCreateLink(IDrawable target)
    {
        return false;
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] diagramNode)
    {
        yield break;
    }
    
    public override void Remove(IDiagramNode diagramNode)
    {
        var view = diagramNode as ViewData;
        if (view != null)
        {
            view.Properties.Remove(this);
        }
    }

    public override void RemoveLink(IDiagramNode target)
    {
        
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        
    }
}