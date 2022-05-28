using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitsCollection;

public class TurnBaseCombat : MonoBehaviour
{
    void Start()
    {
        UnitsShuffle(_unitsCollection);
    }


    private void UnitsShuffle(List<Unit> units)
    {
        System.Random random = new System.Random();

        for (int i = 0; i < units.Count; i++)
        {
            var tmp = units[i];
            units.RemoveAt(i);
            units.Insert(random.Next(units.Count), tmp);
        }
    }

    private void Attack(Unit attacker, Unit target)
    {
        if (attacker != null && target != null)
        {
            if (target.TryGetComponent(out HealthContainer healthContainer))
            {
                healthContainer.TakeDamage(attacker.Attack());
            }
        }
    }
}
