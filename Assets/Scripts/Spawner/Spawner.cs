using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breach
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Spawnable[] prefabs;
        [SerializeField] private Transform[] spawnPoints;

        private float _spawnTimer;
        private int _totalNumberToSpawn;
        private int _totalNumberSpawned = 0;
        private readonly float initialSpawnDelay = 1;
        private readonly float _respawnRate = 10;
        private readonly int _numberToSpawnEachTime = 1;

        private void OnEnable()
        {
            _spawnTimer = _respawnRate - initialSpawnDelay;
        }

        private void Start()
        {
            if (LevelManager.TryGetInstance(out LevelManager manager))
                _totalNumberToSpawn = manager.CurrentLevel.DudeNumberToSpawn;
        }

        private void Update()
        {
            _spawnTimer += Time.deltaTime;

            if (ShouldSpawn())
                Spawn();
        }

        private void Spawn()
        {
            _spawnTimer = 0;

            var availableSpawnPoints = spawnPoints.ToList();
            for (int i = 0; i < _numberToSpawnEachTime; i++)
            {
                if (_totalNumberSpawned >= _totalNumberToSpawn && _totalNumberToSpawn > 0)
                    break;

                Spawnable prefab = ChooseRandomPrefab();
                if (prefab != null)
                {
                    Transform spawnPoint = ChooseRandomSpawnPoint(availableSpawnPoints);

                    if (availableSpawnPoints.Contains(spawnPoint))
                        availableSpawnPoints.Remove(spawnPoint);

                    var spawned = prefab.Get<Spawnable>(spawnPoint.position, spawnPoint.rotation);

                    _totalNumberSpawned++;
                }
            }
        }

        private Transform ChooseRandomSpawnPoint(List<Transform> availableSpawnPoints)
        {
            if (availableSpawnPoints.Count == 0)
                return transform;

            if (availableSpawnPoints.Count == 1)
                return availableSpawnPoints[0];

            int index = Random.Range(0, availableSpawnPoints.Count);
            return availableSpawnPoints[index];
        }

        private Spawnable ChooseRandomPrefab()
        {
            if (prefabs.Length == 0)
                return null;

            if (prefabs.Length == 1)
                return prefabs[0];

            int index = Random.Range(0, prefabs.Length);
            return prefabs[index];
        }

        private bool ShouldSpawn()
        {
            if (_totalNumberSpawned >= _totalNumberToSpawn && _totalNumberToSpawn > 0)
                return false;

            return _spawnTimer >= _respawnRate;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, Vector3.one);
            foreach (var spawnPoint in spawnPoints)
            {
                Gizmos.DrawSphere(spawnPoint.position, 0.5f);
            }
        }

#endif
    }
}