using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy,
    None
}

public enum EntityClass
{
    None,
    Tank,
    Warrior,
    Rogue,
    Mage
}

public class Entity
{
    public List<Vector3> stats = new List<Vector3>();
    public readonly IList<string> finalData = new List<string>();
    
    public void FillFinalData()
    {
        finalData.Clear();
        finalData.Add("Level,Health,Attack,Defense");
        for (int i = 0; i < stats.Count; i++)
        {
            finalData.Add(
                $"{i + 1},{stats[i].x.ToString("0.00", CultureInfo.GetCultureInfo("en-US"))},{stats[i].y.ToString("0.00", CultureInfo.GetCultureInfo("en-US"))},{stats[i].z.ToString("0.00", CultureInfo.GetCultureInfo("en-US"))}");
        }
    }

    public virtual EntityType GetEntity()
    {
        return EntityType.None;
    }

    public virtual EntityClass GetClass()
    {
        return EntityClass.None;
    }
}