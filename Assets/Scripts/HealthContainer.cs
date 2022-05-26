using UnityEngine;

public class HealthContainer : MonoBehaviour
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _currenthealth;

    private void Start()
    {
        _currenthealth = _maxHealth;
    }
}
