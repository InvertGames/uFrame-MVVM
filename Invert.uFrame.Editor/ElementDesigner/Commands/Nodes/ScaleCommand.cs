using Invert.Common;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class ScaleCommand : EditorCommand<float>
    {
        public float Scale { get; set; }

        public override void Perform(float node)
        {

            ElementDesignerStyles.Scale = Scale;

            
        }

        public override string CanPerform(float node)
        {
            if (Scale < 0.5f)
            {
                return "Can't scale that small";
            }
            return null;
        }
    }
}