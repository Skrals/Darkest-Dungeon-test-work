using UnityEngine;

public class PlayerTargetChanger : MonoBehaviour
{
    [SerializeField] private Enemy _enemyTarget;
    [SerializeField] private Camera _camera;
    [SerializeField] private TurnBaseCombat _turnBaseCombat;

    [SerializeField]private bool _attackButtonClick;

    private void Start()
    {
        _turnBaseCombat = gameObject.GetComponent<TurnBaseCombat>();
    }

    private void Update()
    {
        if(!_turnBaseCombat.PlayerTurn)
        {
            _attackButtonClick = false;
            return;
        }

        if(Input.GetMouseButtonDown(0) && _attackButtonClick)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                _enemyTarget = hit.collider.gameObject.GetComponent<Enemy>();
                _turnBaseCombat.GetTarget(_enemyTarget);
                _attackButtonClick = false;
            }
        }

    }

    public void AttackButton()
    { 
        _attackButtonClick = true;
    }
}
