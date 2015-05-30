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

[TemplateClass(MemberGeneratorLocation.Both, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT)]
public partial class ViewModelTemplate : ViewModel, IClassTemplate<ElementNode>, IClassRefactorable
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

        foreach (var property in Ctx.Data.PersistedItems.OfType<ITypedItem>())
        {
            var type = InvertApplication.FindTypeByName(property.RelatedTypeName);
            if (type == null) continue;

            Ctx.TryAddNamespace(type.Namespace);
        }

        StateMachineProperties = Ctx.Data.AllProperties.Where(p => (p.RelatedNode() is StateMachineNode)).ToArray();
        ViewModelProperties = Ctx.Data.AllProperties.Where(p => !StateMachineProperties.Contains(p)).ToArray();

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

    [TemplateMethod]
    public override void Bind()
    {
        if (!Ctx.IsDesignerFile) return;
        foreach (var command in Ctx.Data.LocalCommands)
        {
            Ctx._("this.{0} = new Signal<{0}Command>(this)", command.Name);
        }
        foreach (var property in ViewModelProperties)
        {
            Ctx._("{0} = new P<{1}>(this, \"{2}\")", property.Name.AsSubscribableField(), property.RelatedTypeName, property.Name);
        }
        // No more parents so no need to bind to the collection change, this was bad anyways
        foreach (var property in Ctx.Data.LocalCollections)
        {
            Ctx._("{0} = new ModelCollection<{1}>(this, \"{2}\")", property.Name.AsField(), property.RelatedTypeName, property.Name);
            //   Ctx._("{0}.CollectionChanged += {1}CollectionChanged", property.Name.AsField(), property.Name);
        }

        foreach (var item in StateMachineProperties)
        {
            Ctx._("{0} = new {1}(this, \"{2}\")", item.Name.AsSubscribableField(), item.RelatedTypeName, item.Name);
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


    #region Properties

    //[TemplateProperty(uFrameFormats.SUBSCRIBABLE_PROPERTY_FORMAT, AutoFillType.NameAndTypeWithBackingField)]
    [ForEach("StateMachineProperties"),TemplateProperty, WithField]
    public virtual _ITEMTYPE_ _Name2_Property
    {
        get
        {
            return null;
        }
    }

    //[TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameOnly)]
    [ForEach("StateMachineProperties"),TemplateProperty]
    public virtual State _Name2_
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

    //[TemplateProperty(uFrameFormats.SUBSCRIBABLE_PROPERTY_FORMAT, AutoFillType.NameAndTypeWithBackingField)]
    [ForEach("ViewModelProperties"), TemplateProperty, WithField]
    public virtual P<_ITEMTYPE_> _Name_Property
    {
        get { return null; }

    }

    [ForEach("ViewModelProperties"), TemplateProperty]
    public virtual _ITEMTYPE_ _Name_
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

    #region Collections
    //[TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndTypeWithBackingField)]
    [ForEach("LocalCollections"), TemplateProperty, WithField]
    public virtual ModelCollection<_ITEMTYPE_> _Name4_
    {
        get { return null; }
    }

    #endregion

    #region Commands
   // [TemplateProperty("{0}", AutoFillType.NameOnlyWithBackingField)]
    [ForEach("LocalCommands"), TemplateProperty, WithField]
    public virtual object _Name3_
    {
        get
        {
            Ctx.SetType("Signal<{0}Command>", Ctx.Item.Name);
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

    [TemplateMethod]
    protected override void FillCommands(List<ViewModelCommandInfo> list)
    {
        //base.FillCommands(list);
        foreach (var commandChildItem in Ctx.Data.LocalCommands)
        {
            Ctx._("list.Add(new ViewModelCommandInfo(\"{0}\", {0}) {{ ParameterType = typeof({1}) }})",
               commandChildItem.Name,
               string.IsNullOrEmpty(commandChildItem.RelatedTypeName) ? "void" : commandChildItem.RelatedTypeName
            );
        }

    }


    [TemplateMethod]
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


   // [TemplateMethod(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "Get{0}Dependents")]
    [ForEach("ComputedProperties"), TemplateMethod]
    public virtual IEnumerable<IObservableProperty> Get_Name_Dependents()
    {
        var computed = Ctx.ItemAs<ComputedPropertyNode>();
        foreach (var item in computed.InputsFrom<PropertiesChildItem>())
        {

            Ctx._("yield return {0}", item.Name.AsSubscribableProperty());
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
        }
        foreach (var item in computed.InputsFrom<ComputedPropertyNode>())
        {
            Ctx._("yield return {0}", item.Name.AsSubscribableField());
        }


        Ctx._("yield break");

        yield break;
    }
    
    //[TemplateMethod(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "Reset{0}")]
    [ForEach("ComputedProperties"),TemplateMethod]
    public virtual void Reset_Name_()
    {
        var computed = Ctx.ItemAs<ComputedPropertyNode>();
        Ctx._if("_{0}Disposable != null", computed.Name)
            .TrueStatements._("_{0}Disposable.Dispose()", computed.Name);
        Ctx._("_{0}Disposable = {1}.ToComputed(Compute{0}, this.Get{0}Dependents().ToArray()).DisposeWith(this)", computed.Name, computed.Name.AsSubscribableField());

    }

    //[TemplateMethod(MemberGeneratorLocation.Both, AutoFill = AutoFillType.NameOnly, NameFormat = "Compute{0}")]
    [ForEach("ComputedProperties"), TemplateMethod]
    public virtual Boolean Compute_Name_()
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

    public IEnumerable<string> ClassNameFormats
    {
        get
        {
            yield return "{0}ViewModel";
            yield return "{0}ViewModelBase";
        }
    }
}

[TemplateClass(MemberGeneratorLocation.DesignerFile, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT)]
public partial class ViewModelConstructorTemplate : IClassTemplate<ElementNode>
{
    public TemplateContext<ElementNode> Ctx { get; set; }

    public string OutputPath
    {
        get { return Path2.Combine(Ctx.Data.Graph.Name, "ViewModels"); }
    }
    [TemplateConstructor("aggregator")]
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
        Ctx.CurrentDecleration.Name = string.Format(uFrameFormats.VIEW_MODEL_FORMAT, Ctx.Data.Name);
    }

}

[TemplateClass(MemberGeneratorLocation.DesignerFile, ClassNameFormat = "{0}Command")]
public partial class ViewModelCommandClassTemplate : ViewModelCommand, IClassTemplate<CommandsChildItem>, IClassRefactorable
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

        var type = InvertApplication.FindTypeByName(Ctx.Data.RelatedTypeName);
        if (type != null)
        {
            Ctx.TryAddNamespace(type.Namespace);
        }
        else
        {
            type = InvertApplication.FindType(Ctx.Data.RelatedTypeName);
            if (type != null)
            Ctx.TryAddNamespace(type.Namespace);
        }
        Ctx.CurrentDecleration.IsPartial = true;
        Ctx.CurrentDecleration.Name = Ctx.Data.Name + "Command";
        Ctx.AddCondition("Argument", _ => !string.IsNullOrEmpty(_.RelatedType));
    }

    public TemplateContext<CommandsChildItem> Ctx { get; set; }

    public bool HasArgument
    {
        get { return !string.IsNullOrEmpty(Ctx.Data.RelatedType); }
    }

    [TemplateProperty, WithField, If("HasArgument")]
    public _ITEMTYPE_ Argument { get; set; }

    public IEnumerable<string> ClassNameFormats
    {
        get { yield return "{0}Command"; }
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

[TemplateClass(MemberGeneratorLocation.DesignerFile, ClassNameFormat = "{0}Command")]
public partial class CommandClassTemplate : CommandClassTemplateBase, IClassRefactorable
{
    public IEnumerable<string> ClassNameFormats
    {
        get
        {
            yield return "{0}Command";
        }
    }

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
        foreach (var property in Ctx.Data.PersistedItems.OfType<ITypedItem>())
        {
            var type = InvertApplication.FindTypeByName(property.RelatedTypeName);
            if (type == null) continue;

            Ctx.TryAddNamespace(type.Namespace);
        }

        Ctx.CurrentDecleration.IsPartial = true;
    }


    [ForEach("Collections"), TemplateProperty, WithField]
    public _ITEMTYPE_ _Name_
    {
        get;
        set;
    }

    [ForEach("Collections"), TemplateProperty, WithField]
    public List<_ITEMTYPE_> _CollectionName_
    {
        get;
        set;
    }
}

