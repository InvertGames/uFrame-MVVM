namespace Invert.uFrame.Editor.Refactoring
{
    public class ChangeParameterTypeRefactor : Refactorer
    {
        public override int Priority
        {
            get { return 1; } // Make sure any renames happen first
        }

        public ChangeParameterTypeRefactor(string parameterName, string oldType, string newType)
        {
            ParameterName = parameterName;
            OldType = oldType;
            NewType = newType;
        }

        public override void Process(RefactorContext context)
        {
            if (ParameterName == context.CurrentToken)
            {
                if (context.PreviousToken == OldType)
                    context.PreviousToken = NewType;
            }
        }

        public override void PostProcess(RefactorContext context)
        {
            
        }

        public string OldType { get; set; }
        public string NewType { get; set; }

        public string ParameterName { get; set; }
    }
}