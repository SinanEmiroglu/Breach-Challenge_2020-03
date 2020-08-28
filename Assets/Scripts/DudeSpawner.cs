// Copyright (c) Breach AS. All rights reserved.
using System.Collections;
using UnityEngine;

namespace Breach
{
    public class DudeSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject dudePrefab;

        private float spawnVolumeSize = 10f;
        private float gridSnap = 0.5f;

        private void Start()
        {        
            StartCoroutine(SpawnDudes());
        }

        private IEnumerator SpawnDudes()
        {
            while(true)
            { 
                yield return new WaitForSeconds(0.25f);

                // Create gridsnapped random spawn position
                Vector3 randomSpawnSpot = new Vector3(
                    Mathf.Round(UnityEngine.Random.Range(-spawnVolumeSize * 0.5f, spawnVolumeSize * 0.5f) / gridSnap) * gridSnap,
                    Mathf.Round(UnityEngine.Random.Range(0, spawnVolumeSize) / gridSnap) * gridSnap,
                    Mathf.Round(UnityEngine.Random.Range(-spawnVolumeSize * 0.5f, spawnVolumeSize * 0.5f) / gridSnap) * gridSnap
                );

                Quaternion randomRotation = Quaternion.Euler(0, (int)Random.Range(0, 3) * 90f, 0);
                GameObject spawnedDudeGO = Instantiate(dudePrefab, randomSpawnSpot, randomRotation);

                // Set an important state on the dude (which needs to be saved/loaded as well)
                spawnedDudeGO.GetComponent<Dude>().AnImportantStateValue = UnityEngine.Random.Range(1, 10000);
            }
        }
    }
}