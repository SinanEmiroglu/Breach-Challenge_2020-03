// Copyright (c) Breach AS. All rights reserved.
using System;
using UnityEngine;

namespace Breach
{
    public class GameManager : Singleton<GameManager>
    {
        public static event Action OnSave = delegate { };
        public static event Action OnLoad = delegate { };

        private int _tick = 0;
        private float _tickLength = 1f;
        private float _lastTick;

        public void Save() => OnSave?.Invoke();
        public void Load() => OnLoad?.Invoke();

        private void Update()
        {
            if (Time.time - _lastTick > _tickLength)
            {
                _tick++;
                _lastTick = Time.time;
            }
        }

        void OnGUI()
        {

            if (GUI.Button(new Rect(10, 10, 150, 50), "Save Level"))
            {
                Save();

            }
            //print("Save the current level.");

            if (GUI.Button(new Rect(170, 10, 150, 50), "Load Level"))
                Load();
            //print("Replaces current level with one loaded from a previous save");

            if (GUI.Button(new Rect(10, 70, 150, 50), "Save GameState"))
                print("Save transforms and velocities of all dynamic game object and states");

            if (GUI.Button(new Rect(170, 70, 150, 50), "Load GameState"))
                print("Loads previously saved dynamic game objects, restoring their transforms and velocities");

            GUI.TextField(new Rect(10, 130, 150, 20), "Tick: " + _tick.ToString());

        }
    }
}