using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

public class TurnBaseCombat : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Unit _attacker;
    [SerializeField] private Unit _target;
    [SerializeField] private MoveUnits _moveUnits;
    [SerializeField] private int _currentUnitNumber;

    [SerializeField] private UnitsCollection _units;

    private List<Unit> _mainList;
    private List<Player> _playerList;
    private List<Enemy> _enemyList;

    [SerializeField] private int _deadEnemy;
    [SerializeField] private int _deadPlayer;

    public bool PlayerTurn { get; private set; }

    private void Start()
    {
        PlayerTurn = false;
        _mainList = _units._unitsCollection;
        _playerList = _units._playerCollection;
        _enemyList = _units._enemyCollection;
    }

    public void BattleStart()
    {
        UnitsShuffle(_mainList);
        _attacker = _mainList[_currentUnitNumber];
        StartCoroutine(BattleLoop());
    }

    public void GetTarget(Unit target)
    {
        _target = target;
        PlayerTurn = false;
    }

    public void Skipping()
    {
        if (!PlayerTurn)
        {
            return;
        }
        PlayerTurn = false;
        StopAllCoroutines();
        NextUnit();
        StartCoroutine(BattleLoop());
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
            Log($"{attacker} {_currentUnitNumber} attaked {target}");
            var health = target.gameObject.GetComponent<HealthContainer>();
            health.TakeDamage(attacker.Attack());

           _moveUnits.UnitsChangePositions(attacker, target);
        }
    }

    private bool CompairUnits(Unit attacker, Unit target)
    {
        if (attacker.GetType() != target.GetType())
        {
            return true;
        }
        return false;
    }

    private void NextUnit()
    {
        _currentUnitNumber++;

        if (_currentUnitNumber <= _mainList.Count - 1)
        {
            _attacker = _mainList[_currentUnitNumber];
        }
        else
        {
            _currentUnitNumber = 0;

            UnitsShuffle(_mainList);
            Log("Shuffle");
        }
    }

    private bool DeadUnits()
    {
        _deadEnemy = 0;
        _deadPlayer = 0;

        foreach (var unit in _enemyList)
        {
            if (unit == null)
                _deadEnemy++;
        }

        foreach (var unit in _playerList)
        {
            if (unit == null)
                _deadPlayer++;
        }

        if (_deadPlayer >= _playerList.Count || _deadEnemy >= _enemyList.Count)
        {
            return true;
        }

        return false;
    }

    private IEnumerator WaitInput()
    {
        yield return new WaitWhile(() => _target == null);
    }

    private IEnumerator BattleLoop()//Todo анимации атаки и перемещение цели и атакера вперед
    {
        System.Random targetIndex = new System.Random();

        while (true)
        {
            _target = null;

            if (DeadUnits())
            {
                yield break;
            }

            if (_attacker == null)
            {
                NextUnit();
                continue;
            }

            if (_mainList[_currentUnitNumber].gameObject.GetComponent<Player>() && _mainList[_currentUnitNumber].gameObject != null)
            {
                yield return new WaitForSeconds(2);

                PlayerTurn = true;

                yield return StartCoroutine(WaitInput());

                if (_target != null && CompairUnits(_attacker, _target))
                {
                    Attack(_attacker, _target);
                    PlayerTurn = false;
                    yield return new WaitForSeconds(2);
                }
            }
            else
            {
                _target = _mainList[targetIndex.Next(_mainList.Count)];

                if (_target == null)
                {
                    foreach (Unit player in _mainList)
                    {
                        if (player != null && player.gameObject.GetComponent<Player>())
                        {
                            _target = player;
                        }
                    }
                }

                if (_target != null && CompairUnits(_attacker, _target))
                {
                    Attack(_attacker, _target);
                    yield return new WaitForSeconds(2);
                }
            }

            NextUnit();

            yield return new WaitForEndOfFrame();
        }
    }

}
