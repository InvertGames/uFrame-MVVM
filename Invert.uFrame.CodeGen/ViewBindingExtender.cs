using System.CodeDom;

public abstract class ViewBindingExtender
{
    public abstract void ExtendPropertyBinding(ElementData element, CodeStatementCollection trueStatements, ViewModelPropertyData property, ElementDataBase relatedElement);

    public abstract void ExtendViewBase(CodeTypeDeclaration decl, ElementData elementData);

    public abstract bool Initialize(ViewClassGenerator viewClassGenerator);
}