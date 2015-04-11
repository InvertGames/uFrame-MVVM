using System;
using Invert.uFrame.MVVM;

public abstract class uFrameMVVMPage<TParentPage> : uFrameMVVMPage
{

    public override Type ParentPage
    {
        get { return typeof (TParentPage); }
    }
}