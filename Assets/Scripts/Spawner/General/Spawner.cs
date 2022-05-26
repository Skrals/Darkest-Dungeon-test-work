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
        int random;

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            random = Random.Range(0, 2);
            Debug.Log(random);
            if (i >= _spawnPoints.Length / 2)
            {
                UnitSpawn(_playersCollection[random], _spawnPoints[i]);
            }
            else
            {
                UnitSpawn(_enemysCollection[random], _spawnPoints[i]);
            }
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
