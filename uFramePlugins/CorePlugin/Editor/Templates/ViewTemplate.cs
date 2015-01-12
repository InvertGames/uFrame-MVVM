using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Invert.Core.GraphDesigner;
using Invert.uFrame.Editor;
using uFrame.Graphs;
using UnityEngine;

[TemplateClass("Views",uFrameFormats.VIEW_FORMAT, MemberGeneratorLocation.Both)]
public class ViewTemplate : ViewBase , IClassTemplate<ElementViewNode>
{
    public void TemplateSetup()
    {
        foreach (var item in Ctx.Data.Element.Properties.Where(p => p.RelatedTypeName != null))
        {
            var field = Ctx.CurrentDecleration._private_(item.RelatedTypeName, item.Name.AsField());
            field.Comments.Add(new CodeCommentStatement(item.RelatedTypeName));
            field.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof (UFGroup)),
                new CodeAttributeArgument(new CodePrimitiveExpression("View Model Properties"))));
            field.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof (HideInInspector))));
        }
        Ctx.AddIterator("ExecuteCommand", _ => _.Element.Commands.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)));
        Ctx.AddIterator("ExecuteCommandWithArg", _ => _.Element.Commands.Where(p => !string.IsNullOrEmpty(p.RelatedTypeName)));
    }

    public TemplateContext<ElementViewNode> Ctx { get; set; }


    [TemplateProperty]
    public override Type ViewModelType
    {
        get
        {
            Ctx._("return typeof({0})",Ctx.Data.Element.Name.AsViewModel());
            return null;
        }
    }

    [TemplateProperty]
    public  Type ViewModelProperty
    {
        get
        {
            Ctx.CurrentProperty.Name = Ctx.Data.Element.Name;
            Ctx.CurrentProperty.Type = Ctx.Data.Element.Name.AsViewModel().ToCodeReference();
            Ctx._("return ({0})ViewModelObject", Ctx.Data.Element.Name.AsViewModel());
            return null;
        }
    }

    [TemplateMethod(CallBase = false)]
    public override ViewModel CreateModel()
    {
        //var property = Context.Get<IDiagramNodeItem>();
        Ctx._("return this.RequestViewModel(GameManager.Container.Resolve<{0}>())",Ctx.Data.Element.Name.AsController());

        return null;
    }

    protected override void InitializeViewModel(ViewModel model)
    {

    }

    [TemplateMethod(MemberGeneratorLocation.Both, true)]
    public override void Bind()
    {
        if (!Ctx.IsDesignerFile) return;
        // Let the Dll Know about uFrame Binding Specific Types
        uFrameBindingType.ObservablePropertyType = typeof (IObservableProperty);
        uFrameBindingType.UFGroupType = typeof (UFGroup);
        // For each binding lets do some magic
        foreach (var item in Ctx.Data.Bindings)
        {
            // Cast the source of our binding (ie: Property, Collection, Command..etc)
            var source = item.SourceItem as ITypedItem;
            // Create a boolean field for each property that has a binding this will serve the condition
            // in the bind method to turn the binding on or off.
            var bindingField = Ctx.CurrentDecleration._public_(typeof (bool), "_Bind{0}", source.Name);
            // Add a toggle group attribute to it, this hides and shows anything within the same group
            bindingField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(UFToggleGroup)),
              new CodeAttributeArgument(new CodePrimitiveExpression(source.Name))));
            // Hide them in the insepctor, our custom 'ViewInspector' class will handle them manually
            bindingField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(HideInInspector))));
            // Create the binding condition
            var bindingCondition = Ctx._if("{0}", bindingField.Name);
            // Grab the uFrame Binding Type
            var bindingType = item.BindingType;
            // Create the binding signature based on the Method Info
            var bindingStatement = bindingType.CreateBindingSignature(
                Ctx.CurrentDecleration, 
                _ => source.RelatedTypeName.ToCodeReference(), 
                Ctx.Data, source);
            // Add the binding statement to the condition
            bindingCondition.TrueStatements.Add(bindingStatement);
            // Add the bindingCondition to this method
            Ctx.CurrentStatements.Add(bindingCondition);
        }
        //var bindingStatement = ViewBindingExtensions.CreateBindingSignature(Ctx.CurrentDecleration,
        //    typeof (ViewBindings).GetMethods(BindingFlags.Public | BindingFlags.Static)
        //        .Where(p => p.Name == "BindProperty")
        //        .FirstOrDefault(),_=>typeof(string));
        //Ctx.CurrentStatements.Add(bindingStatement);
    }

    [TemplateMethod("Execute{0}", MemberGeneratorLocation.DesignerFile,false,AutoFill = AutoFillType.NameOnly)]
    public void ExecuteCommand()
    {
        Ctx._("this.ExecuteCommand({0}.{1})", Ctx.Data.Name, Ctx.Item.Name);
    }

    [TemplateMethod("Execute{0}", MemberGeneratorLocation.DesignerFile, false, AutoFill = AutoFillType.NameOnly)]
    public void ExecuteCommandWithArg(object arg)
    {
        Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);

        Ctx._("this.ExecuteCommand({0}.{1}, arg)",Ctx.Data.Name,Ctx.Item.Name);
    }
}
public static class ViewBindingExtensions
{
}
//public interface IElementViewBinding
//{
//    void CreateBinding(TemplateContext<ElementViewNode> ctx, ViewBindingsReference bindingInfo);
//}

//public class ElementViewBinding : IElementViewBinding
//{
//    public TemplateContext<ElementViewNode> Ctx { get; set; }
//    public ViewBindingsReference Info { get; set; }
//    public virtual void CreateBinding(TemplateContext<ElementViewNode> ctx, ViewBindingsReference bindingInfo)
//    {
//        Ctx = ctx;
//        Info = bindingInfo;

//    }
//}
//public class PropertyBinding : ElementViewBinding
//{
//    public override void CreateBinding(TemplateContext<ElementViewNode> ctx, ViewBindingsReference bindingInfo)
//    {
//        base.CreateBinding(ctx, bindingInfo);
//        ctx.RenderTemplateMethod(this, "PropertyChanged");
//    }

//    [TemplateMethod("")]
//    public void PropertyChanged(object value)
//    {
//        Ctx.Item.Name;
//    }
//}
