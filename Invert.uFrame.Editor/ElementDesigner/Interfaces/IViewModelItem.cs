using System.Collections.Generic;

public interface IViewModelItem : IDiagramSubItem, IRefactorable
{
    
    //string Label { get; }

    
    string RelatedType { get; set; }
    string RelatedTypeName { get; }
    bool AllowEmptyRelatedType { get;  }

    IEnumerable<ElementItemType> GetAvailableRelatedTypes(IElementsDataRepository repository);

    
}