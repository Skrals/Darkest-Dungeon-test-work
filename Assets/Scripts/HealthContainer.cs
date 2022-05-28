using UnityEngine;
using UnityEngine.Events;

public class HealthContainer : MonoBehaviour
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _currenthealth;

    public event UnityAction<float> OnHealthChange;

    private void Start()
    {
        _currenthealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currenthealth -= damage;
        OnHealthChange?.Invoke(_currenthealth);
    }
    
    
    public float CurrentHealth()
    {
        return _currenthealth; 
    }
}
