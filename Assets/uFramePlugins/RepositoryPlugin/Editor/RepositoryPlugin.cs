using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.uFrame;
using Invert.uFrame.Editor;
using UnityEngine;
using System.Collections;
using System.CodeDom;
public class RepositoryPlugin : DiagramPlugin {
    public override void Initialize(uFrameContainer container)
    {
        // Register the Repository node generator
        container.Register<DesignerGeneratorFactory, RepositoryGeneratorFactory>("RepositoryGenerator");
        // Register the type modifiers
        container.RegisterInstance<ITypeGeneratorPostProcessor>(
            new RepositorySaveMethodGenerator(),
            "RepositorySaveMethod");
    }
}

public class RepositoryGeneratorFactory : DesignerGeneratorFactory<IElementDesignerData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, IElementDesignerData item)
    {
        var elements = new List<ElementData>();
        // Create the file generators
        foreach (var elementData in diagramData.Elements)
        {
            if (diagramData.Links.Any(p => p.Target == elementData)) continue;
            if (elementData.IsMultiInstance) continue;
           elements.Add(elementData);
        }

        
        yield return new RepositoryInterfaceGenerator()
        {
            IsDesignerFile = true,
            DesignerData = diagramData,
            Elements = elements,
            Filename = diagramData.Name + "Repository.designer.cs"//Path.Combine("Repository", diagramData.Name + "Repository.designer.cs")
        };
        yield return new RepositoryConcreteGenerator()
        {
            IsDesignerFile = true,
            DesignerData = diagramData,
            Elements = elements,
            Name = "Default",
            Filename = diagramData.Name + "Repository.designer.cs"//Path.Combine("Repository", diagramData.Name + "Repository.designer.cs")
        };
    }
}

/// <summary>
/// A base class for different any repository type
/// </summary>
public abstract class RepositoryClassGenerator : CodeGenerator
{
    public IElementDesignerData DesignerData { get; set; }
    public List<ElementData> Elements { get; set; }
}

/// <summary>
/// The interface for the repository
/// </summary>
public class RepositoryInterfaceGenerator : RepositoryClassGenerator
{
    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        var declaration = new CodeTypeDeclaration()
        {
            Name = "I" + DesignerData.Name + "Repository",
            IsInterface = true
        }; ;
        ProcessModifiers(declaration);
        Namespace.Types.Add(declaration);
    }
}
/// <summary>
/// The interface for the repository
/// </summary>
public class RepositoryConcreteGenerator : RepositoryClassGenerator
{
    public string Name { get; set; }
    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        var declaration = new CodeTypeDeclaration()
        {
            Name = Name + DesignerData.Name + "Repository",
            IsInterface = false,
        };
        declaration.BaseTypes.Add(new CodeTypeReference("DataRepository"));
        declaration.BaseTypes.Add(new CodeTypeReference("I" + DesignerData.Name + "Repository"));
        ProcessModifiers(declaration);
        Namespace.Types.Add(declaration);
    }
}

/// <summary>
/// The save method on the interface and concrete repository types ( Hence the generic RepositoryClassGenerator type parameter )
/// </summary>
public class RepositorySaveMethodGenerator : TypeGeneratorPostProcessor<RepositoryClassGenerator>
{
    public bool IsInterface
    {
        get { return Declaration.IsInterface; }
    }

    public override void Apply()
    {
        foreach (var element in CodeGenerator.Elements)
        {
            Declaration.Members.Add(CreateElementMethod(element));
        }
    }

    private CodeMemberMethod CreateElementMethod(ElementData element)
    {
        var method = new CodeMemberMethod()
        {
            Name = "Save" + element.Name,
            Attributes = MemberAttributes.Public,
        };
        if (!IsInterface)
        {
            method.Statements.Add(new CodeSnippetStatement("Storage.Save(model);"));
        }
        method.Parameters.Add(new CodeParameterDeclarationExpression(element.NameAsViewModel, "model"));
        return method;
    }
}
