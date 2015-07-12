using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Kernel;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, "{0}")]
    public partial class SceneTemplate : IClassTemplate<SceneTypeNode>, IClassRefactorable
    {
        [GenerateProperty]
        public virtual string DefaultKernelScene
        {
            get
            {
                Ctx.CurrentProperty.Attributes = MemberAttributes.Override | MemberAttributes.Public;
                Ctx._("return \"{0}KernelScene\"", Ctx.Data.Graph.Project.Name);
                return null;
            }
        }

        public void TemplateSetup()
        {
            if (Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Add(typeof (MonoBehaviour).ToCodeReference());
                Ctx.SetBaseType(typeof (Scene));
            }
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


        [GenerateProperty()]
        public virtual object Settings
        {
            get
            {
                //Ctx.SetType("{0}Settings");
                Ctx.SetType(string.Format("{0}Settings", Ctx.Data.Name).ToCodeReference());
                Ctx._(string.Format("return _SettingsObject as {0}Settings", Ctx.Data.Name));
                return null;
            }
            set { Ctx._(string.Format("_SettingsObject = value")); }
        }

        public IEnumerable<string> ClassNameFormats
        {
            get
            {
                yield return "{0}";
                yield return "{0}Base";
            }
        }
    }
}