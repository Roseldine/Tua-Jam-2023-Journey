
using UnityEngine;
using StardropTools;
using StardropTools.Tween;
using StardropTools.Pool;

public class Collectable : BaseObject, IPoolable
{
    [Header("Value")]
    [SerializeField] protected float collectValue;

    [Header("Effect")]
    [SerializeField] protected int effectID = -1;
    [SerializeField] protected float effectLifetime = .5f;
    [SerializeField] protected Transform effectSpawnPoint;

    public float Value => collectValue;


    #region Poolable
    PoolItem poolItem;

    public void SetPoolItem(PoolItem poolItem) => this.poolItem = poolItem;

    public void Despawn() => poolItem.Despawn();

    public void OnSpawn()
    {
        Initialize();
        TweenManager.StartTweenComponents(tweenComponents);
    }

    public void OnDespawn()
    {
        TweenManager.StopTweenComponents(tweenComponents);
    }

    #endregion // Poolable

    [SerializeField] TweenComponent[] tweenComponents;

    public void Collect()
    {
        GameManager.OnCollect?.Invoke(collectValue);

        if (effectID >= 0)
            PoolManager.Instance.SpawnEffect(effectID, effectSpawnPoint.position, effectSpawnPoint.rotation, null, effectLifetime);

        Despawn();
    }
}
