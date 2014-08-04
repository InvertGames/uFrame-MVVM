using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PipeController : PipeControllerBase {
    
    public override void InitializePipe(PipeViewModel pipe) {
    }

    public override void Passed(PipeViewModel pipe)
    {
        base.Passed(pipe);
        pipe.ParentAngryFlappersGame.Score++;
    }
}
