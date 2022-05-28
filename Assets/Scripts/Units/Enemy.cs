using UnityEngine;
using static UnityEngine.Debug;

public class Enemy : Unit
{
    public override float Attack()
    {
        return AttackDamage();
    }

    protected override void OnHealthUpdate(float health)
    {
        Log($"{gameObject.name} current health {health}");
        if (health <= 0)
        {
            Destroy(gameObject);
            Log($"{gameObject.name} was killes");
            _units._unitsCollection.Remove(this);
            _units._enemyCollection.Remove(this);
        }
    }

}
