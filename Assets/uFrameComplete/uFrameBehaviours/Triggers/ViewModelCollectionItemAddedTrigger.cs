using System.Collections.Generic;

public class ViewModelCollectionItemAddedTrigger : UBCustomTrigger, IBindingProvider
{
    public string PropertyName { get; set; }

    public void Awake()
    {
        this.GetView().BindingProviders.Add(this);
    }
    public void Bind(ViewBase view)
    {
        var property = view.ViewModelObject.Properties[PropertyName] as IModelCollection;
        property.CollectionChanged += PropertyOnChanged;
    }

    public override void Initialize(TriggerInfo triggerInfo, Dictionary<string, object> settings)
    {
        PropertyName = triggerInfo.Data;
    }

    public void Unbind(ViewBase view)
    {
        var property = view.ViewModelObject.Properties[PropertyName] as IModelCollection;
        property.CollectionChanged -= PropertyOnChanged;
    }

    public static IEnumerable<IUBVariableDeclare> ViewModelCollectionItemAddedTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "item", ValueType = typeof(ViewModel) };
    }

    protected virtual void PropertyOnChanged(ModelCollectionChangeEvent changeArgs)
    {
        if (changeArgs.NewItems.Length > 0)
        {

            for (var i = 0; i < changeArgs.NewItems.Length; i++)
                ExecuteSheetWithVars(new UBSystemObject(changeArgs.NewItems[i], changeArgs.NewItems[i].GetType())
                {
                    Guid = "item",
                    Name = "item"
                }
               );
        }
    }
}

public class ViewModelCollectionItemRemovedTrigger : ViewModelCollectionItemAddedTrigger, IBindingProvider
{
    public static IEnumerable<IUBVariableDeclare> ViewModelCollectionItemRemovedTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "item", ValueType = typeof(ViewModel) };
    }
    protected override void PropertyOnChanged(ModelCollectionChangeEvent changeArgs)
    {
        if (changeArgs.OldItems.Length > 0)
        {
            for (var i = 0; i < changeArgs.OldItems.Length; i++)
                ExecuteSheetWithVars(new UBSystemObject(changeArgs.OldItems[i], changeArgs.OldItems[i].GetType())
                {
                    Guid = "item",
                    Name = "item"
                }
               );
        }
    }
}