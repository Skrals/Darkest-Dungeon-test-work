using UnityEngine;

[RequireComponent(typeof(HealthContainer))]
public abstract class Unit : MonoBehaviour
{
    [SerializeField] private HealthContainer _healthContainer;

    private void Awake() => _healthContainer = GetComponent<HealthContainer>();

    private void OnEnable() => _healthContainer.OnHealthChange += OnHealthUpdate;

    private void OnDisable() => _healthContainer.OnHealthChange -= OnHealthUpdate;

    protected abstract void OnHealthUpdate(float health);

    public abstract float Attack();

}
