using UnityEngine;

public enum PlayerGrowthState
{
    GenStats,
    FixStats
}

public class Player : Entity
{
    private readonly EntityType _t;
    
    public Player(EntityType type = EntityType.Player)
    {
        _t = type;
    }

    public void AddInitialStats(Vector3 initialStats)
    {
        stats.Add(initialStats);
        
    }

    public override EntityType GetEntity()
    {
        return _t;
    }
}