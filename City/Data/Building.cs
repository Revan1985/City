using Path = City.PathFinding.Path;

namespace City.Data.Buildings;

public interface IHasInventory
{
    int Balance();
}

public abstract class Building
{
    public abstract int Width { get; }
    public abstract int Height { get; }

    public abstract bool HasAnyContract { get; }
    public abstract string Description { get; }



    public abstract void AddContract(Contract contract);
    public abstract void CreateContract(BlockCoordinate coordinate, ITradeEntity other, Tradeable tradeable, int quantity, Path path);
    public abstract void VoidContractsWith(ITradeEntity other);
    public abstract int CurrentQuantityForSale(Tradeable tradeable);
    public abstract int CurrentQuantityWanted(Tradeable tradeable);
    public abstract int ProducesQuantity(Tradeable tradeable);
    public abstract int ConsumesQuantity(Tradeable tradeable);
    public abstract int QuantityOnHand(Tradeable tradeable);
}
