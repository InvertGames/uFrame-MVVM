using System.CodeDom;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Invert.uFrame.Editor;

public class ViewGenerator : ViewClassGenerator
{
    public ViewData View
    {
        get;
        set;
    }

    public void CreateViewViewBase()
    {

    }

    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);

        //AddViewBase();

        Decleration =  new CodeTypeDeclaration(View.NameAsView) { IsPartial = true };
        if (View.ViewForElement != null)
        {
            if (View.ViewForElement.IsTemplate)
            {
                Decleration.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;
            }
        }
        else
        {
            Decleration.TypeAttributes = TypeAttributes.Public;
        }
        if (IsDesignerFile)
        {
            Decleration.BaseTypes.Add(View.NameAsViewViewBase);
        }
        else
        {
            var bindingGenerators = uFrameEditor.GetBindingGeneratorsFor(View.ViewForElement, isOverride: true, generateDefaultBindings: DiagramData.Settings.GenerateDefaultBindings).ToArray();

            foreach (var bindingGenerator in bindingGenerators)
            {
                if (View.BindingMethods.All(p => p.Name != bindingGenerator.MethodName) &&
                    View.NewBindings.All(p => p.MethodName != bindingGenerator.MethodName)) continue;

                //bindingGenerator.IsOverride = true;

                bindingGenerator.CreateMembers(Decleration.Members);


            }
        }
        Namespace.Types.Add(Decleration);

        //if (View.BaseNode == null)
        //{
        //    return;
        //}
        //if (IsDesignerFile)
        //{
        //    AddViewBase(View.ViewForElement as ElementData, View.NameAsViewViewBase, View.BaseViewName);
        //}
        //else
        //{
        //    AddView(View);
        //}
        //var baseView = View.BaseView;
        //if (baseView != null && IsDesignerFile)
        //{
        //    AddView(View);
        //    AddViewBase(View.ViewForElement as ElementData, View.NameAsViewViewBase, baseView.NameAsView);
        //}
        //else
        //{
        //    AddView(View);
        //    if (IsDesignerFile)
        //    {
        //        AddViewBase(View.ViewForElement as ElementData, View.NameAsViewViewBase, View.BaseViewName);    
        //    }

        //}
    }

    public void AddView(ViewData view)
    {
        var decl = new CodeTypeDeclaration(view.NameAsView) { IsPartial = true };
        if (view.ViewForElement != null)
        {
            if (view.ViewForElement.IsTemplate)
            {
                decl.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;
            }
        }
        else
        {
            decl.TypeAttributes = TypeAttributes.Public;
        }
        if (IsDesignerFile)
        {
            var viewViewBase = View.BaseView != null;
            decl.BaseTypes.Add(new CodeTypeReference(viewViewBase ? view.NameAsViewViewBase : view.BaseViewName));
            decl.Members.Add(CreateUpdateMethod(view, decl));
            GenerateBindMethod(decl, view);
        }
        else
        {
            foreach (var bindingGenerator in view.NewBindings)
            {
                bindingGenerator.CreateMembers(decl.Members);
            }
            //GenerateBindingMembers(decl,view.ViewForElement,true);
            var bindMethod = new CodeMemberMethod()
            {
                Name = "Bind",
                Attributes = MemberAttributes.Override | MemberAttributes.Public
            };
            decl.Members.Add(bindMethod);
            bindMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "Bind"));

        }
        Namespace.Types.Add(decl);
    }
}

public class ViewViewBaseGenerator : ViewClassGenerator
{
    public ViewData View { get; set; }
    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        var forElement = View.ViewForElement;

        Decleration = new CodeTypeDeclaration()
        {
            Name = View.NameAsViewViewBase,
            Attributes = MemberAttributes.Public | MemberAttributes.Abstract,
        };

        Decleration.BaseTypes.Add(View.BaseViewName);

        // implement abstract method bind defined in the viewbase class
        //var bindMethod = new CodeMemberMethod()
        //{
        //    Name = "Bind",
        //    Attributes = MemberAttributes.Override | MemberAttributes.Public
        //};
        //Decleration.Members.Add(bindMethod);

        AddBindingMembers();

        GenerateBindMethod(Decleration, View);
        // Make sure we only generate a view model property for whats needed
        if (View.BaseView != null)
        {
            AddViewModelProperty(View.ViewForElement);
            AddInitializeViewModelMethod(View.ViewForElement);
        }
        
        Namespace.Types.Add(Decleration);
    }

    private void AddBindingMembers()
    {
        var bindingGenerators =
            uFrameEditor.GetBindingGeneratorsFor(View.ViewForElement, isOverride: false, generateDefaultBindings: DiagramData.Settings.GenerateDefaultBindings,includeBaseItems: View.BaseView == null)
                .ToArray();

        foreach (var bindingGenerator in bindingGenerators)
        {
            if (View.BindingMethods.All(p => p.Name != bindingGenerator.MethodName) &&
                View.NewBindings.All(p => p.MethodName != bindingGenerator.MethodName)) continue;

            bindingGenerator.CreateMembers(Decleration.Members);
        }
    }
}