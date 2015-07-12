using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using uFrame.MVVM;
using uFrame.MVVM.Bindings;
using uFrame.Serialization;


public class ResolutionInformation : ResolutionInformationBase {
    
    public override bool Equals(object obj)
    {
        if (obj is ResolutionInformation)
        {
            var otherRes = obj as ResolutionInformation;
            if (otherRes.Height == this.Height && otherRes.Width == this.Width) return true;
        }
        return false;
    }

}
