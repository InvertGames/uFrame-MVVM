using System;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEngine;

public class ChangeLogPage : uFrameMVVMPage<MVVMPage>
{
    private TextAsset _changeLog;
    private string[] _lines;

    public TextAsset ChangeLog
    {
        get { return _changeLog ?? (_changeLog = Resources.Load("uFrameReadme", typeof(TextAsset)) as TextAsset); }
        set { _changeLog = value; }
    }
     
    public string[] Lines
    {
        get
        {
            return _lines ?? (_lines = ChangeLog.text.Split(Environment.NewLine.ToCharArray()));
        }
    }
    public override string Name
    {
        get { return "Change Log"; }
    }

    public override decimal Order
    {
        get { return 999; }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
//        foreach (var line in Lines)
//        {
//            _.Paragraph(line);
//        }

        _.Paragraph(ChangeLog.text);
    }
}