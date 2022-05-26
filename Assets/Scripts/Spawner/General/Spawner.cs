using UnityEngine;
using static UnitsCollection;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPointsPrefab;
    [SerializeField] private Enemy[] _enemysCollection;
    [SerializeField] private Player[] _playersCollection;

    [SerializeField] private SpawnPoint[] _spawnPoints;

    private void Start()
    {
        _spawnPoints = _spawnPointsPrefab.GetComponentsInChildren<SpawnPoint>();
        Spawn();
    }

    private void Spawn()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            UnitSpawn(_playersCollection[0], spawnPoint);
        }

        Destroy(_spawnPointsPrefab);
        Debug.Log(_unitsCollection.Count);
    }

    private void UnitSpawn(Unit unitTemplate, SpawnPoint spawnPoint)
    {
        var unit = Instantiate(unitTemplate.transform, spawnPoint.transform);
        unit.transform.parent = null;
        _unitsCollection.Add(unit.gameObject);
    }

}
