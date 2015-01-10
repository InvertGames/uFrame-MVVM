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
            var field = Ctx.CurrentDecleration._private_(item.Type, item.Name.AsField());
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
        //foreach (var property in this.viewp)
        foreach (var item in Ctx.Data.Bindings)
        {
            
            
            var source = item.SourceItem as ITypedItem;
            var bindingField = Ctx.CurrentDecleration._public_(typeof (bool), "_Bind{0}", source.Name);
            bindingField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(UFToggleGroup)),
              new CodeAttributeArgument(new CodePrimitiveExpression(source.Name))));
            bindingField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(HideInInspector))));
            var bindingCondition = Ctx._if("{0}", bindingField.Name);
            var bindingType = item.BindingType;
            var bindingStatement = ViewBindingExtensions.CreateBindingSignature(Ctx.CurrentDecleration, bindingType.MethodInfo, _ => source.RelatedTypeName.ToCodeReference(), Ctx.Data.Element.Name, source.Name);
            bindingCondition.TrueStatements.Add(bindingStatement);
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
    public static CodeExpression CreateBindingSignature(CodeTypeDeclaration context, MethodInfo info, Func<Type, CodeTypeReference> convertGenericParameter, string vmName, string propertyName)
    {
        var methodInvoke = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), info.Name);
        var isExtensionMethod = info.IsDefined(typeof (ExtensionAttribute), true);

        for (int index = 0; index < info.GetParameters().Length; index++)
        {
            var parameter = info.GetParameters()[index];
            if (isExtensionMethod && index == 0) continue;

            var genericArguments = parameter.ParameterType.GetGenericArguments();
            if (typeof (Delegate).IsAssignableFrom(parameter.ParameterType))
            {
                var method = CreateDelegateMethod(convertGenericParameter, parameter, genericArguments, propertyName);

                methodInvoke.Parameters.Add(new CodeSnippetExpression(string.Format("this.{0}", method.Name)));
                context.Members.Add(method);
            }
            else if (typeof(IObservableProperty).IsAssignableFrom(parameter.ParameterType))
            {
                methodInvoke.Parameters.Add(new CodeSnippetExpression(string.Format("this.{0}.{1}", vmName, propertyName)));
            }
            else
            {
                var field = context._private_(parameter.ParameterType, "_{0}{1}", propertyName, parameter.Name);
                field.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(UFGroup)),
              new CodeAttributeArgument(new CodePrimitiveExpression(propertyName))));
                field.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof (SerializeField))));
                methodInvoke.Parameters.Add(new CodeSnippetExpression(field.Name));
            }
        }
        return methodInvoke;
    }

    private static CodeMemberMethod CreateDelegateMethod(Func<Type, CodeTypeReference> convertGenericParameter, ParameterInfo parameter,
        Type[] genericArguments,string propertyName)
    {
        var method = new CodeMemberMethod()
        {
            Name = string.Format("{0}{1}{2}",propertyName, parameter.Name.Substring(0,1).ToUpper(),parameter.Name.Substring(1)),
            Attributes = MemberAttributes.Public
        };
        var isFunc = parameter.ParameterType.Name.Contains("Func");
        if (isFunc)
        {
            var returnType = genericArguments.LastOrDefault();
            if (returnType != null)
            {
                method.ReturnType = new CodeTypeReference(returnType);
            }
        }
        var index = 1;
        foreach (var item in genericArguments)
        {
            if (isFunc && item == genericArguments.Last()) continue;
            var type = item;
            if (item.IsGenericParameter)
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(convertGenericParameter(item), string.Format("arg{0}", index)));
            }
            else
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(type, string.Format("arg{0}", index)));
            }
            
        }
        return method;
    }

    public static void CreateActionSignature(Type actionType)
    {
        
    }
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
