using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Debug;

public class TurnBaseCombat : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;

    [Header("Attacker and target units")]
    [SerializeField] private Unit _attacker;
    [SerializeField] private Unit _target;

    [Header("Units animation scripts")]
    [SerializeField] private MoveUnits _moveUnits;
    [SerializeField] private Animations _animations;

    [Header("Units collection")]
    [SerializeField] private int _currentUnitNumber;
    [SerializeField] private UnitsCollection _units;

    private List<Unit> _mainList;
    private List<Player> _playerList;
    private List<Enemy> _enemyList;

    [Header("Unit view preset")]
    [SerializeField] private UnitTurn _turnView;
    [SerializeField] private Color _turnColor = new Color(243, 209, 0, 255);
    [SerializeField] private Color _turnColorOff = new Color(243, 209, 0, 0);

    [Header("Turn settings")]
    [SerializeField] private bool _startBattle;
    public bool PlayerTurn { get; private set; }

    [SerializeField] private float _turnDelay;
    [SerializeField] private float _playerPreTurnDelay;

    private void Start()
    {
        _animations = new Animations();

        _turnDelay = 5f;
        _playerPreTurnDelay = 2f;
        PlayerTurn = false;

        _mainList = _units._unitsCollection;
        _playerList = _units._playerCollection;
        _enemyList = _units._enemyCollection;
    }

    public void BattleStart()
    {
        if (_startBattle)
        {
            return;
        }

        _startBattle = true;
        UnitsShuffle(_mainList);
        ChangeAttacker();
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
        TurnViewPointSwitcher();
        StopAllCoroutines();
        NextUnit();
        StartCoroutine(BattleLoop());
    }

    private void TurnViewPointSwitcher()
    {
        Color color = _turnView.GetComponent<Image>().color == _turnColor ? _turnColorOff : _turnColor;
        _turnView.GetComponent<Image>().color = color;
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
            Log($"{attacker.name} {_currentUnitNumber} attaked {target.name} and deal {attacker.Attack()} damage");
            var health = target.gameObject.GetComponent<HealthContainer>();
            health.TakeDamage(attacker.Attack());

            _moveUnits.UnitsChangePositions(attacker, target);

            _animations.AttacksAnimations(attacker, target);
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
            ChangeAttacker();
        }
        else
        {
            _currentUnitNumber = 0;
            UnitsShuffle(_mainList);

            ChangeAttacker();
            Log("Shuffle");
        }
    }

    private void ChangeAttacker()
    {
        _attacker = _mainList[_currentUnitNumber];
    }

    private bool DeadUnits()
    {
        if (_playerList.Count <= 0 || _enemyList.Count <= 0)
        {
            string result = _playerList.Count <= 0 ? "You squad was defeated" : "You won this battle";
            Log(result);

            return true;
        }

        return false;
    }

    private IEnumerator WaitInput()
    {
        yield return new WaitWhile(() => _target == null);
        yield break;
    }

    private IEnumerator BattleLoop()
    {
        System.Random targetIndex = new System.Random();

        while (true)
        {
            _target = null;

            if (DeadUnits())
            {
                yield break;
            }

            if (_currentUnitNumber > _mainList.Count - 1)
            {
                _currentUnitNumber = 0;
                UnitsShuffle(_mainList);

                ChangeAttacker();

                continue;
            }

            _turnView = _attacker.GetComponentInChildren<UnitTurn>();

            if (_mainList[_currentUnitNumber].gameObject != null && _mainList[_currentUnitNumber].gameObject.GetComponent<Player>())
            {
                yield return new WaitForSeconds(_playerPreTurnDelay);

                TurnViewPointSwitcher();
                PlayerTurn = true;

                yield return StartCoroutine(WaitInput());

                if (_target != null && CompairUnits(_attacker, _target))
                {
                    Attack(_attacker, _target);
                    PlayerTurn = false;

                    yield return new WaitForSeconds(_turnDelay);
                }

                TurnViewPointSwitcher();
            }
            else
            {
                _target = _mainList[targetIndex.Next(_mainList.Count)];

                if (_target == null || _target != gameObject.GetComponent<Player>())//если не нашли игрока делаем дополнительные итерации
                {
                    for (int i = 0; i < _mainList.Count; i++)
                    {
                        var target = _mainList[targetIndex.Next(_mainList.Count)];

                        if (target != null && target.gameObject.GetComponent<Player>())
                        {
                            _target = target;
                            break;
                        }
                    }
                }

                if (_target != null && CompairUnits(_attacker, _target))
                {
                    TurnViewPointSwitcher();

                    Attack(_attacker, _target);

                    yield return new WaitForSeconds(_turnDelay);
                }

                TurnViewPointSwitcher();
            }

            NextUnit();

            yield return new WaitForEndOfFrame();
        }
    }
}
