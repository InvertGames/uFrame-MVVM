using System.Collections.Generic;
using System.IO;
using Invert.uFrame;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEngine;
using System.Collections;
using System.CodeDom;
public class SamplePlugin : DiagramPlugin {
    public override void Initialize(uFrameContainer container)
    {
       // container.RegisterInstance<IDiagramNodeCommand>(new SampleMessageCommand(), "SampleMessage");
        container.RegisterInstance<IToolbarCommand>(new SampleMessageCommand(), "SampleMessage");
        var command = new SampleMessageCommand();
        uFrameEditor.HookCommand<IToolbarCommand>("SampleMessage", command);

        container.Register<NodeItemGenerator, SamplePluginNodeGenerator>("SamplePluginCodeGenerator");
        //uFrameEditor.HookCommand<IDiagramContextCommand>("SampleMessage", command);
    }
}

public class SampleMessageCommand : EditorCommand<IElementDesignerData>, IDiagramContextCommand, IToolbarCommand
{
    public override void Perform(IElementDesignerData node)
    {
        Debug.Log(node.Name);
    }

    public override string CanPerform(IElementDesignerData node)
    {

        return null;
    }

    public ToolbarPosition Position
    {
        get { return ToolbarPosition.Right; }
    }
}

public class SamplePluginNodeGenerator : NodeItemGenerator<ElementData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ElementData item)
    {
        foreach (var command in item.Commands)
        {
            yield return new PlaymakerActionCodeGenerator()
            {
                IsDesignerFile = false,
                CommandData = command,
                DiagramData = diagramData,
                Filename = Path.Combine("Playmaker",command.Name +"PlaymakerAction.cs")
            };
            yield return new PlaymakerActionCodeGenerator()
            {
                IsDesignerFile = true,
                CommandData = command,
                DiagramData = diagramData,
                Filename = diagramData.Name + "PlaymakerActions.designer.cs"
            };
        }
        
    }
}

public class PlaymakerActionCodeGenerator : CodeGenerator
{
    public ViewModelCommandData CommandData { get; set; }
    public IElementDesignerData DiagramData { get; set; }

    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        
        Namespace.Name = DiagramData.Name + ".PlaymakerActions";
        Namespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
//        Namespace.Imports.Add(new CodeNamespaceImport("PlaymakerFSM"));
        var classDecleration = new CodeTypeDeclaration();
        classDecleration.Name = CommandData.Name + (IsDesignerFile ? "PlaymakerActionBase" : "PlaymakerAction");
        classDecleration.Attributes = MemberAttributes.Public;

        classDecleration.BaseTypes.Add(new CodeTypeReference(IsDesignerFile ? "MonoBehaviour" : CommandData.Name + "PlaymakerActionBase"));

        Namespace.Types.Add(classDecleration);

    }

}