using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class HealthContainer : Base
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _currenthealth;
    [SerializeField] private Slider _hpView;

    public event UnityAction<float> OnHealthChange;

    private void Start()
    {
        _currenthealth = _maxHealth;
        _hpView = GetComponentInChildren<Slider>();
        _hpView.maxValue = _maxHealth;
        _hpView.value = _currenthealth;
    }

    public void TakeDamage(float damage)
    {
        _currenthealth -= damage;
        OnHealthChange?.Invoke(_currenthealth);
        StartCoroutine(ViewUpdater());
    }

    public float CurrentHealth()
    {
        return _currenthealth;
    }

    private IEnumerator ViewUpdater()
    {
        yield return new WaitForSeconds(2);
        _hpView.value = _currenthealth;
        yield break;
    }
}
