using UnityEngine;

public class Dropdown : MonoBehaviour
{
    public EntityClass enemyType = EntityClass.None;
    
    public void DropdownValueChange(int index)
    {
        enemyType = (EntityClass)index;
    }
}