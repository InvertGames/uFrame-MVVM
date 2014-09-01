
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class AICheckersGameController : AICheckersGameControllerBase {
    
    public override void InitializeAICheckersGame(AICheckersGameViewModel aICheckersGame) {
        
    }
    public IEnumerator DecideAndMove()
    {
        yield return new WaitForSeconds(3f);
        var allowedMoves = CheckerBoardController.CheckerBoard.Checkers.Where(p => p.Type != Me).SelectMany(p => GetAllowedMoves(p)).ToArray();
        var jumpMove = allowedMoves.FirstOrDefault(p => p.IsJump);
        if (jumpMove != null)
        {
            CheckersGame.CurrentChecker = jumpMove._CheckerViewModel;
            MakeMove(jumpMove.NewPosition);
        }
        else
        {
            if (allowedMoves.Length > 0)
            {
                var random = allowedMoves[Random.Range(0, allowedMoves.Length)];
                var checker = random._CheckerViewModel;
                CheckersGame.CurrentChecker = checker;
                MakeMove(random.NewPosition);
            }
        }
    }

    protected override void CurrentPlayerChanged(CheckersGameViewModel checkersGameViewModel, CheckerType type)
    {
        base.CurrentPlayerChanged(checkersGameViewModel, type);
        Debug.Log("Current player changed.");
        if (type!= Me)
        {
            StartCoroutine(DecideAndMove());
        }
    }
}
