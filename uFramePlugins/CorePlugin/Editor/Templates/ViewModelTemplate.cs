using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.Editor;
using uFrame.Graphs;
using UnityEngine;

[TemplateClass("ViewModels", MemberGeneratorLocation.Both, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT)]
public partial class ViewModelTemplate : ViewModel, IClassTemplate<ElementNode>
{
    #region Template Stuff
    public TemplateContext<ElementNode> Ctx { get; set; }

    public void TemplateSetup()
    {
        // Ensure the namespaces for each property type are property set up
        
        foreach (var property in Ctx.Data.AllProperties)
        {
            var type = InvertApplication.FindTypeByName(property.RelatedTypeName);
            if (type == null) continue;
            Ctx.TryAddNamespace(type.Namespace);
        }
        StateMachineProperties = Ctx.Data.AllProperties.Where(p => (p.RelatedNode() is StateMachineNode)).ToArray();
        ViewModelProperties = Ctx.Data.AllProperties.Where(p => !StateMachineProperties.Contains(p)).ToArray();
         
        Ctx.AddIterator("ResetComputed", (d) => d.ComputedProperties);
        Ctx.AddIterator("Compute", (d) => d.ComputedProperties);
        Ctx.AddIterator("ComputedDependents", (d) => d.ComputedProperties);
        Ctx.AddIterator("ViewModelProperty", (d) => ViewModelProperties);
        Ctx.AddIterator("ViewModelValueProperty", (d) => ViewModelProperties);
        Ctx.AddIterator("CollectionProperty", (d) => d.Collections);
        Ctx.AddIterator("CommandItems", (d) => d.Commands);
        Ctx.AddIterator("CommandMethod", (d) => d.Commands);
        Ctx.AddIterator("CollectionChanged", (d) => d.Collections.Where(p => p.RelatedTypeName != null));

        Ctx.AddIterator("StateMachineProperty", (d)=> StateMachineProperties);
        Ctx.AddIterator("StateMachineCurrentState", (d) => StateMachineProperties);

        foreach (var item in Ctx.Data.ComputedProperties)
        {
            Ctx.CurrentDecleration._private_(typeof(IDisposable), "_{0}Disposable", item.Name);
        }
    }

    public ITypedItem[] ViewModelProperties { get; set; }

    public ITypedItem[] StateMachineProperties { get; set; }

    #endregion

    [TemplateConstructor(MemberGeneratorLocation.Both, "controller", "initialize")]
    public void ControllerConstructor(Controller controller, bool initialize)
    {
        Ctx.CurrentConstructor.Parameters[0].Type = (Ctx.Data.Name.AsController() + "Base").ToCodeReference();
        Ctx.CurrentConstructor.Parameters[1].Name = "initialize = true";
    }

    [TemplateMethod(MemberGeneratorLocation.Both, true)]
    public override void Bind()
    {
        if (!Ctx.IsDesignerFile) return;

        foreach (var property in ViewModelProperties)
        {
            Ctx._("{0} = new P<{1}>(this, \"{2}\")", property.Name.AsSubscribableField(), property.RelatedTypeName,property.Name);
        }

        foreach (var property in Ctx.Data.Collections)
        {
            Ctx._("{0} = new ModelCollection<{1}>(this, \"{2}\")", property.Name.AsField(), property.RelatedTypeName,property.Name);
            Ctx._("{0}.CollectionChanged += {1}CollectionChanged", property.Name.AsField(), property.Name);
        }

        foreach (var item in StateMachineProperties)
        {
            Ctx._("{0} = new {1}(this, \"{2}\")", item.Name.AsSubscribableField(), item.RelatedTypeName,item.Name);
        }

        foreach (var item in Ctx.Data.ComputedProperties)
        {
            Ctx._("Reset{0}()", item.Name);
        }

    }

    [TemplateMethod(MemberGeneratorLocation.DesignerFile)]
    protected override void WireCommands(Controller controller)
    {
        //base.WireCommands(controller);
        var varName = Ctx.Data.Name.ToLower();
        Ctx._("var {0} = controller as {1}", varName, Ctx.Data.Name.AsController());
        foreach (var command in Ctx.Data.Commands.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)))
        {
            Ctx._("this.{0} = new CommandWithSender<{1}>(this as {1}, {2}.{0})", command.Name, Ctx.Data.Name.AsViewModel(), varName);
        }
        foreach (var command in Ctx.Data.Commands.Where(p => !string.IsNullOrEmpty(p.RelatedTypeName)))
        {
            Ctx._("this.{0} = new CommandWithSenderAndArgument<{1},{3}>(this as {1}, {2}.{0})", command.Name, Ctx.Data.Name.AsViewModel(), varName, command.RelatedTypeName);
        }
    }
    #region Properties

    [TemplateProperty(uFrameFormats.SUBSCRIBABLE_PROPERTY_FORMAT, AutoFillType.NameAndTypeWithBackingField)]
    public virtual float StateMachineProperty
    {
        get
        {
            return 0;
        }
    }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameOnly)]
    public virtual State StateMachineCurrentState
    {
        get
        {
            Ctx._("return {0}.Value", Ctx.Item.Name.AsSubscribableProperty());
            return null;
        }
        set
        {
            Ctx._("{0}.Value = value", Ctx.Item.Name.AsSubscribableProperty());
        }
    }
    #endregion
    #region Properties

    [TemplateProperty(uFrameFormats.SUBSCRIBABLE_PROPERTY_FORMAT, AutoFillType.NameAndTypeWithBackingField)]
    public virtual P<float> ViewModelProperty
    {
        get
        {
            return _viewModelProperty;
        }
        //set { _viewModelProperty = value; }
    }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndType)]
    public virtual Single ViewModelValueProperty
    {
        get
        {
            Ctx._("return {0}.Value", Ctx.Item.Name.AsSubscribableProperty());
            return 0f;
        }
        set
        {
            Ctx._("{0}.Value = value", Ctx.Item.Name.AsSubscribableProperty());
        }
    }
    #endregion

    #region Collections
    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndTypeWithBackingField)]
    public virtual ModelCollection<Single> CollectionProperty
    {
        get { return null; }
    }
    [TemplateMethod("{0}CollectionChanged", MemberGeneratorLocation.DesignerFile, true, AutoFill = AutoFillType.NameOnly)]
    protected virtual void CollectionChanged()
    {
        // Doesn't like this one in a normal parameter
        Ctx.CurrentMethod.Parameters.Add(
            new CodeParameterDeclarationExpression("System.Collections.Specialized.NotifyCollectionChangedEventArgs",
                "args"));

    }
    #endregion

    #region Commands
    [TemplateProperty("{0}", AutoFillType.NameOnlyWithBackingField)]
    public virtual ICommand CommandItems { get; set; }
    #endregion

    #region Serialization
    [TemplateMethod]
    public override void Read(ISerializerStream stream)
    {
        foreach (var property in Ctx.Data.Properties.Where(x => x.RelatedTypeName != null && x.Type != null && AcceptableTypes.ContainsKey(x.Type)))
        {
            Ctx._("stream.Deserialize{0}(\"{1}\")", AcceptableTypes[property.Type], property.Name);
        }
        foreach (var property in Ctx.Data.Collections.Where(x => x.RelatedTypeName != null && x.Type != null && AcceptableTypes.ContainsKey(x.Type)))
        {
            Ctx._("stream.DeserializeArray<{0}>(\"{1}\")", AcceptableTypes[property.Type], property.Name);
        }
    }

    [TemplateMethod]
    public override void Write(ISerializerStream stream)
    {
        foreach (var property in Ctx.Data.Properties.Where(x => x.Type != null && AcceptableTypes.ContainsKey(x.Type)))
        {
            Ctx._("stream.Serialize{0}(\"{1}\", this.{1})", AcceptableTypes[property.Type], property.Name);
        }
        foreach (var property in Ctx.Data.Collections.Where(x => x.Type != null && AcceptableTypes.ContainsKey(x.Type)))
        {
            Ctx._("stream.SerializeArray(\"{0}\", this.{0})", property.Name);
        }
    }
    #endregion

    #region Reflection

    [TemplateMethod(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.None)]
    protected override void FillCommands(List<ViewModelCommandInfo> list)
    {
        //base.FillCommands(list);
        foreach (var commandChildItem in Ctx.Data.Commands)
        {
            Ctx._("list.Add(new ViewModelCommandInfo(\"{0}\", {0}) {{ ParameterType = typeof({1}) }})",
               commandChildItem.Name,
               string.IsNullOrEmpty(commandChildItem.RelatedTypeName) ? "void" : commandChildItem.RelatedTypeName
            );
        }
        
    }


    [TemplateMethod(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.None)]
    protected override void FillProperties(List<ViewModelPropertyInfo> list)
    {
        //base.FillProperties(list);
        foreach (var property in Ctx.Data.AllProperties)
        {
            Ctx._("list.Add(new ViewModelPropertyInfo({0}, {1}, {2}, {3}))",
               property.Name.AsSubscribableField(),
               property.RelatedNode() is ElementNode ? "true" : "false",
               "false", // TODO FOR ENUMS
               "false",
               property is ElementComputedPropertyNode ? "true" : "false"

            );
        }
        foreach (var property in Ctx.Data.Collections)
        {
            Ctx._("list.Add(new ViewModelPropertyInfo({0}, {1}, {2}, {3}))",
               property.Name.AsField(),
               property.RelatedNode() is ElementNode ? "true" : "false",
               "true",
               "false", // TODO FOR ENUMS
               "false"
            );
        }
    }

    #endregion

    [TemplateMethod(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "Get{0}Dependents")]
    public virtual IEnumerable<IObservableProperty> ComputedDependents()
    {
        var computed = Ctx.ItemAs<ElementComputedPropertyNode>();
        foreach (var item in computed.InputsFrom<PropertyChildItem>())
        {
           

            var relatedNode = item.RelatedTypeNode;
            if (relatedNode != null)
            {
                var conditionStatements = Ctx._if("{0}.Value != null", item.Name.AsSubscribableProperty())
                    .TrueStatements;
                foreach (var p in computed.Properties.Where(p => p.SourceItem.Node == relatedNode))
                {
                    conditionStatements._("yield return {0}.Value.{1}", item.Name.AsSubscribableProperty(), p.Name.AsSubscribableProperty());
                }
            }
            else
            {
                Ctx._("yield return {0}", item.Name.AsSubscribableField());
            }
            

        }
       

        Ctx._("yield break");

        yield break;
    }
    [TemplateMethod(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "Reset{0}")]
    public virtual void ResetComputed()
    {
        var computed = Ctx.ItemAs<ElementComputedPropertyNode>();
        Ctx._if("_{0}Disposable != null", computed.Name)
            .TrueStatements._("_{0}Disposable.Dispose()",computed.Name);
        Ctx._("_{0}Disposable = {1}.ToComputed(Compute{0}, this.Get{0}Dependents().ToArray()).DisposeWith(this)",computed.Name, computed.Name.AsSubscribableField());
        
    }

    [TemplateMethod(MemberGeneratorLocation.Both, AutoFill = AutoFillType.NameOnly, NameFormat = "Compute{0}")]
    public virtual Boolean Compute()
    {
        var type = Ctx.ItemAs<ElementComputedPropertyNode>().RelatedTypeName;
        Ctx.SetType(type);
        Ctx._("return default({0})", type);
        return false;
    }

    public static Dictionary<Type, string> AcceptableTypes = new Dictionary<Type, string>
    {
        {typeof(int),"Int" },
        {typeof(Vector3),"Vector3" },
        {typeof(Vector2),"Vector2" },
        {typeof(string),"String" },
        {typeof(bool),"Bool" },
        {typeof(float),"Float" },
        {typeof(double),"Double" },
        {typeof(Quaternion),"Quaternion" },
    };

    private P<float> _viewModelProperty;
}