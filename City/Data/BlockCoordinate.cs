using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace City.Data;

public struct BlockCoordinate : IEquatable<BlockCoordinate>
{
    public int X;
    public int Y;

    public BlockCoordinate Top => new(X, Y - 1);
    public BlockCoordinate Left => new(X - 1, Y);
    public BlockCoordinate Right => new(X + 1, Y);
    public BlockCoordinate Bottom => new(X, Y + 1);

    public BlockCoordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public IEnumerable<BlockCoordinate> Circle(int radius = 1)
    {
        List<BlockCoordinate> coordinates = new();
        for (int i = Y - radius; i < Y + radius; ++i)
        {
            int di2 = (i - Y) * (i - Y);
            for (int j = X - radius; j < Y + radius; ++j)
            {
                if ((j - X) * (j - X) + di2 <= (radius * radius))
                {
                    coordinates.Add(new(j, i));
                }
            }
        }
        return coordinates;
    }
    public IEnumerable<BlockCoordinate> Neighbours(int radius = 1)
    {
        List<BlockCoordinate> coordinates = new();
        for (int i = Y - radius; i < Y + radius; ++i)
        { 
            for (int j = X - radius; j < Y + radius; ++j)
            {
                coordinates.Add(new(j, i));
            }
        }
        return coordinates;
    }
    public BlockCoordinate Fuzz(int amount)
    {
        int x = RandomNumberGenerator.GetInt32(-amount, amount);
        int y = RandomNumberGenerator.GetInt32(-amount, amount);
        return new(X + x, Y + y);
    }

    public double DistanceTo(BlockCoordinate other) => Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y));
    public double ManhattanDistanceTo(BlockCoordinate other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

    public static BlockCoordinate operator +(BlockCoordinate lhs, BlockCoordinate rhs) => new(lhs.X + rhs.X, lhs.Y + rhs.Y);
    public static BlockCoordinate operator -(BlockCoordinate lhs, BlockCoordinate rhs) => new(lhs.X - rhs.X, lhs.Y - rhs.Y);
    public static bool operator ==(BlockCoordinate lhs, BlockCoordinate rhs) => lhs.Equals(rhs);
    public static bool operator !=(BlockCoordinate lhs, BlockCoordinate rhs) => !lhs.Equals(rhs);

    public bool Equals(BlockCoordinate other) => X == other.X && Y == other.Y;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is BlockCoordinate other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
}
