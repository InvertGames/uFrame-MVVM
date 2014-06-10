using System.Collections.Generic;

public interface IViewModelItem : IDiagramNodeItem, IRefactorable
{
    string RelatedType { get; set; }
    string RelatedTypeName { get; }
    bool AllowEmptyRelatedType { get;  }
}