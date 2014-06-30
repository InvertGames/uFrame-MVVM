using System.Collections.Generic;
using Invert.uFrame.Editor.Refactoring;

public interface IElementDesignerData
{
    
    string Identifier { get; set; }
    // Settings
    ElementDiagramSettings Settings { get; }
    // Basic Information
    string Name { get; }
    string Version { get; set; }

    // Not Persisted
    int RefactorCount { get; set; }

    FilterState FilterState { get; set; }
    // Filters
    IDiagramFilter CurrentFilter { get; }

    // Queries
    IEnumerable<IDiagramNode> AllDiagramItems { get; }
    //IEnumerable<IDiagramNode> GetImportableItems();

    // Filter Stuff
    List<Refactorer> Refactorings { get; }
    SceneFlowFilter SceneFlowFilter { get;  }

    // Node Data
    IEnumerable<SceneManagerData> SceneManagers { get; }
    IEnumerable<ISubSystemData> SubSystems { get; }
    IEnumerable<ViewComponentData> ViewComponents { get; }
    IEnumerable<ElementData> Elements { get; }
    IEnumerable<ViewData> Views { get; }
    //IEnumerable<ImportedElementData> ImportedElements { get; }
    IEnumerable<EnumData> Enums { get; }

    // Dynamically loaded
    List<IDiagramLink> Links { get; }

    /// <summary>
    /// Should be called when first loaded.
    /// </summary>
    void Initialize();



    void AddNode(IDiagramNode data);
    void RemoveNode(IDiagramNode enumData);
}