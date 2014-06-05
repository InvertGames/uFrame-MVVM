using System.CodeDom;
using System.Linq;
using Invert.uFrame.Editor;

public class ViewGenerator : ViewClassGenerator
{
    public ViewData View
    {
        get; set;
    }

    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        AddView(View);
    }

    public void AddView(ViewData view)
    {
        var data = DiagramData.AllDiagramItems.FirstOrDefault(p => p.AssemblyQualifiedName == view.ForAssemblyQualifiedName);
        if (data == null) return;

        var decl = new CodeTypeDeclaration(view.NameAsView);
        decl.IsPartial = true;
        // var baseView = new CodeTypeReference(_diagramData.NameAsViewBase);
        //baseView.TypeArguments.Add(_diagramData.NameAsViewModel);
        var elementData = data as ElementData;
        if (IsDesignerFile)
        {
            if (elementData == null)
            {
                decl.BaseTypes.Add(new CodeTypeReference(data.Name));
            }
            else
            {
                decl.BaseTypes.Add(new CodeTypeReference(elementData.NameAsViewBase));
            }
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