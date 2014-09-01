using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckersGameController : CheckersGameControllerBase
{
    public virtual CheckerType Me { get; set; }

    public IEnumerable<CheckerMoveViewModel> GetAllowedMoves(CheckerViewModel checker)
    {
        return checker.GetMoves().Where(p => p.IsAllowed(CheckerBoardController.CheckerBoard));
    }

    /// <summary>
    /// Make a checker move
    /// </summary>
    /// <param name="desiredPosition">The desired position for the checker to move to.</param>
    public void MakeMove(Vector2 desiredPosition)
    {
        var checker = CheckersGame.CurrentChecker;
        if (checker == null) return;

        var move = GetAllowedMoves(checker).ToArray().FirstOrDefault(p => p.NewPosition == desiredPosition);
        if (move != null)
        {
            // Deselect the current checker
            StartCoroutine(SelectChecker(null));
            if (move.IsJump)
            {
                // Remove the jumped checker
                var remove = CheckerBoardController.CheckerBoard.Checkers.FirstOrDefault(p => p.Position == move.JumpPosition);
                if (remove != null)
                {
                    CheckerBoardController.CheckerBoard.Checkers.Remove(remove);
                    // Add To the score
                    if (remove.Type == CheckerType.Black)
                    {
                        CheckersGame.RedScore++;
                    }
                    else
                    {
                        CheckersGame.BlackScore++;
                    }

                }
            }

            checker.Position = desiredPosition;
            // Is the checker at the opposite end
            if ((checker.Position.y == 0 && checker.Type == CheckerType.Black) ||
                (checker.Position.y == 7 && checker.Type == CheckerType.Red)
                )
            {
                checker.IsKingMe = true;
            }

            if (move.IsJump)
            {
                var doubleJump = GetAllowedMoves(checker).FirstOrDefault(p => p.IsJump);
                if (doubleJump != null)
                {
                    CheckersGame.CurrentChecker = checker;
                    MakeMove(doubleJump.NewPosition);

                    return;
                }
            }

            // Switch player
            CheckersGame.CurrentPlayer = CheckersGame.CurrentPlayer == CheckerType.Black ? CheckerType.Red : CheckerType.Black;
        }
    }

    protected virtual void CurrentPlayerChanged(CheckersGameViewModel checkersGameViewModel, CheckerType type)
    {

    }

    public virtual IEnumerator SelectChecker(CheckerViewModel checker)
    {
        if (checker != null && checker.Type != CheckersGame.CurrentPlayer) yield break;
        // Make sure its their turn
        if (CheckersGame.CurrentPlayer == Me)
        {
            if (CheckersGame.CurrentChecker != null)
            {
                var allowedMoves = GetAllowedMoves(CheckersGame.CurrentChecker).ToArray();
                foreach (var move in allowedMoves)
                {
                    CheckerBoardController.CheckerBoard.Plates.First(p => p.Position == move.NewPosition).CanMoveTo = false;
                }
                CheckersGame.CurrentChecker.Selected = false;
            }

            CheckersGame.CurrentChecker = checker;

            if (CheckersGame.CurrentChecker != null)
            {
                CheckersGame.CurrentChecker.Selected = true;
                var allowedMoves = GetAllowedMoves(CheckersGame.CurrentChecker).ToArray();
                foreach (var move in allowedMoves)
                {
                    CheckerBoardController.CheckerBoard.Plates.First(p => p.Position == move.NewPosition).CanMoveTo = true;
                }
            }
        }
        yield break;
    }

    public virtual void SelectPlate(CheckerPlateViewModel model)
    {
        if (CheckersGame.CurrentPlayer == Me)
            MakeMove(model.Position);
    }

    public override void InitializeCheckersGame(CheckersGameViewModel checkersGame)
    {
        this.SubscribeToProperty(checkersGame, checkersGame._CurrentPlayerProperty, CurrentPlayerChanged);
    }

    public override void GameOver()
    {
        
    }
}