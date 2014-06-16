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
        if (View.ViewForElement == null) return;

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



        var decl = new CodeTypeDeclaration(view.NameAsView);
        decl.IsPartial = true;
        if (view.ViewForElement.IsTemplate)
        {
            decl.TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public;
        }
        // var baseView = new CodeTypeReference(_diagramData.NameAsViewBase);
        //baseView.TypeArguments.Add(_diagramData.NameAsViewModel);

        if (IsDesignerFile)
        {
            var viewViewBase = View.BaseView != null;
            decl.BaseTypes.Add(new CodeTypeReference(viewViewBase ? view.NameAsViewViewBase : view.BaseViewName));
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