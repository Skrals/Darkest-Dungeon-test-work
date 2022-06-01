using UnityEngine;

[RequireComponent(typeof(HealthContainer))]
public abstract class Unit : Base
{
    [SerializeField] private HealthContainer _healthContainer;
    [SerializeField] protected UnitsCollection _units;

    [Header("Damage settings")]
    [SerializeField] private Vector2 _randomDamage;
    [SerializeField] private float _damage;

    private void Awake()
    {
        _healthContainer = GetComponent<HealthContainer>();
        _units = FindObjectOfType<UnitsCollection>().GetComponent<UnitsCollection>();
    }

    private void OnEnable() => _healthContainer.OnHealthChange += OnHealthUpdate;
    private void OnDisable() => _healthContainer.OnHealthChange -= OnHealthUpdate;
    protected abstract void OnHealthUpdate(float health);

    protected float AttackDamage()
    {
        _damage = Random.Range(_randomDamage.x, _randomDamage.y);
        return _damage;
    }

    public abstract float Attack();
}
