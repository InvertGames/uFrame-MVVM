using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class ViewModelPropertyTrigger : UBCustomTrigger, IBindingProvider
{
    private ModelPropertyBinding _propertyBinding;

    public static IUBCSharpGenerator LastGenerator { get; set; }

    public string PropertyName { get; set; }

    public ViewBase View { get; set; }

    public static void Awake(IUBCSharpGenerator generator, IUBContext instance, TriggerInfo data)
    {
        if (LastGenerator == generator) return;
        var view = instance.GameObject.GetView();
        generator.AddField("_View", view.GetType());
        generator.AppendFormat("_View = GetComponent<{0}>();", view.GetType());
        LastGenerator = generator;
    }

    public static void Bind(IUBCSharpGenerator generator, IUBContext instance, TriggerInfo data, string propertyBindingName)
    {
        var view = instance.GameObject.GetView();
        var fieldInfo = view.ViewModelType.GetField(data.Data);
        var typeOf = fieldInfo.FieldType.GetGenericArguments().First();

        var str = @"var _PropertyNameBinding = new ModelPropertyBinding
        {
            SetTargetValueDelegate = value => { PropertyVariableName = (propertyType)value; ExecuteSheet(); },
            ModelPropertySelector = () => view.ViewModelObject.Properties[""PropertyName""],
            TwoWay = false,
            Source = view
        };

        view.AddBinding(_PropertyNameBinding);"
            .Replace("PropertyName", data.Data)
            .Replace("PropertyVariableName", data.Data.Replace("_", "").Replace("Property", ""))
            .Replace("propertyType", typeOf.FullName)
            .Replace("_propertyBinding", propertyBindingName)
            .Replace("ExecuteSheet()", generator.InvokeTriggerSheet())
            ;
        generator.Append(str);
    }

    public static void Unbind(IUBCSharpGenerator generator)
    {
    }

    public void Awake()
    {
    }

    public void Bind(ViewBase view)
    {
        _propertyBinding = new ModelPropertyBinding
        {
            SetTargetValueDelegate = o => ExecuteSheet(),
            ModelPropertySelector = () => view.ViewModelObject.Properties[PropertyName],
            TwoWay = false,
            Source = view
        };

        view.AddBinding(_propertyBinding);
    }

    public override void Initialize(TriggerInfo triggerInfo, Dictionary<string, object> settings)
    {
        PropertyName = triggerInfo.Data;
    }

    public override void Initialized()
    {
        base.Initialized();
        var view = GetComponent<ViewBase>();
        if (view == null)
            throw new Exception("View Not Found");

        view.BindingProviders.Add(this);
    }

    public void Unbind(ViewBase viewBase)
    {
        if (_propertyBinding != null)
        {
            _propertyBinding.Unbind();
        }
    }
}