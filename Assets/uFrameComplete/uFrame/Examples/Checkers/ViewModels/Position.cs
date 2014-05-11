using System;
using UnityEngine;

[Serializable]
public struct Position
{
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Position operator -(Position p1, Position p2)
    {
        return new Position(p1.x - p2.x, p1.y - p2.y);
    }

    public static bool operator !=(Position p1, Position p2)
    {
        return !(p1 == p2);
    }

    public static Position operator +(Position p1, Position p2)
    {
        return new Position(p1.x + p2.x, p1.y + p2.y);
    }

    public static bool operator ==(Position p1, Position p2)
    {
        return p1.Equals(p2);
    }

    public bool Equals(Position other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is Position && Equals((Position)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (x * 397) ^ y;
        }
    }
}