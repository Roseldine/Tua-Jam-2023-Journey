
namespace StardropTools.Pool
{

    using System.Collections.Generic;
    using UnityEngine;

    public class Pool : MonoBehaviour
    {
        [NaughtyAttributes.ShowAssetPreview] [SerializeField] GameObject prefab;
        [SerializeField] int capacity = 0;
        [SerializeField] bool debug;
        [Space]
        [Tooltip("List of all instantiated items")]
        [SerializeField] List<PoolItem> pool;

        [Tooltip("List of all spawned items")]
        [SerializeField] List<PoolItem> activeCache;

        Transform self;
        bool isPopulated;

        public GameObject Prefab => prefab;
        public int PoolCount => pool.Count;
        public int ActiveCount => activeCache.Count;


        public void SetPrefab(GameObject prefab) => this.prefab = prefab;
        public void SetCapacity(int capacity) => this.capacity = capacity;


        public void SetPool(GameObject prefab, int capacity, bool shouldPopulate)
        {
            isPopulated = false;

            SetPrefab(prefab);
            SetCapacity(capacity);

            if (shouldPopulate)
                Populate();
        }


        /// <summary>
        /// Create as many items as set in "capacity" and stores them in a list
        /// </summary>
        public void Populate()
        {
            if (isPopulated)
                return;

            if (prefab.Equals(null))
                Debug.Log($"Pool: {name}, <color=red>NO PREFAB</color>");

            if (capacity <= 0)
                Debug.Log($"Pool: {name}, <color=orange>NO CAPACITY</color>");

            self = transform;

            pool = new List<PoolItem>();
            activeCache = new List<PoolItem>();

            for (int i = 0; i < capacity; i++)
                CreateItem();

            isPopulated = true;
        }

        /// <summary>
        /// Creates a PoolItem obj and sotres it into cache
        /// </summary>
        /// <returns></returns>
        PoolItem CreateItem()
        {
            if (prefab.Equals(null))
                return null;

            GameObject obj = Instantiate(prefab, self);
            obj.name += $" - {pool.Count}";

            PoolItem item = new PoolItem(obj, this);
            item.SetActive(false);

            pool.Add(item);

            return item;
        }


        /// <summary>
        /// Loops through instantiated items and tries to find an Innactive one. If not found, creates a new one and adds it to Pool list
        /// </summary>
        /// <returns></returns>
        PoolItem FindInnactiveItem()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].IsActive == false)
                    return pool[i];
            }

            // if we get here, All items in pool are active
            // we must create more!
            return CreateItem();
        }

        public PoolItem Spawn(Vector3 position, Quaternion rotation, Transform parent, float lifetime = 0)
        {
            PoolItem item = FindInnactiveItem();
            item.SetPositionRotationAndParent(position, rotation, parent);
            item.SetActive(true);
            activeCache.Add(item);

            if (lifetime > 0)
                SetItemLifetime(item, lifetime);

            item.OnSpawn();
            return item;
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation, Transform parent, float lifetime = 0)
        {
            T item = Spawn(position, rotation, parent, lifetime).ItemGameObject.GetComponent<T>();
            return item;
        }

        public bool Despawn(PoolItem item)
        {
            if (item.IsFromPool(this) && activeCache.Contains(item))
            {
                item.OnDespawn();
                item.SetActive(false);
                activeCache.Remove(item);

                return true;
            }

            else
            {
                if (debug)
                    Debug.Log($"Object: {item.ItemGameObject.name}, didn't come from pool: {name}");
                return false;
            }
        }

        public void DespawnAll()
        {
            for (int i = 0; i < activeCache.Count; i++)
                Despawn(activeCache[i]);
            //activeCache[i].Despawn();
        }


        void SetItemLifetime(PoolItem item, float lifetime)
        {
            Coroutine lifetimeCR = StartCoroutine(item.LifetimeCR(lifetime));
            item.SetLifetimeCoroutine(lifetimeCR);
        }

        public void StopItemLifetimeCoroutine(Coroutine lifetimeCR) => StopCoroutine(lifetimeCR);
    }
}