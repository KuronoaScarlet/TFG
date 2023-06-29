using UnityEngine;

public class Enemy : Entity
{
    private readonly EntityType _t;
    private readonly EntityClass _c;
    public Enemy(Vector3 initialStats, EntityClass enType, EntityType type = EntityType.Enemy)
    {
        _t = type;
        _c = enType;
        
        switch (enType)
        {
            case EntityClass.Tank:
                // Gradación de las stats
                stats.Add(new Vector3(initialStats.x, initialStats.y * 0.6f, initialStats.z));
                break;
            case EntityClass.Warrior:
                // Gradación de las stats
                stats.Add(new Vector3(initialStats.x * 0.9f, initialStats.y * 0.9f, initialStats.z * 0.8f));
                break;
            case EntityClass.Rogue:
                // Gradación de las stats
                stats.Add(new Vector3(initialStats.x * 0.6f, initialStats.y, initialStats.z * 0.7f));
                break;
            case EntityClass.Mage:
                // Gradación de las stats
                stats.Add(new Vector3(initialStats.x * 0.45f, initialStats.y * 1.2f, initialStats.z));
                break;
        }
    }
    
    public override EntityType GetEntity()
    {
        return _t;
    }
    
    public override EntityClass GetClass()
    {
        return _c;
    }
}