using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

public class TurnBaseCombat : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Unit _attacker;
    [SerializeField] private Unit _target;
    [SerializeField] private int _currentUnitNumber;

    [SerializeField] private UnitsCollection _units;

    public void BattleStart()
    {
        UnitsShuffle(_units._unitsCollection);
        _attacker = _units._unitsCollection[_currentUnitNumber];
        StartCoroutine(BattleLoop());
    }

    public void Skipping()
    {
        NextUnit();
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
            var health = target.gameObject.GetComponent<HealthContainer>();
            health.TakeDamage(attacker.Attack());
        }
    }

    private void NextUnit()
    {
        _currentUnitNumber++;
        _attacker = _units._unitsCollection[_currentUnitNumber];
    }


    private IEnumerator BattleLoop()
    {
        System.Random targetIndex = new System.Random();

        while (true)
        {
            if (_units._playerCollection.Count <= 0 || _units._enemyCollection.Count <= 0)
            {
                StopCoroutine(BattleLoop());
            }
            else if (_currentUnitNumber >= _units._unitsCollection.Count-1)
            {
                _currentUnitNumber = 0;
                UnitsShuffle(_units._unitsCollection);
                Log("Shuffle");
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }

            if(_units._unitsCollection[_currentUnitNumber].GetType() == typeof(Player))
            {
                _target = _units._enemyCollection[targetIndex.Next(_units._enemyCollection.Count)];
                Attack(_attacker, _target);
                NextUnit();
                Log($"{_attacker} {_currentUnitNumber} attaked {_target}");
              //  yield return new WaitForSeconds(5);
            }
            else if (_units._unitsCollection[_currentUnitNumber].GetType() == typeof(Enemy))
            {
                _target = _units._playerCollection[targetIndex.Next(_units._playerCollection.Count)];
                Attack(_attacker, _target);
                NextUnit();
                Log($"{_attacker} {_currentUnitNumber} attaked {_target}");
                //yield return new WaitForSeconds(5);
            }
        }
    }


    private void DebugInspectUnits()
    {
        foreach (var item in _units._unitsCollection)
        {
            Log($"{item.name}");
            Log($"{item.gameObject.GetComponent<HealthContainer>().CurrentHealth()}");
        }
    }

    private void DebugInspectSplitCollections()
    {
        Log($"Enemys {_units._enemyCollection.Count}");
        Log("");
        Log($"Players {_units._playerCollection.Count}");
    }
}
