using System;
using System.Collections.Generic;

namespace Invert.uFrame.Editor
{
    public abstract class NodeItemGenerator
    {
        public abstract Type DiagramItemType
        {
            get;
        }

        [Inject]
        public IUFrameContainer Container { get; set; }

        [Inject]
        public IElementsDataRepository Repository { get; set; }

        public object ObjectData { get; set; }

        public abstract IEnumerable<CodeGenerator> GetGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, IDiagramNode node);
    }

    public abstract class NodeItemGenerator<TData> : NodeItemGenerator where TData : DiagramNode
    {
        public override Type DiagramItemType
        {
            get { return typeof(TData); }
        }

        public sealed override IEnumerable<CodeGenerator> GetGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, IDiagramNode node)
        {
            return CreateGenerators(pathStrategy, diagramData, node as TData);
        }
        public abstract IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, TData item);

    }
}