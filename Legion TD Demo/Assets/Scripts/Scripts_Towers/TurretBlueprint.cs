using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // see fields
public class TurretBlueprint // no mono (put into object)
{
    public GameObject prefab;
    public GameObject upgradedPrefab;
    public int cost;
    public int upgradeCost;

    public int GetSellAmount ()
    {
        return cost / 2;
    }
}
