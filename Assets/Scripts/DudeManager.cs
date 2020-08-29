// Copyright (c) Breach AS. All rights reserved.
using System.Collections.Generic;
using UnityEngine;

namespace Breach
{
    public class DudeManager : MonoBehaviour
    {
        private const string SAVE_FILE_KEY = "Dudes";

        [SerializeField] private Dude dudePrefab;

        private List<DudeData> _allAvailableDudesData;

        //[SerializeField] private bool isSpawning;
        //private float spawnVolumeSize = 10f;
        //private float gridSnap = 0.5f;

        private void OnEnable()
        {
            GameManager.OnSave += SaveHandler;
            GameManager.OnLoad += LoadHandler;
        }

        private void Start()
        {
            _allAvailableDudesData = new List<DudeData>();
            //StartCoroutine(SpawnDudes());
        }

        private void LoadHandler()
        {
            if (!SaveLoad.SaveExists(nameof(SAVE_FILE_KEY)))
                return;

            _allAvailableDudesData = SaveLoad.Load<List<DudeData>>(nameof(SAVE_FILE_KEY));

            foreach (var data in _allAvailableDudesData)
            {
                var spawned = dudePrefab.Get<Dude>(data.Position, data.Rotation);
                spawned.SetDudeData(data);
            }
        }

        private void SaveHandler()
        {
            SaveLoad.Save(_allAvailableDudesData, nameof(SAVE_FILE_KEY));
        }

        private void OnDisable()
        {
            GameManager.OnSave -= SaveHandler;
            GameManager.OnLoad -= LoadHandler;
        }

        //private IEnumerator SpawnDudes()
        //{
        //    while (isSpawning)
        //    {
        //        yield return new WaitForSeconds(0.25f);

        //        Vector3 randomSpawnSpot = GetRandomSpawnSpot();
        //        Quaternion randomRotation = GetRandomRotation();

        //        Dude spawnedDudeGO = Instantiate(dudePrefab, randomSpawnSpot, randomRotation);
        //        var dudeData = new DudeData(Random.Range(1, 10000), GetRandomSpawnSpot(), randomRotation);
        //        Set an important state on the dude(which needs to be saved / loaded as well)
        //        spawnedDudeGO.SetDudeData(dudeData);

        //        _allAvailableDudes.Add(dudeData);
        //        spawnedDudeGO.OnReturnToPool += (dude) => _allAvailableDudes.Remove(dude.GetComponent<Dude>().DudeData);
        //    }
        //}

        //private Quaternion GetRandomRotation() => Quaternion.Euler(0, Random.Range(0, 3) * 90f, 0);

        //private Vector3 GetRandomSpawnSpot()
        //{
        //    return new Vector3(
        //                        Mathf.Round(Random.Range(-spawnVolumeSize * 0.5f, spawnVolumeSize * 0.5f) / gridSnap) * gridSnap,
        //                        Mathf.Round(Random.Range(0, spawnVolumeSize) / gridSnap) * gridSnap,
        //                        Mathf.Round(Random.Range(-spawnVolumeSize * 0.5f, spawnVolumeSize * 0.5f) / gridSnap) * gridSnap
        //                    );
        //}
    }
}