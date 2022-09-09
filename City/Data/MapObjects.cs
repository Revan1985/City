using City.Data.Buildings;

namespace City.Data;

public enum TileType : byte
{
    Ground = 0,
    Water = 1,
}

public enum Zone : byte
{
    Residential = 0,
    Commercial = 1,
    Industrial = 2,
}

public class Location
{
    public BlockCoordinate Coordinate;
    public Building? Building;

    public IEnumerable<BlockCoordinate>? Blocks => Building?.BuildingBlocks(Coordinate);

    public bool Overlaps(Location other)
    {
        bool xOverlaps =
            Coordinate.X < other.Coordinate.X + other.Building.Width - 1 &&
            Coordinate.X + Building.Width - 1 > other.Coordinate.X;
        bool yOverlaps =
            Coordinate.Y < other.Coordinate.Y + other.Building.Height - 1 &&
            Coordinate.X + Building.Height - 1 > other.Coordinate.Y;

        return xOverlaps && yOverlaps;
    }
}
