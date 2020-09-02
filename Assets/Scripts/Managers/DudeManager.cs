// Copyright (c) Breach AS. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Breach
{
    public class DudeManager : MonoBehaviour
    {
        private const string SAVE_FILE_KEY = "Dudes";

        [SerializeField] private Dude dudePrefab;

        private List<DudeData> _allAvailableDudesData = new List<DudeData>();

        private void OnEnable()
        {
            GameManager.OnSave += SaveHandler;
            GameManager.OnLoad += LoadHandler;
            SceneManager.sceneUnloaded += SceneUnloadedHandler;
        }

        /// <summary>
        /// When Level scene was unloaded, return all dudes to the pool.
        /// </summary>
        private void SceneUnloadedHandler(Scene scene)
        {
            if (scene.buildIndex == 1)
            {
                var dudePool = Pool.GetPool(dudePrefab);
                foreach (var dude in dudePool.GetComponentsInChildren<Dude>())
                    dude.ReturnToPool();
            }
        }

        /// <summary>
        /// When Level is loaded, instantiate all dudes and set their saved values.
        /// </summary>
        private void LoadHandler()
        {
            if (!SaveLoad.SaveExists(SAVE_FILE_KEY))
                return;

            _allAvailableDudesData = SaveLoad.Load<List<DudeData>>(SAVE_FILE_KEY);

            foreach (var data in _allAvailableDudesData)
            {
                var spawned = dudePrefab.Get<Dude>(data.Position, data.Rotation);
                spawned.SetDudeData(data);
            }

            _allAvailableDudesData.Clear();
        }

        /// <summary>
        /// Save all active dudes in the scene. {Positions, Rotations, Colors, Speeds, AnImportantStateValues}
        /// </summary>
        private void SaveHandler()
        {
            var spawnables = Spawnable.GetActiveSpawnables();

            foreach (Dude dude in spawnables)
            {
                _allAvailableDudesData.Add(dude.DudeData);
            }

            SaveLoad.Save(_allAvailableDudesData, SAVE_FILE_KEY);
        }

        private void OnDisable()
        {
            GameManager.OnSave -= SaveHandler;
            GameManager.OnLoad -= LoadHandler;
            SceneManager.sceneUnloaded -= SceneUnloadedHandler;
        }
    }
}