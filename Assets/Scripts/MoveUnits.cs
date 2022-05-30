using UnityEngine;

public class MoveUnits : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;

    private void Start()
    {
        _spawner = GetComponent<Spawner>();
    }

    private int SearchIndex(Unit unit)
    {
        var pointsArray = _spawner.GetSpawnPoints();
        int index = 0;

        foreach (var point in pointsArray)
        {
            if (unit.gameObject.GetComponentInParent<SpawnPoint>() == point)
            {
                return index;
            }
            index++;
        }
        return 0;
    }

    private void MoveUnitsPlayer(int start, int end, SpawnPoint[] points)//Todo - сделать плавное перемещение - добавить перемещение противников
    {
        Vector3 tmp = Vector3.zero;
        SpawnPoint point;
        for (int i = start; i > end; i--)
        {
            var next = points[i].transform.position;
            tmp = points[i - 1].transform.position;

            var currentPoint = points[i];
            point = points[i - 1];

            points[i - 1].transform.position = next;
            points[i].transform.position = tmp;

            points[i - 1] = currentPoint;
            points[i] = point;
        }
    }

    public void UnitsChangePositions(Unit attacker, Unit target) //c 0 по 3 противник с 4 по 7 игрок
    {
        var PlayerIndex = 0;
        var EnemyIndex = 0;

        if (attacker.gameObject.GetComponent<Player>())
        {
            PlayerIndex = SearchIndex(attacker);
            EnemyIndex = SearchIndex(target);

            MoveUnitsPlayer(PlayerIndex, 4, _spawner.GetSpawnPoints());

        }
        else
        {
            PlayerIndex = SearchIndex(target);
            EnemyIndex = SearchIndex(attacker);

            MoveUnitsPlayer(PlayerIndex, 4, _spawner.GetSpawnPoints());
        }
    }
}
