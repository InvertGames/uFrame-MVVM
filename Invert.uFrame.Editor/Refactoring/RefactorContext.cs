using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Invert.uFrame.Editor.Refactoring
{
    public class RefactorContext
    {
        //private List<string> _filenames;

        //public List<string> Filenames
        //{
        //    get { return _filenames; }
        //    set { _filenames = value; }
        //}
        public List<Refactorer> Refactors { get; set; }

        public string[] CurrentTokens { get; set; }

        public int CurrentTokenIndex { get; set; }

        public string CurrentToken
        {
            get { return CurrentTokens[CurrentTokenIndex]; }
            set { CurrentTokens[CurrentTokenIndex] = value; }
        }

        public string PreviousToken
        {
            get
            {
                if (CurrentTokenIndex == 0) return string.Empty;
                return CurrentTokens[CurrentTokenIndex - 1];
            }
            set { CurrentTokens[CurrentTokenIndex - 1] = value; }
        }

        public string NextToken
        {
            get
            {
                if (CurrentTokenIndex == CurrentTokens.Length - 1)
                    return string.Empty;
                return CurrentTokens[CurrentTokenIndex + 1];
            }
            set { CurrentTokens[CurrentTokenIndex + 1] = value; }
        }
        public string CurrentFileText { get; set; }

        public string CurrentFilename { get; set; }

        public void RefactorFile(string filename)
        {
            if (!File.Exists(filename)) return;
            CurrentFileText = File.ReadAllText(filename);
            CurrentFilename = filename; 
            string strRegex = @"@?[_\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*";
            var myRegex = new Regex(strRegex, RegexOptions.None);
            CurrentTokens = myRegex.Matches(CurrentFileText).OfType<Match>()
                .Select(p => p.Value).ToArray();
            for (var i = 0; i < CurrentTokens.Length; i++)
            {
                CurrentTokenIndex = i;
                foreach (var refactorer in Refactors.OrderBy(p=>p.Priority))
                {
                    refactorer.Process(this);
                }
            }
            var index = 0;
            CurrentFileText = myRegex.Replace(CurrentFileText, (m) => CurrentTokens[index++]);
            File.WriteAllText(filename,CurrentFileText);
    
                
        }

        public RefactorContext(List<Refactorer> refactors)
        {
            Refactors = refactors;
        }

        public void Refactor(params string[] filenames)
        {
            foreach (var filename in filenames)
            {
                RefactorFile(filename);
            }
            foreach (var refactorer in Refactors.OrderBy(p => p.Priority))
            {
                refactorer.PostProcess(this);
            }
        }
    }
}
