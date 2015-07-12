using System.CodeDom;
using System.Collections.Generic;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, "{0}Loader")]
    public partial class SceneLoaderTemplate : IClassTemplate<SceneTypeNode>, IClassRefactorable
    {
        public IEnumerable<string> ClassNameFormats
        {
            get
            {
                yield return "{0}";
                yield return "{1}Base";
            }
        }

        public void TemplateSetup()
        {
            this.Ctx.TryAddNamespace("uFrame.IOC");
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.MVVM");
            this.Ctx.TryAddNamespace("uFrame.Serialization");
            if (Ctx.IsDesignerFile)
            {
                //Ctx.SetBaseType("SceneLoader<{0}>",Ctx.Data.Name)
                Ctx.CurrentDeclaration.BaseTypes.Clear();
                Ctx.CurrentDeclaration.BaseTypes.Add(string.Format("SceneLoader<{0}>", Ctx.Data.Name));
            }
            else
            {
                Ctx.TryAddNamespace("UnityEngine");
            }
        }

        [GenerateMethod(CallBase = false), Inside(TemplateLocation.Both)]
        protected virtual void LoadScene(object scene, object progressDelegate)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Data.Name);
            Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference("Action<float, string>");
            Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            Ctx.CurrentMethod.ReturnType = "IEnumerator".ToCodeReference();
            Ctx._("yield break");
        }

        [GenerateMethod(CallBase = false), Inside(TemplateLocation.Both)]
        protected virtual void UnloadScene(object scene, object progressDelegate)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Data.Name);
            Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference("Action<float, string>");
            Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            Ctx.CurrentMethod.ReturnType = "IEnumerator".ToCodeReference();


            Ctx._("yield break");
        }

        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "Scenes"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public TemplateContext<SceneTypeNode> Ctx { get; set; }
    }
}