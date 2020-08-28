using UnityEngine;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public GameObject LoadButtonPrefab;

    public string[] SavedFiles;

    public void GetLoadFiles()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");

        SavedFiles = Directory.GetFiles(Application.persistentDataPath + "/Saves/");
    }
}