using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class CheckerViewModel : ViewModel
{
    public Vector2 DiagonalBackLeft
    {
        get
        {
            return Type == CheckerType.Black ? new Vector2(-1, 1) : new Vector2(1, -1);
        }
    }

    public Vector2 DiagonalBackRight
    {
        get
        {
            return Type == CheckerType.Black ? new Vector2(1, 1) : new Vector2(-1, -1);
        }
    }

    public Vector2 DiagonalLeft
    {
        get
        {
            return Type == CheckerType.Black ? new Vector2(-1, -1) : new Vector2(1, 1);
        }
    }

    public Vector2 DiagonalRight
    {
        get
        {
            return Type == CheckerType.Black ? new Vector2(1, -1) : new Vector2(-1, 1);
        }
    }

    public IEnumerable<CheckerMoveViewModel> GetMoves()
    {
        yield return new CheckerMoveViewModel() { _CheckerViewModel = this, _Direction = DiagonalRight, IsJump = false };
        yield return new CheckerMoveViewModel() { _CheckerViewModel = this, _Direction = DiagonalRight, IsJump = true };
        yield return new CheckerMoveViewModel() { _CheckerViewModel = this, _Direction = DiagonalLeft, IsJump = false };
        yield return new CheckerMoveViewModel() { _CheckerViewModel = this, _Direction = DiagonalLeft, IsJump = true };
        if (IsKingMe)
        {
            yield return new CheckerMoveViewModel() { _CheckerViewModel = this, _Direction = DiagonalBackRight, IsJump = false };
            yield return new CheckerMoveViewModel() { _CheckerViewModel = this, _Direction = DiagonalBackRight, IsJump = true };
            yield return new CheckerMoveViewModel() { _CheckerViewModel = this, _Direction = DiagonalBackLeft, IsJump = false };
            yield return new CheckerMoveViewModel() { _CheckerViewModel = this, _Direction = DiagonalBackLeft, IsJump = true };
        }
    }
}