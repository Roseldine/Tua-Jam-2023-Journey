using StardropTools.Pool;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] PoolCluster pool_Platforms;
    [SerializeField] PoolCluster pool_Obstacles;
    [SerializeField] PoolCluster pool_Collectabale;
    [SerializeField] PoolCluster pool_Effects;

    protected override void Awake()
    {
        base.Awake();

        pool_Platforms.Populate();
        pool_Obstacles.Populate();
        pool_Collectabale.Populate();
        pool_Effects.Populate();
    }

    public Platform SpawnPlaform(Vector3 position, Transform parent)
        => pool_Platforms.SpawnFromPool<Platform>(0, position, Quaternion.identity, parent);

    public void ClearPlatforms() => pool_Platforms.DespawnAllPools();


    public Obstacle SpawnObstacle(int poolID, Vector3 position, Transform parent)
        => pool_Obstacles.SpawnFromPool<Obstacle>(poolID, position, Quaternion.identity, parent);

    public void ClearObstacles() => pool_Obstacles.DespawnAllPools();

    
    public Collectable SpawnCollectable(int poolID, Vector3 position, Transform parent)
        => pool_Collectabale.SpawnFromPool<Collectable>(poolID, position, Quaternion.identity, parent);

    public void ClearCollectables() => pool_Collectabale.DespawnAllPools();


    public Transform SpawnEffect(int effectID, Vector3 position, Quaternion rotation, Transform parent, float lifeTime)
        => pool_Effects.SpawnFromPool<Transform>(effectID, position, rotation, parent, lifeTime);

    public void ClearEffects() => pool_Effects.DespawnAllPools();
}