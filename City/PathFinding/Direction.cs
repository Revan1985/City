using City.Data;

namespace City.PathFinding;

public enum Direction
{
    North,
    South,
    East,
    West,
    Stationary,
}

public static class DirectionExtension
{
    public static BlockCoordinate ToDelta(this Direction direction) => direction switch
    {
        Direction.North => new(0, -1),
        Direction.South => new(0, 1),
        Direction.East => new(1, 0),
        Direction.West => new(-1, 0),
        Direction.Stationary => new(0, 0),
        _ => new(0, 0),
    };
}
