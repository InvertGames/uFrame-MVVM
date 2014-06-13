using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

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
                    RenameFilename = context.CurrentFilename.Replace(To + ".cs", From + ".cs");
                }
                    
            }
        }

        public override void PostProcess(RefactorContext context)
        {
            //if (RenameFile)
            //{
            //    var fileInfo = new FileInfo(RenameFilename);
            //    if (fileInfo.Directory != null)
            //    {
            //        fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName, To + ".cs"));
            //    }
            //    RenameFilename = null;
            //}
        }
    }

    public class RenameFileRefactorer : Refactorer
    {
        public string From { get; set; }
        public string To { get; set; }
        public string RootPath { get; set; }

        public RenameFileRefactorer()
        {
        }

        public RenameFileRefactorer(string @from, string to)
        {
            From = @from;
            To = to;
        }

        public override void Process(RefactorContext context)
        {
            if (Applied) return;
            var absoluteRootPath = Path.Combine(Application.dataPath.Substring(0,Application.dataPath.Length - "/Assets".Length).Replace("/",Path.DirectorySeparatorChar.ToString()),RootPath);
            var fromName = Path.Combine(absoluteRootPath, From);
            var from = new FileInfo(fromName);
            if (from.Exists)
                from.MoveTo(Path.Combine(absoluteRootPath, To));
            this.Applied = true;
        }

        public override void PostProcess(RefactorContext context)
        {
            
        }
    }
}