using System.CodeDom;
using System.Linq;
using System.Reflection;
using Invert.uFrame.Editor;

public class ViewGenerator : ViewClassGenerator
{
    public ViewData View
    {
        get;
        set;
    }

    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        if (View.BaseNode == null)
        {
            return;
        }

        var baseView = View.BaseView;
        if (baseView != null && IsDesignerFile)
        {
            AddView(View);
            AddViewBase(View.ViewForElement as ElementData, View.NameAsViewViewBase, baseView.NameAsView);
        }
        else
        {
            AddView(View);
        }
    }

    public void AddView(ViewData view)
    {
        var decl = new CodeTypeDeclaration(view.NameAsView) {IsPartial = true};
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
            decl.Members.Add(CreateUpdateMethod(view));
        }
        else
        {
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