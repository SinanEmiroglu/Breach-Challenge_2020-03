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

        private void OnEnable()
        {
            GameManager.OnSave += SaveHandler;
            GameManager.OnLoad += LoadHandler;
        }

        private void Start()
        {
            _allAvailableDudesData = new List<DudeData>();
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
    }
}