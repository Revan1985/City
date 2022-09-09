using City.Data;

namespace City.PathFinding
{
    public class Path
    {
        internal List<NavigationNode> Nodes { get; } = new();

        public int Length => Nodes.Count;
        public int TotalScore => Nodes.Sum(n => (int)n.Score);
        public IEnumerable<BlockCoordinate> Blocks => Nodes.Select(n => n.Coordinate);

        public Path(IEnumerable<NavigationNode>? nodes)
        {
            if (nodes == null) { throw new ArgumentNullException(nameof(nodes)); }
            Nodes = new(nodes);
        }

        public Path Plus(Path? other)
        {
            if (other == null || other.Nodes.Count == 0) { return this; }
            List<NavigationNode> otherNodes = other.Nodes;
            NavigationNode firstNode = otherNodes.First();
            NavigationNode newFirst = new(firstNode.Coordinate, Nodes.Last(), firstNode.Score, firstNode.TransitType, firstNode.Direction);
            List<NavigationNode> newOtherList = new[] { newFirst }.Union(otherNodes.Except(new[] { firstNode })).ToList();
            return new(Nodes.Union(newOtherList).Distinct());
        }
    }
}