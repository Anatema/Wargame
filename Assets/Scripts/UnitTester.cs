using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTester : MonoBehaviour
{
    public Unit unit1;
    public Unit unit2;
    // Start is called before the first frame update
    public void DealDamage()
    {
        unit1.Abilities[0].Invoke(unit1, unit2.Cell);
    }
}
