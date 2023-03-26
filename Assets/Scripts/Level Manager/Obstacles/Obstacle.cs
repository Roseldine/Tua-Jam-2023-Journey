
using UnityEngine;
using StardropTools;
using StardropTools.Pool;

public class Obstacle : BaseObject, IPoolable
{
    [SerializeField] ObstacleType obstacleType;
    [NaughtyAttributes.Expandable] [SerializeField] ObstacleSettingsSO settings;
    [SerializeField] Transform      model;
    [SerializeField] MeshFilter     meshFilter;
    [SerializeField] MeshRenderer   meshRenderer;
    [SerializeField] Vector3 scale  = Vector3.one;
    [SerializeField] bool uniformScale = false;


    #region Poolable
    PoolItem poolItem;
    public void SetPoolItem(PoolItem poolItem) => this.poolItem = poolItem;
    public void Despawn() => poolItem.Despawn();

    public void OnSpawn()
    {
        Initialize();
    }

    public void OnDespawn()
    {

    }
    #endregion



    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = "Obstacle - " + obstacleType;

        if (transform.childCount > 0)
        {
            model = transform.GetChild(0);
            if (uniformScale == false)
                model.localScale = scale;
            else
                model.localScale = Vector3.one * scale.x;

            meshFilter = GetComponentInChildren<MeshFilter>();
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
    }
}
