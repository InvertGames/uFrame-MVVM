using System.IO;

namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameIdentifierRefactorer : Refactorer
    {
        public string From { get; set; }
        public string To { get; set; }

        public bool RenameFile { get; set; }
        public string RenameFilename { get; set; }
        public RenameIdentifierRefactorer()
        {
        }

        public RenameIdentifierRefactorer(string @from, string to)
        {
            From = @from;
            To = to;
        }

        public override void Process(RefactorContext context)
        {
            if (context.CurrentToken == From)
            {
                context.CurrentToken = To;
                if (context.PreviousToken == "class" && RenameFilename == null)
                {
                    RenameFile = true;
                    RenameFilename = context.CurrentFilename;
                }
                    
            }
        }

        public override void PostProcess(RefactorContext context)
        {
            if (RenameFile)
            {
                var fileInfo = new FileInfo(RenameFilename);
                if (fileInfo.Directory != null) 
                    fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName,To + ".cs"));

                RenameFilename = null;
            }
        }
    }
}