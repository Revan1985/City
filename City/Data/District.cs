using System.Security.Cryptography;
using Vortice.Mathematics;

namespace City.Data;

public class District
{
    public string Name;
    public List<BlockCoordinate> Blocks = new();
    public BlockCoordinate? TopLeft;
    public BlockCoordinate? BottomRight;
    public Color Color;


    public District(string name, List<BlockCoordinate> blocks, BlockCoordinate? topLeft, BlockCoordinate? bottomRight)
    {
        Name = name;
        Blocks = blocks;
        TopLeft = topLeft;
        BottomRight = bottomRight;
        Color = new(RandomNumberGenerator.GetInt32(256), RandomNumberGenerator.GetInt32(256), RandomNumberGenerator.GetInt32(256));
    }
    public District(string name, List<BlockCoordinate> blocks, BlockCoordinate? topLeft, BlockCoordinate? bottomRight, Color color)
        :this(name, blocks, topLeft, bottomRight)
    {
        Color = color;
    }
    
    public void ClearCorners()
    {
        TopLeft = null;
        BottomRight = null;
    }
}
