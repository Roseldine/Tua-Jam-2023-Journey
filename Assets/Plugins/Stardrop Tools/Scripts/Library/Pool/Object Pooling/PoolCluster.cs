
namespace StardropTools.Pool
{

    using System.Collections.Generic;
    using UnityEngine;

    public class PoolCluster : MonoBehaviour
    {
#if UNITY_EDITOR
        [TextArea(1, 5)] [SerializeField] string description;
        [SerializeField] List<GameObject> prefabsToPool;
        [SerializeField] bool createPoolsFromPrefabs;
        [Space]
        [SerializeField] bool getPools;
#endif

        [SerializeField] List<Pool> pools;

        public void Populate()
        {
            GetPools();

            for (int i = 0; i < pools.Count; i++)
                pools[i].Populate();
        }



        public PoolItem SpawnFromPool(int poolIndex, Vector3 position, Quaternion rotation, Transform parent, float lifetime = 0)
        {
            PoolItem item = pools[poolIndex].Spawn(position, rotation, parent, lifetime);
            return item;
        }


        public T SpawnFromPool<T>(int poolIndex, Vector3 position, Quaternion rotation, Transform parent, float lifetime = 0)
            => pools[poolIndex].Spawn<T>(position, rotation, parent, lifetime);



        public bool DespawnToPool(int poolIndex, PoolItem item) => pools[poolIndex].Despawn(item);


        public void DespawnAllPools()
        {
            for (int i = 0; i < pools.Count; i++)
                pools[i].DespawnAll();
        }


        public void CreatePool(GameObject prefab)
        {
            GameObject poolObj = Utilities.CreateEmpty("Pool - " + prefab.name, Vector3.zero, transform).gameObject;
            Pool pool = poolObj.AddComponent<Pool>();
            pool.SetPool(prefab, 1, false);
        }



        void GetPools()
        {
            Pool[] childPools = GetComponentsInChildren<Pool>();
            pools = new List<Pool>();

            for (int i = 0; i < childPools.Length; i++)
                pools.Add(childPools[i]);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (getPools)
            {
                GetPools();
                getPools = false;
            }

            if (createPoolsFromPrefabs)
            {
                CreatePoolsFromPrefabs();
                createPoolsFromPrefabs = false;
            }
        }

        void CreatePoolsFromPrefabs()
        {
            if (prefabsToPool.Count == 0)
                return;

            bool exists = false;
            for (int i = 0; i < prefabsToPool.Count; i++)
            {
                GameObject prefab = prefabsToPool[i];

                for (int j = 0; j < pools.Count; j++)
                {
                    if (prefab.name == pools[i].Prefab.name)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists == false)
                    CreatePool(prefab);
            }

            prefabsToPool = new List<GameObject>();
            GetPools();
        }
#endif
    }
}