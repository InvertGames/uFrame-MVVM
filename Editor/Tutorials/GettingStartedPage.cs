using System.Collections.Generic;
using Invert.uFrame.MVVM;

public class GettingStartedPage : uFrameMVVMPage
{
    public override string Name
    {
        get { return "Getting Started"; }
    }
    
    public override decimal Order
    {
        get { return -2; }
    }
}