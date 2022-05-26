using UnityEngine;

[RequireComponent(typeof(HealthContainer))]
public abstract class Unit : MonoBehaviour
{
    [SerializeField] private HealthContainer _healthContainer;

    private void Awake() => _healthContainer = GetComponent<HealthContainer>();

}
