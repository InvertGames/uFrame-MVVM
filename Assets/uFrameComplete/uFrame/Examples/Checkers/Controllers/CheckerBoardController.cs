using System;
using UnityEngine;

public class CheckerBoardController : CheckerBoardControllerBase
{
    public static int[,] _StartLayout =
    {
        {1, 0, 1, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 0, 1, 0, 1},
        {1, 0, 1, 0, 1, 0, 1, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 1, 0, 1, 0, 1, 0, 1},
        {1, 0, 1, 0, 1, 0, 1, 0},
        {0, 1, 0, 1, 0, 1, 0, 1},
    };
    public override void InitializeCheckerBoard(CheckerBoardViewModel checkerBoard)
    {
        var total = 0;
        for (var y = 0; y < 8; y++)
        {
            var isEvenRow = y % 2 == 0;
            if (isEvenRow)
            {
                total++;
            }
            for (var x = 0; x < 8; x++)
            {
                var position = new Vector2(x, y);
                var plate = CheckerPlateController.CreateCheckerPlate();
                plate.IsEven = total%2 == 0;
                // Create the plate first
                checkerBoard.Plates.Add(plate);
                plate.Position = position;
                bool isRed = y <= 2;
                bool place = _StartLayout[y, x] == 1;
                if (place)
                {
                    var checker = CheckerController.CreateChecker();
                    checker.Type = isRed ? CheckerType.Red : CheckerType.Black;
                    checker.Position = position;
                    checkerBoard.Checkers.Add(checker);
                }
                total++;
            }
            if (isEvenRow)
            {
                total--;
            }
        }
    }

    public void SelectPlate( CheckerPlateViewModel arg)
    {
        CheckersGameController.SelectPlate(arg);
    }

    public void SelectChecker(CheckerViewModel checker)
    {
        StartCoroutine(CheckersGameController.SelectChecker(checker));
    }

}