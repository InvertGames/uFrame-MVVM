using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEngine;

[TemplateClass( MemberGeneratorLocation.Both, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT)]
public partial class ViewModelTemplate : ViewModel, IClassTemplate<ElementNode>
{
    #region Template Stuff
    public TemplateContext<ElementNode> Ctx { get; set; }

    public string OutputPath
    {
        get { return Path2.Combine(Ctx.Data.Graph.Name, "ViewModels"); }
    }

    public bool CanGenerate
    {
        get { return true; }
    }

    public void TemplateSetup()
    {
        // Ensure the namespaces for each property type are property set up
        Ctx.CurrentDecleration.IsPartial = true;
        Ctx.TryAddNamespace("UnityEngine");
        Ctx.TryAddNamespace("UniRx");
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
        Ctx.AddIterator("CollectionProperty", (d) => d.LocalCollections);
        Ctx.AddIterator("CommandItems", (d) => d.LocalCommands);
        Ctx.AddIterator("CommandMethod", (d) => d.LocalCommands);
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


    [TemplateConstructor(MemberGeneratorLocation.DesignerFile, "aggregator")]
    public void AggregatorConstructor(IEventAggregator aggregator)
    {
       
    }
    //[TemplateConstructor(MemberGeneratorLocation.Both, "controller", "initialize")]
    //public void ControllerConstructor(Controller controller, bool initialize)
    //{
    //    Ctx.CurrentConstructor.Parameters[0].Type = (Ctx.Data.Name.AsController() + "Base").ToCodeReference();
    //    Ctx.CurrentConstructor.Parameters[1].Name = "initialize = true";
    //}

    [TemplateMethod(MemberGeneratorLocation.Both, true)]
    public override void Bind()
    {
        if (!Ctx.IsDesignerFile) return;
        foreach (var command in Ctx.Data.Commands)
        {
            Ctx._("this.{0} = new Signal<{0}Command>(this, this.Aggregator)",command.Name);
        }
        foreach (var property in ViewModelProperties)
        {
            Ctx._("{0} = new P<{1}>(this, \"{2}\")", property.Name.AsSubscribableField(), property.RelatedTypeName,property.Name);
        }
        // No more parents so no need to bind to the collection change, this was bad anyways
        //foreach (var property in Ctx.Data.LocalCollections)
        //{
        //    Ctx._("{0} = new ModelCollection<{1}>(this, \"{2}\")", property.Name.AsField(), property.RelatedTypeName,property.Name);
        //    Ctx._("{0}.CollectionChanged += {1}CollectionChanged", property.Name.AsField(), property.Name);
        //}

        foreach (var item in StateMachineProperties)
        {
            Ctx._("{0} = new {1}(this, \"{2}\")", item.Name.AsSubscribableField(), item.RelatedTypeName,item.Name);
        } 
        foreach (var item in Ctx.Data.ComputedProperties)
        {
            Ctx._("Reset{0}()", item.Name);
        }
        //_StateMachineProperty.B.AddComputer(IsCorrectPasswordProperty);
        foreach (var item in Ctx.Data.ComputedProperties)
        {
            
            var transition = item.OutputTo<TransitionsChildItem>();
            if (transition == null) continue;
            var stateMachineNode = transition.Node as IClassTypeNode;
            var property = stateMachineNode.ReferencesOf<PropertiesChildItem>().FirstOrDefault(p => p.Node == Ctx.Data);
            if (property == null) continue;
            Ctx._("{0}.{1}.AddComputer({2})", property.Name.AsSubscribableProperty(), transition.Name, item.Name.AsSubscribableProperty());

        }
        foreach (var item in Ctx.Data.LocalCommands)
        {
            //Transition.Subscribe(_ => StateMachineProperty.B.OnNext(true));
            var transition = item.OutputTo<TransitionsChildItem>();
            if (transition == null) continue;
            var stateMachineNode = transition.Node as IClassTypeNode;
            var property = stateMachineNode.ReferencesOf<PropertiesChildItem>().FirstOrDefault(p => p.Node == Ctx.Data);
            if (property == null) continue;
            Ctx._("{0}.Subscribe(_ => {1}.{2}.OnNext(true))", item.Name, property.Name.AsSubscribableProperty(), transition.Name);

        }
       

    }

    //[TemplateMethod(MemberGeneratorLocation.DesignerFile)]
    protected override void WireCommands(Controller controller)
    {
        //base.WireCommands(controller);
        if (!Ctx.Data.Commands.Any()) return;
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
        get { return null; }
     
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
    public virtual object CommandItems {
        get
        {
            Ctx.SetType("Signal<{0}Command>",Ctx.Item.Name);
            return null;
        }
        set
        {
            
        }
    }
    #endregion

    #region Serialization
    [TemplateMethod]
    public override void Read(ISerializerStream stream)
    {
        foreach (var viewModelPropertyData in Ctx.Data.LocalProperties)
        {

            var relatedNode = viewModelPropertyData.RelatedTypeNode;
            if (relatedNode is EnumNode)
            {
                Ctx._("this.{0} = ({1})stream.DeserializeInt(\"{0}\");", viewModelPropertyData.Name, viewModelPropertyData.RelatedTypeName);
            }
            else if (relatedNode is ElementNode)
            {
                var elementNode = relatedNode as ElementNode;
                Ctx._("\t\tif (stream.DeepSerialize) this.{0} = stream.DeserializeObject<{1}>(\"{0}\");", viewModelPropertyData.Name, elementNode.Name.AsViewModel());
                
            }
            else if (relatedNode is StateMachineNode)
            {
                Ctx._("this.{0}.SetState(stream.DeserializeString(\"{1}\"))", viewModelPropertyData.FieldName, viewModelPropertyData.Name);
            }
            else
            {
                if (viewModelPropertyData.Type == null) continue;
                if (!AcceptableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                Ctx._("this.{0} = stream.Deserialize{1}(\"{0}\");", viewModelPropertyData.Name, AcceptableTypes[viewModelPropertyData.Type]);
            }
        }
        foreach (var collection in Ctx.Data.LocalCollections)
        {
            var relatedNode = collection.RelatedTypeNode;
            if (relatedNode is EnumNode)
            {
                //var statement = new CodeSnippetStatement(string.Format("\t\tstream.SerializeInt(\"{0}\", (int)this.{0});", viewModelPropertyData.Name));
                //writeMethod.Statements.Add(statement);

                //var dstatement = new CodeSnippetStatement(string.Format("\t\tthis.{0} = ({1})stream.DeserializeInt(\"{0}\");", viewModelPropertyData.Name, viewModelPropertyData.RelatedTypeName));
                //readMethod.Statements.Add(dstatement);
            }
            else if (relatedNode is ElementNode)
            {
                var elementNode = relatedNode as ElementNode;

                Ctx.PushStatements(Ctx._if("stream.DeepSerialize").TrueStatements);
                Ctx._("this.{0}.Clear()", collection.Name);
                Ctx._("this.{0}.AddRange(stream.DeserializeObjectArray<{1}>(\"{0}\"))", collection.Name, elementNode.Name.AsViewModel());
                Ctx.PopStatements();
            }
            else
            {
                //if (collection.Type == null) continue;
                //if (!AcceptableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                //viewModelPropertyData.IsEnum(data.OwnerData);
                //var statement = new CodeSnippetStatement(string.Format("\t\tstream.Serialize{0}(\"{1}\", this.{1});", AcceptableTypes[viewModelPropertyData.Type], viewModelPropertyData.Name));
                //writeMethod.Statements.Add(statement);

                //var dstatement = new CodeSnippetStatement(string.Format("\t\tthis.{0} = stream.Deserialize{1}(\"{0}\");", viewModelPropertyData.Name, AcceptableTypes[viewModelPropertyData.Type]));
                //readMethod.Statements.Add(dstatement);
            }
        }
    }

    [TemplateMethod]
    public override void Write(ISerializerStream stream)
    {
        foreach (var viewModelPropertyData in Ctx.Data.LocalProperties)
        {

            var relatedNode = viewModelPropertyData.RelatedTypeNode;
            if (relatedNode is EnumNode)
            {
                Ctx._("stream.SerializeInt(\"{0}\", (int)this.{0});", viewModelPropertyData.Name);

            }
            else if (relatedNode is ElementNode)
            {
                Ctx._("if (stream.DeepSerialize) stream.SerializeObject(\"{0}\", this.{0});", viewModelPropertyData.Name);
            }
            else if (relatedNode is StateMachineNode)
            {

                Ctx._("stream.SerializeString(\"{0}\", this.{0}.Name);", viewModelPropertyData.Name);
            }
            else
            {
                if (viewModelPropertyData.Type == null) continue;
                if (!AcceptableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                Ctx._("stream.Serialize{0}(\"{1}\", this.{1})", AcceptableTypes[viewModelPropertyData.Type], viewModelPropertyData.Name);
            }
        }
        foreach (var collection in Ctx.Data.LocalCollections)
        {
            var relatedNode = collection.RelatedTypeNode;
            if (relatedNode is EnumNode)
            {
                //var statement = new CodeSnippetStatement(string.Format("\t\tstream.SerializeInt(\"{0}\", (int)this.{0});", viewModelPropertyData.Name));
                //writeMethod.Statements.Add(statement);

                //var dstatement = new CodeSnippetStatement(string.Format("\t\tthis.{0} = ({1})stream.DeserializeInt(\"{0}\");", viewModelPropertyData.Name, viewModelPropertyData.RelatedTypeName));
                //readMethod.Statements.Add(dstatement);
            }
            else if (relatedNode is ElementNode)
            {
                Ctx._("if (stream.DeepSerialize) stream.SerializeArray(\"{0}\", this.{0})",
                            collection.Name);
            }
            else
            {
                //if (collection.Type == null) continue;
                //if (!AcceptableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                //viewModelPropertyData.IsEnum(data.OwnerData);
                //var statement = new CodeSnippetStatement(string.Format("\t\tstream.Serialize{0}(\"{1}\", this.{1});", AcceptableTypes[viewModelPropertyData.Type], viewModelPropertyData.Name));
                //writeMethod.Statements.Add(statement);

                //var dstatement = new CodeSnippetStatement(string.Format("\t\tthis.{0} = stream.Deserialize{1}(\"{0}\");", viewModelPropertyData.Name, AcceptableTypes[viewModelPropertyData.Type]));
                //readMethod.Statements.Add(dstatement);
            }
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
            Ctx._comment(property.GetType().Name);
            Ctx._("list.Add(new ViewModelPropertyInfo({0}, {1}, {2}, {3}, {4}))",
               property.Name.AsSubscribableField(),
               property.RelatedNode() is ElementNode ? "true" : "false",
               "false",
               property.RelatedNode() is EnumNode ? "true" : "false", // TODO FOR ENUMS
               property is ComputedPropertyNode ? "true" : "false"

            );
        }
        foreach (var property in Ctx.Data.LocalCollections)
        {
            Ctx._("list.Add(new ViewModelPropertyInfo({0}, {1}, {2}, {3}, {4}))",
               property.Name.AsField(),
               property.RelatedNode() is ElementNode ? "true" : "false",
               "true",
               property.RelatedNode() is EnumNode ? "true" : "false", // TODO FOR ENUMS
               "false"
            );
        }
    }

    #endregion

    [TemplateMethod(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "Get{0}Dependents")]
    public virtual IEnumerable<IObservableProperty> ComputedDependents()
    {
        var computed = Ctx.ItemAs<ComputedPropertyNode>();
        foreach (var item in computed.InputsFrom<PropertiesChildItem>())
        {
           

            var relatedNode = item.RelatedTypeNode;
            if (relatedNode != null)
            {
                var conditionStatements = Ctx._if("{0}.Value != null", item.Name.AsSubscribableProperty())
                    .TrueStatements;
                foreach (var p in computed.SubProperties.Where(p => p.SourceItem.Node == relatedNode))
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
        var computed = Ctx.ItemAs<ComputedPropertyNode>();
        Ctx._if("_{0}Disposable != null", computed.Name)
            .TrueStatements._("_{0}Disposable.Dispose()",computed.Name);
        Ctx._("_{0}Disposable = {1}.ToComputed(Compute{0}, this.Get{0}Dependents().ToArray()).DisposeWith(this)",computed.Name, computed.Name.AsSubscribableField());
        
    }

    [TemplateMethod(MemberGeneratorLocation.Both, AutoFill = AutoFillType.NameOnly, NameFormat = "Compute{0}")]
    public virtual Boolean Compute()
    {
        var type = Ctx.ItemAs<ComputedPropertyNode>().RelatedTypeName;
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

}
[TemplateClass(MemberGeneratorLocation.DesignerFile, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT)]
public partial class ViewModelConstructorTemplate : IClassTemplate<ElementNode>
{
    public TemplateContext<ElementNode> Ctx { get; set; }

    public string OutputPath
    {
        get { return Path2.Combine(Ctx.Data.Graph.Name, "ViewModels"); }
    }
    [TemplateConstructor(MemberGeneratorLocation.Both, "aggregator")]
    public void AggregatorConstructor(IEventAggregator aggregator)
    {

    }
    public bool CanGenerate
    {
        get { return true; }
    }

    public void TemplateSetup()
    {
        // Ensure the namespaces for each property type are property set up
        Ctx.CurrentDecleration.BaseTypes.Clear();
        Ctx.CurrentDecleration.IsPartial = true;
        Ctx.CurrentDecleration.Name = string.Format(uFrameFormats.VIEW_MODEL_FORMAT,Ctx.Data.Name);
    }

}
[TemplateClass(MemberGeneratorLocation.DesignerFile, ClassNameFormat = "{0}Command")]
public class ViewModelCommandClassTemplate : ViewModelCommand, IClassTemplate<CommandsChildItem>
{
    public string OutputPath
    {
        get
        {
            return Path2.Combine(Ctx.Data.Node.Graph.Name, "Commands");
        }
    }

    public bool CanGenerate
    {
        get { return Ctx.Data.OutputCommand == null; }
    }

    public void TemplateSetup()
    {
        
        Ctx.CurrentDecleration.IsPartial = true;
        Ctx.CurrentDecleration.Name = Ctx.Data.Name + "Command";
        Ctx.AddCondition("Argument",_=>!string.IsNullOrEmpty(_.RelatedType));
    }

    public TemplateContext<CommandsChildItem> Ctx { get; set; }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile,AutoFillType.NameOnlyWithBackingField)]
    public object Argument {
        get
        {
            Ctx.SetType(Ctx.Data.RelatedTypeName);
            Ctx.CurrentProperty.Name = "Argument";
            return null;
        }
        set
        {
            
        }
    }

}
[TemplateClass(Location = MemberGeneratorLocation.DesignerFile, AutoInherit = true, ClassNameFormat = "{0}Command")]
public class CommandClassTemplateBase : IClassTemplate<CommandNode>
{

    private Invert.Core.GraphDesigner.TemplateContext<CommandNode> _Ctx;

    public virtual string OutputPath
    {
        get
        {
            return "";
        }
    }

    public virtual bool CanGenerate
    {
        get
        {
            return true;
        }
    }

    public Invert.Core.GraphDesigner.TemplateContext<CommandNode> Ctx
    {
        get
        {
            return _Ctx;
        }
        set
        {
            _Ctx = value;
        }
    }

    public virtual void TemplateSetup()
    {
        if (Ctx.IsDesignerFile)
        {
            Ctx.CurrentDecleration.BaseTypes.Clear();
            Ctx.CurrentDecleration.BaseTypes.Add(new CodeTypeReference("ViewModelCommand"));
        }
    }
}
public class CommandClassTemplate : CommandClassTemplateBase
{
    public override string OutputPath
    {
        get
        {
            return Path2.Combine(Ctx.Data.Node.Graph.Name, "Commands");
        }
    }
    public override void TemplateSetup()
    {
        base.TemplateSetup();
        Ctx.AddIterator("Property", node => node.Properties);
        Ctx.AddIterator("Collection", node => node.Collections);
    }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndTypeWithBackingField)]
    public object Property
    {
        get;
        set;
    }
    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndTypeWithBackingField)]
    public List<object> Collection
    {
        get;
        set;
    }
}
