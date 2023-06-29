using System.Collections.Generic;
using UnityEngine;

public class NewEnemyDropdown : MonoBehaviour
{
    public EntityClass enemyType = EntityClass.None;
    public List<EntityClass> options;

    public void DropdownValueChange(int index)
    {
        enemyType = options[index];
    }
}
