// Copyright (c) Breach AS. All rights reserved.
using UnityEngine;

namespace Breach
{
    [System.Serializable]
    public class DudeData
    {
        public int AnImportantStateValue;
        public float MoveSpeed;
        public Vector3 Position;
        public Vector3 Velocity;
        public Quaternion Rotation;

        public DudeData(int anImportantStateValue, Vector3 position, Quaternion rotation)
        {
            AnImportantStateValue = anImportantStateValue;
            Position = position;
            Rotation = rotation;
        }
    }
}