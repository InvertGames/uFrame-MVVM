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

        public abstract IEnumerable<CodeGenerator> GetGenerators(ElementDesignerData diagramData, IDiagramNode node);
    }

    public abstract class NodeItemGenerator<TData> : NodeItemGenerator where TData : DiagramNode
    {
        public override Type DiagramItemType
        {
            get { return typeof(TData); }
        }

        public sealed override IEnumerable<CodeGenerator> GetGenerators(ElementDesignerData diagramData, IDiagramNode node)
        {
            return CreateGenerators(diagramData, node as TData);
        }
        public abstract IEnumerable<CodeGenerator> CreateGenerators(ElementDesignerData diagramData, TData item);

    }
}