using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.uFrame.Editor;
using uFrame.Graphs;
using UnityEngine;

[TemplateClass("ViewModels", MemberGeneratorLocation.Both, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT)]
public class ViewModelTemplate : ViewModel, IClassTemplate<ElementNode>
{
    #region Template Stuff
    public TemplateContext<ElementNode> Ctx { get; set; }

    public void TemplateSetup()
    {
        Ctx.AddIterator("ResetComputed", (d) => d.ComputedProperties);
        Ctx.AddIterator("Compute", (d) => d.ComputedProperties);
        Ctx.AddIterator("ComputedDependents", (d) => d.ComputedProperties);
        Ctx.AddIterator("ViewModelProperty", (d) => d.AllProperties);
        Ctx.AddIterator("ViewModelValueProperty", (d) => d.AllProperties);
        Ctx.AddIterator("CollectionProperty", (d) => d.Collections);
        Ctx.AddIterator("CommandItems", (d) => d.Commands);
        Ctx.AddIterator("CommandMethod", (d) => d.Commands);
        Ctx.AddIterator("CollectionChanged", (d) => d.Collections.Where(p => p.RelatedTypeName != null));

        foreach (var item in Ctx.Data.ComputedProperties)
        {
            Ctx.CurrentDecleration._private_(typeof(IDisposable), "_{0}Disposable", item.Name);
        }
    }
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
        foreach (var property in Ctx.Data.AllProperties.Where(p => p.RelatedTypeName != null))
        {
            Ctx._("{0} = new P<{1}>(this, \"{0}\")", property.Name.AsSubscribableField(), property.RelatedTypeName);
        }

        foreach (var property in Ctx.Data.Collections.Where(p => p.RelatedTypeName != null))
        {
            Ctx._("{0} = new ModelCollection<{1}>(this, \"{0}\")", property.Name.AsField(), property.RelatedTypeName);
            Ctx._("{0}.CollectionChanged += {1}CollectionChanged", property.Name.AsField(), property.Name);
        }
        foreach (var item in Ctx.Data.ComputedProperties)
        {
            Ctx._("Reset{0}()", item.Name);
        }

    }

    #region Properties
    [TemplateProperty(uFrameFormats.SUBSCRIBABLE_PROPERTY_FORMAT, AutoFillType.NameAndTypeWithBackingField)]
    public virtual P<float> ViewModelProperty { get; set; }

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
    [TemplateProperty("{0}Command", AutoFillType.NameOnlyWithBackingField)]
    public virtual ICommand CommandItems { get; set; }
    #endregion

    #region Serialization
    [TemplateMethod]
    public override void Read(ISerializerStream stream)
    {
        foreach (var property in Ctx.Data.Properties.Where(x => x.RelatedTypeName != null && x.Type != null && AcceptableTypes.ContainsKey(x.Type)))
        {
            Ctx._("stream.Deserialize{0}(\"{1}\", this.{1})", AcceptableTypes[property.Type], property.Name);
        }
        foreach (var property in Ctx.Data.Collections.Where(x => x.RelatedTypeName != null && x.Type != null && AcceptableTypes.ContainsKey(x.Type)))
        {
            Ctx._("stream.DeserializeArray<{0}>(\"{1}\", this.{1})", AcceptableTypes[property.Type], property.Name);
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


    protected override void FillCommands(List<ViewModelCommandInfo> list)
    {
        base.FillCommands(list);

    }

    protected override void FillProperties(List<ViewModelPropertyInfo> list)
    {
        base.FillProperties(list);
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

        // if (_PlayerDiedDisposable != null) _PlayerDiedDisposable.Dispose();
        // _PlayerDiedDisposable = _PlayerDiedProperty.ToComputed(ComputePlayerDied, this.GetPlayerDiedDependents().ToArray()).DisposeWith(this);
    }

    [TemplateMethod(MemberGeneratorLocation.Both, AutoFill = AutoFillType.NameOnly, NameFormat = "Compute{0}")]
    public virtual Boolean Compute()
    {
        Debug.Log("TEMPLATE COMPUTED");
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

}