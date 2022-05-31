using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPointsPrefab;
    [SerializeField] private Enemy[] _enemysArray;
    [SerializeField] private Player[] _playersArray;

    [SerializeField] private SpawnPoint[] _spawnPoints;

    [SerializeField] private UnitsCollection _units;

    private void Start()
    {
        _spawnPoints = _spawnPointsPrefab.GetComponentsInChildren<SpawnPoint>();
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (i >= _spawnPoints.Length / 2)
            {
                UnitChanger(_playersArray, i);
            }
            else
            {
                UnitChanger(_enemysArray, i);
            }
        }
    }

    private void UnitChanger(Unit[] units, int index)
    {
        int random;
        random = Random.Range(0, units.Length);
        UnitSpawn(units[random], _spawnPoints[index]);
    }

    private void UnitSpawn(Unit unitTemplate, SpawnPoint spawnPoint)
    {
        var unit = Instantiate(unitTemplate.transform, spawnPoint.transform);

        UnitAddInCollections(unit.gameObject.GetComponent<Unit>());
    }

    private void UnitAddInCollections(Unit unit)
    {
        _units._unitsCollection.Add(unit);

        if (unit.GetType() == typeof(Enemy))
        {
            _units._enemyCollection.Add((Enemy)unit);
        }
        else if (unit.GetType() == typeof(Player))
        {
            _units._playerCollection.Add((Player)unit);
        }
    }

    public SpawnPoint[] GetSpawnPoints()
    {
        return _spawnPoints;
    }
}
