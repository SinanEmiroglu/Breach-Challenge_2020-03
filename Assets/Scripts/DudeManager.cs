// Copyright (c) Breach AS. All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breach
{
    public class DudeManager : MonoBehaviour
    {
        [SerializeField] private Dude dudePrefab;
        [SerializeField] private bool isSpawning;

        private float spawnVolumeSize = 10f;
        private float gridSnap = 0.5f;
        private List<DudeData> _allAvailableDudes;

        private void OnEnable()
        {
            GameManager.OnSave += SaveHandler;
            GameManager.OnLoad += LoadHandler;
        }

        private void Start()
        {
            _allAvailableDudes = new List<DudeData>();
            StartCoroutine(SpawnDudes());
        }

        private void LoadHandler()
        {
            if (!SaveLoad.SaveExists("Dudes"))
                return;

            _allAvailableDudes = SaveLoad.Load<List<DudeData>>("Dudes");

            foreach (var dude in _allAvailableDudes)
            {
                var spawned = Instantiate(dudePrefab, dude.Position, dude.Rotation);
                spawned.SetDudeData(dude);
            }
        }

        private void SaveHandler()
        {
            SaveLoad.Save(_allAvailableDudes, "Dudes");
        }

        private IEnumerator SpawnDudes()
        {
            while (isSpawning)
            {
                yield return new WaitForSeconds(0.25f);

                Vector3 randomSpawnSpot = GetRandomSpawnSpot();
                Quaternion randomRotation = GetRandomRotation();

                Dude spawnedDudeGO = Instantiate(dudePrefab, randomSpawnSpot, randomRotation);
                var dudeData = new DudeData(Random.Range(1, 10000), GetRandomSpawnSpot(), randomRotation);
                // Set an important state on the dude (which needs to be saved/loaded as well)
                spawnedDudeGO.SetDudeData(dudeData);

                _allAvailableDudes.Add(dudeData);
                spawnedDudeGO.OnDestroyed += (dude) => _allAvailableDudes.Remove(dude.DudeData);
            }
        }

        private Quaternion GetRandomRotation() => Quaternion.Euler(0, Random.Range(0, 3) * 90f, 0);

        private Vector3 GetRandomSpawnSpot()
        {
            return new Vector3(
                                Mathf.Round(Random.Range(-spawnVolumeSize * 0.5f, spawnVolumeSize * 0.5f) / gridSnap) * gridSnap,
                                Mathf.Round(Random.Range(0, spawnVolumeSize) / gridSnap) * gridSnap,
                                Mathf.Round(Random.Range(-spawnVolumeSize * 0.5f, spawnVolumeSize * 0.5f) / gridSnap) * gridSnap
                            );
        }
    }
}