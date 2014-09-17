using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// A component for a property binding.  A component property binding will use reflection to pull the member information
/// so if performance is an issue I would recommend a code only binding.
/// 
/// Note: NGUI added a propertybinding class so this one is renamed to UFPropertyBinding.
/// </summary>
public class UFPropertyBinding : ComponentBinding
{
    public Component _TargetComponent;
    public List<string> _TargetProperties = new List<string>();
    public bool _TwoWay = false;
    protected MemberInfo _targetPropertyInfo;
    protected object _targetPropertyObject;
    private ModelPropertyBase _modelProperty;
    private BindableProperty _targetBindable;

    public BindableProperty TargetProperty
    {
        get
        {
            return _targetBindable ??
                   (_targetBindable = new BindableProperty(_targetPropertyObject, _targetPropertyInfo));
        }
    }

    protected override IBinding GetBinding()
    {
        if (string.IsNullOrEmpty(_ModelMemberName))
        {
            throw new Exception("Model Property Not Found.");
        }

        if (_TargetComponent == null) return null;

        var currentType = _TargetComponent.GetType();
        _targetPropertyObject = _TargetComponent;

        for (var i = 0; i < _TargetProperties.Count; i++)
        {
            var p = _TargetProperties[i];
            _targetPropertyInfo = currentType.GetMember(p).FirstOrDefault();
            if (_targetPropertyInfo == null)
            {
                throw new Exception(string.Format("Member {0} not found.", p));
            }
            if (i < _TargetProperties.Count - 1)
            {
                if (_targetPropertyInfo is FieldInfo)
                {
                    var field = ((FieldInfo)_targetPropertyInfo);
                    _targetPropertyObject = field.GetValue(_targetPropertyObject);
                    currentType = field.FieldType;
                }
                else if (_targetPropertyInfo is PropertyInfo)
                {
                    var prop = ((PropertyInfo)_targetPropertyInfo);
                    _targetPropertyObject = prop.GetValue(_targetPropertyObject, null);
                    currentType = prop.PropertyType;
                }
                else
                {
                    throw new Exception(string.Format("Member {0} is not a Property or field", _targetPropertyInfo.Name));
                }

                if (_targetPropertyObject == null)
                {
                    var path = string.Join(".", _TargetProperties.Take(i).ToArray());
                    throw new Exception(path + " is null on property binding.");
                }
            }
        }
        return new ModelPropertyBinding()
        {
            GetTargetValueDelegate = () => TargetProperty.Value,
            SetTargetValueDelegate = (v) => TargetProperty.Value = v,
            ModelMemberName = _ModelMemberName,
            Source = SourceView.ViewModelObject
        };
    }
}