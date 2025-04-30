using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Healing,
    Bullet
}


public class Item : PoolableMono
{
    public override void Init()
    {

    }

    [SerializeField]
    public ItemType _itemType;
}
