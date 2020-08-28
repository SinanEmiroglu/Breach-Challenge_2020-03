// Copyright (c) Breach AS. All rights reserved.
using UnityEngine;

[System.Serializable]
public class DudeData
{
    public int AnImportantStateValue;
    public float MoveSpeed;
    public Vector3 Position;
    public Quaternion Rotation;

    public DudeData(int anImportantStateValue, Vector3 position, Quaternion rotation)
    {
        AnImportantStateValue = anImportantStateValue;
        Position = position;
        Rotation = rotation;
    }
}