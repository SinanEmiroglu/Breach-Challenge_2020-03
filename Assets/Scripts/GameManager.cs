// Copyright (c) Breach AS. All rights reserved.

using UnityEngine;


namespace Breach
{
    public class GameManager : MonoBehaviour
    {


        private int tick;
        private float tickLength = 1f;
        private float lastTick;
        private void Update()
        {
            if (Time.time - lastTick > tickLength)
            {
                tick++;
                lastTick = Time.time;
            }
        }

        void OnGUI()
        {

            if (GUI.Button(new Rect(10, 10, 150, 50), "Save Level"))
                print("Save the current level.");

            if (GUI.Button(new Rect(170, 10, 150, 50), "Load Level"))
                print("Replaces current level with one loaded from a previous save");

            if (GUI.Button(new Rect(10, 70, 150, 50), "Save GameState"))
                print("Save transforms and velocities of all dynamic game object and states");

            if (GUI.Button(new Rect(170, 70, 150, 50), "Load GameState"))
                print("Loads previously saved dynamic game objects, restoring their transforms and velocities");

            GUI.TextField(new Rect(10, 130, 150, 20), "Tick: " + tick.ToString());

        }
    }

}

