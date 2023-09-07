using System;
using UnityEngine;
using System.Collections.Generic;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace TW.Utility.DesignPattern
{
    public static class SimplePool
    {
        /// <summary>
        /// The Pool class represents the pool for a particular prefab.
        /// </summary>
        public class Pool
        {
            // We append an id to the name of anything we instantiate.
            // This is purely cosmetic.
            private int m_NextId = 1;

            // The structure containing our inactive objects.
            // Why a Stack and not a List? Because we'll never need to
            // pluck an object from the start or middle of the array.
            // We'll always just grab the last one, which eliminates
            // any need to shuffle the objects around in memory.
            private readonly Queue<GameObject> m_Inactive;
            //A Hashset which contains all GetInstanceIDs from the instantiated GameObjects 
            //so we know which GameObject is a member of this pool.
            public readonly HashSet<int> m_MemberIDs;


            // The prefab that we are pooling
            private readonly GameObject m_Prefab;

            public int StackCount => m_Inactive.Count;

            // Constructor
            public Pool(GameObject prefab, int initialQty)
            {
                m_Prefab = prefab;
                // If Stack uses a linked list internally, then this
                // whole initialQty thing is a placebo that we could
                // strip out for more minimal code. But it can't *hurt*.
                m_Inactive = new Queue<GameObject>(initialQty);
                m_MemberIDs = new HashSet<int>();
            }

            public void Preload(int initialQty, Transform parent = null)
            {
                for (int i = 0; i < initialQty; i++)
                {
                    // instantiate a whole new object.
                    GameObject obj = Object.Instantiate(m_Prefab, parent);
                    obj.name = $"{m_Prefab.name} ({m_NextId++})";

                    // Add the unique GameObject ID to our MemberHashset so we know this GO belongs to us.
                    m_MemberIDs.Add(obj.GetInstanceID());

                    obj.SetActive(false);

                    m_Inactive.Enqueue(obj);
                }
            }

            // Spawn an object from our pool
            public GameObject Spawn(Vector3 pos, Quaternion rot)
            {
                while (true)
                {
                    GameObject obj;
                    if (m_Inactive.Count == 0)
                    {
                        // We don't have an object in our pool, so we
                        // instantiate a whole new object.
                        obj = Object.Instantiate(m_Prefab, pos, rot);
                        obj.name = $"{m_Prefab.name} ({m_NextId++})";

                        // Add the unique GameObject ID to our MemberHashset so we know this GO belongs to us.
                        m_MemberIDs.Add(obj.GetInstanceID());
                    }
                    else
                    {
                        // Grab the last object in the inactive array
                        obj = m_Inactive.Dequeue();

                        if (obj == null)
                        {
                            // The inactive object we expected to find no longer exists.
                            // The most likely causes are:
                            //   - Someone calling Destroy() on our object
                            //   - A scene change (which will destroy all our objects).
                            //     NOTE: This could be prevented with a DontDestroyOnLoad
                            //	   if you really don't want this.
                            // No worries -- we'll just try the next one in our sequence.

                            continue;
                        }
                    }
                    obj.transform.SetPositionAndRotation(pos, rot);
                    obj.SetActive(true);
                    return obj;
                }
            }
            // Spawn an object from our pool
            public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent)
            {
                while (true)
                {
                    GameObject obj;
                    if (m_Inactive.Count == 0)
                    {
                        // We don't have an object in our pool, so we
                        // instantiate a whole new object.
                        obj = Object.Instantiate(m_Prefab, pos, rot, parent);
                        obj.name = $"{m_Prefab.name} ({m_NextId++})";

                        // Add the unique GameObject ID to our MemberHashset so we know this GO belongs to us.
                        m_MemberIDs.Add(obj.GetInstanceID());
                    }
                    else
                    {
                        // Grab the last object in the inactive array
                        obj = m_Inactive.Dequeue();

                        if (obj == null)
                        {
                            // The inactive object we expected to find no longer exists.
                            // The most likely causes are:
                            //   - Someone calling Destroy() on our object
                            //   - A scene change (which will destroy all our objects).
                            //     NOTE: This could be prevented with a DontDestroyOnLoad
                            //	   if you really don't want this.
                            // No worries -- we'll just try the next one in our sequence.

                            continue;
                        }
                    }
                    obj.transform.SetPositionAndRotation(pos, rot);
                    obj.GetCachedTransform().SetParent(parent);
                    obj.SetActive(true);
                    return obj;
                }
            }

            public T Spawn<T>(Vector3 pos, Quaternion rot) where T : Component
            {
                return Spawn(pos, rot).GetCachedComponent<T>();
            }
            public T Spawn<T>(Vector3 pos, Quaternion rot, Transform parent) where T : Component
            {
                return Spawn(pos, rot, parent).GetCachedComponent<T>();
            }

            //public void ManuelPush(GameObject obj)
            //{
            //    inactive.Push(obj);
            //}
            // Return an object to the inactive pool.
            public void DeSpawnGameObject(GameObject obj)
            {
                if (!obj.activeSelf)
                    return;
                obj.SetActive(false);
                // Since Stack doesn't have a Capacity member, we can't control
                // the growth factor if it does have to expand an internal array.
                // On the other hand, it might simply be using a linked list 
                // internally.  But then, why does it allow us to specify a size
                // in the constructor? Maybe it's a placebo? Stack is weird.
                m_Inactive.Enqueue(obj);
            }
        }

        // You can avoid resizing of the Stack's internal data by
        // setting this to a number equal to or greater to what you
        // expect most of your pool sizes to be.
        // Note, you can also use Preload() to set the initial size
        // of a pool -- this can be handy if only some of your pools
        // are going to be exceptionally large (for example, your bullets.)
        private const int DefaultPoolSize = 10;
        // Dictionary to store cached transforms for GameObjects
        private static readonly Dictionary<GameObject, Transform> CachedTransforms = new Dictionary<GameObject, Transform>();
        // Dictionary to store cached components for GameObjects
        private static readonly Dictionary<GameObject, Dictionary<Type, Component>> CachedComponents = new Dictionary<GameObject, Dictionary<Type, Component>>();

        private static Transform sRoot = null;


        // All of our pools
        private static Dictionary<int, Pool> pools;

        /// <summary>
        /// Initialize our dictionary.
        /// </summary>
        private static void Init(GameObject prefab = null, int qty = DefaultPoolSize)
        {
            if (pools == null)
                pools = new Dictionary<int, Pool>();

            if (prefab != null)
            {
                //changed from (prefab, Pool) to (int, Pool) which should be faster if we have 
                //many different prefabs.
                var prefabID = prefab.GetInstanceID();
                if (!pools.ContainsKey(prefabID))
                    pools[prefabID] = new Pool(prefab, qty);
            }
        }

        public static void PoolPreLoad(GameObject prefab, int qty, Transform newParent = null)
        {
            Init(prefab, 1);
            pools[prefab.GetInstanceID()].Preload(qty, newParent);
        }

        /// <summary>
        /// If you want to preload a few copies of an object at the start
        /// of a scene, you can use this. Really not needed unless you're
        /// going to go from zero instances to 100+ very quickly.
        /// Could technically be optimized more, but in practice the
        /// Spawn/DeSpawnGameObject sequence is going to be pretty darn quick and
        /// this avoids code duplication.
        /// </summary>
        public static GameObject[] Preload(GameObject prefab, int qty = 1, Transform newParent = null)
        {
            Init(prefab, qty);
            // Make an array to grab the objects we're about to pre-spawn.
            var obs = new GameObject[qty];
            for (int i = 0; i < qty; i++)
            {
                obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
                if (newParent != null)
                    obs[i].transform.SetParent(newParent);
            }

            // Now despawn them all.
            for (int i = 0; i < qty; i++)
                DeSpawn(obs[i]);
            return obs;
        }

        /// <summary>
        /// Spawns a copy of the specified prefab (instantiating one if required).
        /// NOTE: Remember that Awake() or Start() will only run on the very first
        /// spawn and that member variables won't get reset.  OnEnable will run
        /// after spawning -- but remember that toggling IsActive will also
        /// call that function.
        /// </summary>
        /// 
        public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            Init(prefab);

            return pools[prefab.GetInstanceID()].Spawn(pos, rot);
        }

        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        public static T Spawn<T>(T prefab) where T : Component
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity);
        }
        public static T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public static T Spawn<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component
        {
            Init(prefab.gameObject);
            return pools[prefab.gameObject.GetInstanceID()].Spawn<T>(pos, rot);
        }
        public static T Spawn<T>(T prefab, Vector3 pos, Quaternion rot, Transform parent) where T : Component
        {
            Init(prefab.gameObject);
            return pools[prefab.gameObject.GetInstanceID()].Spawn<T>(pos, rot, parent);
        }

        /// <summary>
        /// DeSpawnGameObject the specified gameobject back into its pool.
        /// </summary>
        public static void DeSpawn(GameObject obj)
        {
            Pool p = null;
            foreach (var pool in pools.Values)
            {
                if (pool.m_MemberIDs.Contains(obj.GetInstanceID()))
                {
                    p = pool;
                    break;
                }
            }

            if (p == null)
            {
                Debug.LogFormat("Object '{0}' wasn't spawned from a pool. Destroying it instead.", obj.name);
                Object.Destroy(obj);
            }
            else
            {
                p.DeSpawnGameObject(obj);
                obj.transform.SetParent(GetRootGameObject());
            }
        }

        public static int GetStackCount(GameObject prefab)
        {
            if (pools == null)
                pools = new Dictionary<int, Pool>();
            if (prefab == null) return 0;
            return pools.ContainsKey(prefab.GetInstanceID()) ? pools[prefab.GetInstanceID()].StackCount : 0;
        }

        public static void ClearPool()
        {
            if (pools != null)
            {
                pools.Clear();
            }
        }
        private static Transform GetRootGameObject()
        {
            if (sRoot == null)
            {
                GameObject go = new GameObject();
                go.name = "PoolingRoot";
                sRoot = go.transform;
            }
            return sRoot;
        }

        // Helper method to get the cached component for a GameObject
        private static T GetCachedComponent<T>(this GameObject obj) where T : Component
        {
            if (!CachedComponents.ContainsKey(obj)) CachedComponents[obj] = new Dictionary<Type, Component>();
            if (!CachedComponents[obj].ContainsKey(typeof(T)))
                CachedComponents[obj].Add(typeof(T), obj.GetComponent<T>());
            return CachedComponents[obj][typeof(T)] as T;
        }
        // Helper method to get the cached transform for a GameObject
        private static Transform GetCachedTransform(this GameObject obj)
        {
            if (!CachedTransforms.ContainsKey(obj))
                CachedTransforms.Add(obj, obj.transform);
            return CachedTransforms[obj];
        }
    } 
}