using System;
using System.CodeDom;

namespace Invert.uFrame.Editor
{
    public interface ITypeDeclerationModifier
    {
        Type For { get; }
        CodeGenerator Generator { get; set; }
        CodeTypeDeclaration Decleration { get; set; }
        
        void Apply();
    }
}