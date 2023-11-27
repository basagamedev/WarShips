using EnemyShip;
using Ship.Attack;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolSystem
{
    public class GenericObjectPool : MonoBehaviour
    {
        #region INSTANCE_FIELD
        public static GenericObjectPool Instance { get; private set; }
        #endregion

        #region SERIALIZE_FIELDS
        [Header("CannonBall")]
        [SerializeField] private int numberOfBalls = 30;
        [SerializeField] private CannonBall cannonBallPrefab = null;


        [Header("Enemy")]
        [SerializeField] private int numberOfEnemys = 10;
        [SerializeField] private EnemyShipController enemyShipPrefab = null;
        #endregion

        #region PRIVATE_FIELDS
        private Dictionary<Type, List<Component>> pooledObjects;
        private Dictionary<Type, Component> prefabs;
        #endregion

        #region UNITY_METHODS
        void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);

            pooledObjects = new Dictionary<Type, List<Component>>();
            prefabs = new Dictionary<Type, Component>();

            AddType(cannonBallPrefab, numberOfBalls);
            AddType(enemyShipPrefab, numberOfEnemys);
        }
        #endregion

        #region PUBLIC_METHODS
        public void AddType<T>(T prefab, int amountToPool) where T : Component
        {
            prefabs[typeof(T)] = prefab;

            var objectList = new List<Component>();
            for (int i = 0; i < amountToPool; i++)
            {
                objectList.Add(InstantiateObject(prefab));
            }

            pooledObjects[typeof(T)] = objectList;
        }

        public T GetPooledObject<T>() where T : Component
        {
            Type type = typeof(T);
            if (!pooledObjects.ContainsKey(type)) return null;

            foreach (T obj in pooledObjects[type])
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    return obj;
                }
            }

            return InstantiateObject(prefabs[type]) as T;
        }

        public void ReturnObjectToPool<T>(T objectToReturn) where T : Component
        {
            objectToReturn.gameObject.SetActive(false);
        }
        #endregion

        #region PRIVATE_METHODS
        private T InstantiateObject<T>(T prefab) where T : Component
        {
            T obj = Instantiate(prefab, this.transform);
            obj.gameObject.SetActive(false);
            return obj;
        }
        #endregion
    }
}
