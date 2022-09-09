namespace City.Data;

public enum ZotType : byte
{
    TooMuchTraffic = 0,
    UnhappyNeighbors,
    NoGoods,
    NoWorkers,
    NoDemand,
    NoPower,
    NoCustomers,
    TooMuchPollution,
}

public record struct Zot(ZotType Type, int Age = 1);
