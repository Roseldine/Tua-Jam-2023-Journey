using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StardropTools;
using StardropTools.Pool;
using StardropTools.Grid;

public class Platform : BaseObject, IPoolable
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] PointGrid grid;
    [SerializeField] BoxColliderDetector playerDetector;
    [SerializeField] List<Obstacle> obstacles = new List<Obstacle>();
    [SerializeField] Transform[] groundPlanes;

    public Vector3 SpawnPosition => spawnPoint.position;


    #region Poolable
    PoolItem poolItem;
    public void SetPoolItem(PoolItem poolItem) => this.poolItem = poolItem;
    public void Despawn() => poolItem.Despawn();

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
        for (int i = 0; i < obstacles.Count; i++)
            obstacles[i].Despawn();

        obstacles = new List<Obstacle>();
    }
    #endregion // poolable


    protected override void Start()
    {
        base.Start();
        playerDetector.OnContactStart.AddListener(() => GameManager.OnPlayerEnterPlatform?.Invoke());
    }

    public void ScanForPlayer() => playerDetector.SearchForColliders();

    public Vector3[] GetRandomGridPositions(int amount) => grid.GetRandomPoints(amount).ToArray();

    public int GetRandomObstacleType()
    {
        int typeLength = System.Enum.GetNames(typeof(ObstacleType)).Length;
        return Random.Range(0, typeLength);
    }

    public void SpawnObstacles(int amount)
    {
        List<Vector3> randomPositions = grid.GetRandomPoints(amount);

        for (int i = 0; i < randomPositions.Count; i++)
        {
            Vector3 spawnPos = transform.position + randomPositions[i];
            Obstacle obstacle = PoolManager.Instance.SpawnObstacle(GetRandomObstacleType(), spawnPos, selfTransform);
        }

        float[] rotations = { -90, 90 };
        float[] scales = { 1.5f, -1.5f };
        for (int i = 0; i < groundPlanes.Length; i++)
        {
            groundPlanes[i].eulerAngles = Vector3.up * rotations.GetRandom();
            Vector3 scale = new Vector3(1, 1, scales.GetRandom());
            groundPlanes[i].localScale = scale;
        }
    }

    public void SpawnCollectibles(int amount)
    {
        List<Vector3> randomPositions = grid.GetRandomPoints(amount);

        for (int i = 0; i < randomPositions.Count; i++)
        {
            Vector3 spawnPos = transform.position + randomPositions[i];
            Collectable collectable = PoolManager.Instance.SpawnCollectable(0, spawnPos, selfTransform);
        }
    }
}
