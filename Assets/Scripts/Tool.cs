using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField]
    private ToolTypes type;
    [SerializeField]
    private ToolMaterials material;
    public int damageToEnemy;
    public int damageToBlock;

    void Start()
    {
        damageToEnemy = (int)type * (int)material;

        switch(type)
        {
            case ToolTypes.Pickaxe:
                damageToBlock = 4 * (int)material;
                break;
            case ToolTypes.Sword:
                damageToBlock = 1 * (int)material;
                break;
        }
    }

    void Update()
    {
        
    }
}
