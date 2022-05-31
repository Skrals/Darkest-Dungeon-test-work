using static UnityEngine.Debug;
using System.Threading.Tasks;

public class Enemy : Unit
{
    public override float Attack()
    {
        return AttackDamage();
    }

    protected  async override void OnHealthUpdate(float health)
    {
        Log($"{gameObject.name} current health {health}");
        if (health <= 0)
        {
            Log($"{gameObject.name} was killed");
            _units._unitsCollection.Remove(this);
            _units._enemyCollection.Remove(this);
            await Task.Delay(3000);
            Destroy(gameObject);
        }
    }
}
