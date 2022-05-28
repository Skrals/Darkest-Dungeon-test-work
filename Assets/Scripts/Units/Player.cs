using UnityEngine;
using static UnityEngine.Debug;

public class Player : Unit
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
            Log($"{gameObject.name} was killes");
            Destroy(gameObject);
        }
    }
}
