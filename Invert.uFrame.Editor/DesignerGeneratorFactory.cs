using System;
using System.Collections.Generic;

namespace Invert.uFrame.Editor
{
    public abstract class DesignerGeneratorFactory
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

        public abstract IEnumerable<CodeGenerator> GetGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, object node);
    }

    public abstract class DesignerGeneratorFactory<TData> : DesignerGeneratorFactory where TData : class
    {
        public override Type DiagramItemType
        {
            get { return typeof(TData); }
        }

        public sealed override IEnumerable<CodeGenerator> GetGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, object node)
        {
            return CreateGenerators(pathStrategy, diagramData, node as TData);
        }
        public abstract IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, TData item);

    }
}