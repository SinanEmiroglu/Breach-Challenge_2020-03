using System.Collections.Generic;
using UnityEngine;

namespace Breach
{
    public class Spawnable : PooledMonoBehaviour
    {
        [Header("Spawnable Attributes")]
        [SerializeField] private int totalNumberToSpawn;

        private static List<Spawnable> _spawnables = new List<Spawnable>();

        public int TotalNumberToSpawn => totalNumberToSpawn;

        /// <summary>
        /// Returning all active spawnable game objects in the hierarchy.
        /// </summary>
        public static IEnumerable<Spawnable> GetActiveSpawnables()
        {
            foreach (var item in _spawnables)
            {
                yield return item;
            }
        }

        protected virtual void OnEnable()
        {
            _spawnables.Add(this);
            OnReturnToPool += (pooled) => _spawnables.Remove(this);
        }
    }
}