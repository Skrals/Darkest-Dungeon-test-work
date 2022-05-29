using UnityEngine;

public class PlayerTargetChanger : MonoBehaviour
{
    [SerializeField] private Enemy _enemyTarget;
    [SerializeField] private Camera _camera;
    [SerializeField] private TurnBaseCombat _turnBaseCombat;

    private void Start()
    {
        _turnBaseCombat = gameObject.GetComponent<TurnBaseCombat>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _turnBaseCombat.PlayerTurn)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                _enemyTarget = hit.collider.gameObject.GetComponent<Enemy>();
            }
        }
    }
}
