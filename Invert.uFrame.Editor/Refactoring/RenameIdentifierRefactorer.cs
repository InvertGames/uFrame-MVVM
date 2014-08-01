using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Text;
using Invert.uFrame.Code.Bindings;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        public override void PreProcess(RefactorContext refactorContext)
        {

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
            var absoluteRootPath = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "/Assets".Length).Replace("/", Path.DirectorySeparatorChar.ToString()), RootPath);
            var fromName = Path.Combine(absoluteRootPath, From);
            var toName = Path.Combine(absoluteRootPath, To);
            var from = new FileInfo(fromName);



            if (from.Exists)
                from.MoveTo(toName);


            this.Applied = true;
        }

        public override void PostProcess(RefactorContext context)
        {

        }

        public override void PreProcess(RefactorContext refactorContext)
        {

        }
    }

    public class InsertMethodRefactorer : Refactorer
    {

        public string InsertText { get; set; }
        public string ClassName { get; set; }
        public bool Complete { get; set; }

        public override void Process(RefactorContext context)
        {
            if (Complete) return;
 
            if (context.PreviousToken == "class" && ClassName.ToUpper() == context.CurrentToken.ToUpper())
            {
                Complete = false;
                InClass = true;
            }

            if (context.CurrentToken == "{" && !Complete && InClass)
            {
                context.CurrentToken = string.Format("{{ {0}{1}", Environment.NewLine, InsertText.ToString());
                Complete = true;
            }


        }

        public bool InClass { get; set; }

        public override void PostProcess(RefactorContext context)
        {
            
        }

        public override void PreProcess(RefactorContext refactorContext)
        {
         
        }
    }
}