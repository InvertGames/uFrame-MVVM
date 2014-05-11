
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class CheckerPlateController : CheckerPlateControllerBase {
    
    public override void InitializeCheckerPlate(CheckerPlateViewModel checkerPlate) {
    }
    
    public override void SelectCommand(CheckerPlateViewModel checkerPlate) {
        CheckerBoardController.SelectPlate(checkerPlate);
    }

}
