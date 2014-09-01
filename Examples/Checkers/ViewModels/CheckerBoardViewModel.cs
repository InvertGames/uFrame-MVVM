using System.Linq;
using UnityEngine;

public partial class CheckerBoardViewModel
{

    public CheckerViewModel this[Vector2 position]
    {
        get
        {
            if (!IsOnCheckerBoard(position))
            {
                return null;
            }
            return _CheckersProperty.FirstOrDefault(c => c.Position == position);
        }
    }


    /// <summary>
    /// Conveniance method for determining wether or not a position is valid.
    /// </summary>
    /// <param name="pos">The position to check for</param>
    /// <returns>True if it's on the checker board</returns>
    public bool IsOnCheckerBoard(Vector2 pos)
    {
        if (pos.x >= 8 || pos.x <= -1) return false;
        return pos.y < 8 && pos.y > -1;
    }
}