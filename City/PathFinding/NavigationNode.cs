using City.Data;

namespace City.PathFinding;

public class NavigationNode : IEquatable<NavigationNode>
{
    public BlockCoordinate Coordinate;
    public NavigationNode? Parent;
    public double Score = 0.0;
    public TransitType TransitType = TransitType.Road;
    public Direction Direction;

    public NavigationNode(BlockCoordinate coordinate, NavigationNode? parent, double score, TransitType transitType, Direction direction)
    {
        Coordinate = coordinate;
        Parent = parent;
        Score = score;
        TransitType = transitType;
        Direction = direction;
    }

    public bool Equals(NavigationNode? other) => Coordinate == other?.Coordinate;
    public override bool Equals(object? obj) => obj is NavigationNode other && Equals(other);
    public override int GetHashCode() => Coordinate.GetHashCode();
}
