using City.Data.Buildings;
using City.PathFinding;
using City.Util;
using System.Reflection.Metadata.Ecma335;
using Path = City.PathFinding.Path;
using static System.Diagnostics.Debug;

namespace City.Data;

public enum Tradeable
{
    Money,
    Goods,
    Labor,
    RawMaterials,
    WholesaleGoods,
}

public static class TradeableExtensions
{
    public static int PriceForGoods(this Tradeable tradeable, int quantity) => tradeable switch
    {
        Tradeable.Money => quantity * 1,
        Tradeable.Goods => quantity * 5,
        Tradeable.Labor => quantity * 1,
        Tradeable.RawMaterials => quantity * 1,
        Tradeable.WholesaleGoods => quantity * 3,
        _ => 0,
    };
}

public interface ITradeEntity
{
    string? Description { get; }
    Building? Building { get; }
    BlockCoordinate Coordinate { get; }
    bool HasAnyContracts { get; }

    void AddContract(Contract contract);
    void CreateContract(BlockCoordinate coordinate, ITradeEntity other, Tradeable tradeable, int quantity, Path path);
    void VoidContractsWith(ITradeEntity other);

    int CurrentQuantityForSale(Tradeable tradeable);
    int CurrentQuantityWanted(Tradeable tradeable);

    int ProducesQuantity(Tradeable tradeable);
    int ConsumesQuantity(Tradeable tradeable);
    int QuantityOnHand(Tradeable tradeable);
}


public class CityTradeEntity : ITradeEntity
{
    internal Building _building;
    public Building? Building => _building;

    public string? Description => _building.Description;
    public BlockCoordinate Coordinate { get; }
    public bool HasAnyContracts => _building.HasAnyContract;

    public CityTradeEntity(BlockCoordinate coordinate, Building building)
    {
        _building = building;
        Coordinate = coordinate;
    }

    public void AddContract(Contract contract) => _building.AddContract(contract);
    public void CreateContract(BlockCoordinate coordinate, ITradeEntity other, Tradeable tradeable, int quantity, Path path)
     => Building?.CreateContract(coordinate, other, tradeable, quantity, path);
    public void VoidContractsWith(ITradeEntity other) => _building.VoidContractsWith(other);

    public int CurrentQuantityForSale(Tradeable tradeable) => _building.CurrentQuantityForSale(tradeable);
    public int CurrentQuantityWanted(Tradeable tradeable) => _building.CurrentQuantityWanted(tradeable);

    public int ProducesQuantity(Tradeable tradeable) => _building.ProducesQuantity(tradeable);
    public int ConsumesQuantity(Tradeable tradeable) => _building.ConsumesQuantity(tradeable);
    public int QuantityOnHand(Tradeable tradeable) => _building.QuantityOnHand(tradeable);

}

public class Contract : IDebuggable
{
    public ITradeEntity From;
    public ITradeEntity To;
    public Tradeable Tradeable;
    public int Quantity;
    public Path? Path;

    public bool Debug => false;


    public Contract(ITradeEntity from, ITradeEntity to, Tradeable tradeable, int quantity, Path? path)
    {
        From = from;
        To = to;
        Tradeable = tradeable;
        Quantity = quantity;
        Path = path;
    }

    public bool Execute()
    {
        lock (From)
        {
            lock (To)
            {
                int quantityWanted = From.CurrentQuantityWanted(Tradeable);
                int quantityAvailable = To.CurrentQuantityForSale(Tradeable);

                if (quantityAvailable >= quantityWanted)
                {
                    Building buildingFrom = From.Building ?? throw new InvalidOperationException();
                    Building buildingTo = To.Building ?? throw new InvalidOperationException();

                    WriteLine($"The customer {buildingTo.Description} naturally wants this many {Tradeable} :{buildingTo.ConsumesQuantity(Tradeable)}");
                    int doubleTradeable = buildingTo.ConsumesQuantity(Tradeable) * 2;
                    WriteLine($"So let's try and make sure they have {doubleTradeable}");
                    int customerWantsQuantity = doubleTradeable - buildingTo.QuantityOnHand(Tradeable);
                    WriteLine($"They already have {buildingTo.QuantityOnHand(Tradeable)}");
                    WriteLine($"So they want {customerWantsQuantity} more...");

                    int howManyToSend = new[] { customerWantsQuantity, buildingFrom.QuantityOnHand(Tradeable) }.Min();
                    if (howManyToSend > 0)
                    {
                        WriteLine($"We have {buildingFrom.QuantityOnHand(Tradeable)} and will send them {howManyToSend}");
                        WriteLine($"Before transfer... building has {buildingFrom.QuantityOnHand(Tradeable.Money)}");
                        int howManyTransferred = buildingFrom.TransferInventory(To, Tradeable, howManyToSend);
                        buildingFrom.AddInventory(Tradeable.Money, Tradeable.PriceForGoods(howManyToSend));
                        WriteLine($"{buildingFrom.Description}: We transferred {howManyToSend} {Tradeable} to {To.Description}");
                        WriteLine($"After transfer... building has {buildingFrom.QuantityOnHand(Tradeable.Money)}");
                    }
                    else
                    {
                        WriteLine($"{buildingFrom.Description} wanted to send {Quantity} {Tradeable} but it was out of stock...");
                        return false;
                    }
                }
                return true;
            }
        }
    }

    public void VoidContracts()
    {
        lock (From)
        {
            lock (To)
            {
                From.VoidContractsWith(To);
                To.VoidContractsWith(From);
            }
        }
    }


    public override string ToString() => $"Contract(To={To.Description} From={From.Description} Tradeable={Tradeable} Quantity={Quantity})";
}

public class Inventory
{
    private Dictionary<Tradeable, int> _inventory = new();

    public int Add(Tradeable tradeable, int quantity)
    {
        if (!_inventory.TryGetValue(tradeable, out int onHand))
        {
            _inventory.Add(tradeable, 0);
        }
        _inventory[tradeable] += quantity;
        return _inventory?[tradeable] ?? 0;
    }
    public int Subtract(Tradeable tradeable, int quantity)
    {
        if (!_inventory.TryGetValue(tradeable, out int onHand))
        {
            _inventory.Add(tradeable, 0);
        }
        _inventory[tradeable] -= quantity;
        return _inventory?[tradeable] ?? 0;
    }

    public bool Has(Tradeable tradeable, int quantity) => _inventory.TryGetValue(tradeable, out int onHand) && onHand >= quantity;
    public int Quantity(Tradeable tradeable) => _inventory.GetValueOrDefault(tradeable, 0);
    public int Put(Tradeable tradeable, int quantity)
    {
        if(_inventory.TryGetValue(tradeable, out int onHand))
        {
            _inventory[tradeable] = quantity;
            return onHand;
        }
        _inventory.Add(tradeable, quantity);
        return 0;
    }

    public void ForEach()
    {
        
    }
}