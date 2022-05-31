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
    public bool PlayerTurn { get; private set; }

    [SerializeField] private bool _startBattle;

    private void Start()
    {
        PlayerTurn = false;
        _animations = new Animations();
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
        _turnView.GetComponent<Image>().color = _turnColorOff;// скрываем метку хода
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
            _attacker = _mainList[_currentUnitNumber];
        }
        else
        {
            _currentUnitNumber = 0;
            UnitsShuffle(_mainList);

            _attacker = _mainList[_currentUnitNumber];
            Log("Shuffle");
        }
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

                _attacker = _mainList[_currentUnitNumber];

                continue;
            }

            _turnView = _attacker.GetComponentInChildren<UnitTurn>();

            if (_mainList[_currentUnitNumber].gameObject != null && _mainList[_currentUnitNumber].gameObject.GetComponent<Player>())
            {
                yield return new WaitForSeconds(2);

                _turnView.GetComponent<Image>().color = _turnColor;// показываем метку хода
                PlayerTurn = true;

                yield return StartCoroutine(WaitInput());

                if (_target != null && CompairUnits(_attacker, _target))
                {
                    Attack(_attacker, _target);
                    PlayerTurn = false;

                    yield return new WaitForSeconds(5);
                }

                _turnView.GetComponent<Image>().color = _turnColorOff;// скрываем метку хода
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
                    _turnView.GetComponent<Image>().color = _turnColor;// показываем метку хода

                    Attack(_attacker, _target);

                    yield return new WaitForSeconds(5);
                }

                _turnView.GetComponent<Image>().color = _turnColorOff;// скрываем метку хода
            }

            NextUnit();

            yield return new WaitForEndOfFrame();
        }
    }
}
