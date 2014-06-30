using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SceneManagerData : DiagramNode
{
    public override void Serialize(JSONClass cls)
    {
        base.Serialize(cls);
        cls.Add("SubSystemIdentifier", _subSystemIdentifier);
        
    }
    public override void Deserialize(JSONClass cls)
    {
        base.Deserialize(cls);
        _subSystemIdentifier = cls["SubSystemIdentifier"].Value;
    }

    [SerializeField]
    private FilterLocations _locations = new FilterLocations();

    [SerializeField]
    private string _subSystemIdentifier;

    [SerializeField]
    private List<SceneManagerTransition> _transitions = new List<SceneManagerTransition>();

    public override IEnumerable<IDiagramNodeItem> ContainedItems
    {
        get { return Transitions.Cast<IDiagramNodeItem>(); }
        set { Transitions = value.OfType<SceneManagerTransition>().ToList(); }
    }

    public Type CurrentSettingsType
    {
        get
        {
            return Type.GetType(UFrameAssetManager.DesignerVMAssemblyName.Replace("ViewModel", NameAsSettings));
        }
    }

    public Type CurrentType
    {
        get
        {
            return Type.GetType(UFrameAssetManager.DesignerVMAssemblyName.Replace("ViewModel", NameAsSceneManager));
        }
    }

    public bool ImportedOnly
    {
        get { return true; }
    }

    public override IEnumerable<IDiagramNodeItem> Items
    {
        get
        {
            //if (SubSystem == null) yield break;
            //foreach (var command in SubSystem.IncludedCommands)
            //{
            //    yield return command;
            //}
            return Transitions.Cast<IDiagramNodeItem>();
        }
    }

    public override string Label
    {
        get { return Name; }
    }

    public FilterLocations Locations
    {
        get { return _locations; }
        set { _locations = value; }
    }

    public string NameAsSceneManager
    {
        get { return string.Format("{0}", Name); }
    }

    public string NameAsSceneManagerBase
    {
        get { return string.Format("{0}Base", Name); }
    }

    public string NameAsSettings
    {
        get { return string.Format("{0}Settings", Name); }
    }

    public string NameAsSettingsField { get { return string.Format("_{0}Settings", Name); } }

    public RenameSceneManagerRefactorer RenameRefactor { get; set; }

    public ISubSystemData SubSystem
    {
        get
        {
            return Data.SubSystems.FirstOrDefault(p => p.Identifier == SubSystemIdentifier);
        }
    }

    public string SubSystemIdentifier
    {
        get { return _subSystemIdentifier; }
        set { _subSystemIdentifier = value; }
    }

    public List<SceneManagerTransition> Transitions
    {
        get { return _transitions; }
        set { _transitions = value; }
    }

    public override void BeginEditing()
    {
        base.BeginEditing();
        if (RenameRefactor == null)
        {
            RenameRefactor = new RenameSceneManagerRefactorer(this);
        }
    }

    public override bool CanCreateLink(IDrawable target)
    {
        return false;
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        //var scItem = target as sceneManagerData;
        //if (scItem == null) return;
        //Transitions.Add(new SceneManagerTransition() { ToIdentifier = scItem.Name,Name ="To" + scItem.Name });
    }

    public override RenameRefactorer CreateRenameRefactorer()
    {
        return new RenameSceneManagerRefactorer(this);
    }

    public override bool EndEditing()
    {
        return base.EndEditing();
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        // Sync commands to here
        if (this.SubSystem != null)
        {
            var commands = this.SubSystem.GetIncludedCommands().ToArray();

            foreach (var command in commands)
            {
                var transition = Transitions.FirstOrDefault(p => p.CommandIdentifier == command.Identifier);
                if (transition == null)
                {
                    Transitions.Add(new SceneManagerTransition()
                    {
                        Name = command.Name,
                        CommandIdentifier = command.Identifier
                    });
                }
                else
                {
                    transition.Name = command.Name;
                }
            }
            Transitions.RemoveAll(p => commands.All(x => x.Identifier != p.CommandIdentifier));
        }

        foreach (var transition in Transitions)
        {
            var linkedTo = nodes.OfType<SceneManagerData>().FirstOrDefault(p => p.Identifier == transition.ToIdentifier);
            if (linkedTo == null) continue;

            yield return new TransitionLink()
            {
                From = transition,
                To = linkedTo
            };
        }
        yield break;
    }

    public bool IsAllowed(object item, Type t)
    {
        if (t == typeof(SceneManagerData))
        {
            return false;
        }
        return true;
    }

    public override void RemoveFromDiagram()
    {
        base.RemoveFromDiagram();
        Data.RemoveNode(this);
    }

    public override void RemoveLink(IDiagramNode target)
    {
    }
}