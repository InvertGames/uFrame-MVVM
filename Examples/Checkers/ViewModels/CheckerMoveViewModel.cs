using UnityEngine;

public partial class CheckerMoveViewModel : ViewModel
{
    public CheckerViewModel _CheckerViewModel;
    public Vector2 _Direction;

    public bool IsJump
    {
        get;
        set;
    }

    public Vector2 JumpPosition
    {
        get
        {
            return _CheckerViewModel.Position + _Direction;
        }
    }

    public Vector2 NewPosition
    {
        get
        {
            if (IsJump)
                return _CheckerViewModel.Position + _Direction + _Direction;

            return _CheckerViewModel.Position + _Direction;
        }
    }

    public Vector2 OldPosition
    {
        get
        {
            return _CheckerViewModel.Position;
        }
    }

    public bool IsAllowed(CheckerBoardViewModel gameView)
    {
        var newPosition = NewPosition;

        if (gameView.IsOnCheckerBoard(newPosition))
        {
            if (gameView[newPosition] == null)
            {
                if (IsJump)
                {
                    // Grab our jump position in the direction this move is going
                    var jumpedCheckerPosition = _CheckerViewModel.Position + _Direction;
                    // Grab the checker at the jump position
                    var jumpChecker = gameView[jumpedCheckerPosition];
                    // Is there a checker to jump over ?
                    if (jumpChecker == null)
                        return false;
                    // Is the checker we are jumping of the other player
                    if (jumpChecker.Type != _CheckerViewModel.Type)
                        return true;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
}