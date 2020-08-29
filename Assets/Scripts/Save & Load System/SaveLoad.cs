using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Breach
{
    public sealed class SaveLoad
    {
        static readonly string _savePath = Application.persistentDataPath + "/Saves/";

        public static void Save<T>(T objToSave, string key)
        {
            BinaryFormatter formatter = GetBinaryFormatter();

            if (!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);

            string path = $"{_savePath}{key}.txt";

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
                formatter.Serialize(fileStream, objToSave);
        }

        public static T Load<T>(string key)
        {
            T returnValue = default;

            BinaryFormatter formatter = GetBinaryFormatter();
            string path = $"{_savePath}{key}.txt";

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
                returnValue = (T)formatter.Deserialize(fileStream);

            return returnValue;
        }

        public static bool SaveExists(string key)
        {
            string path = $"{_savePath}{key}.txt";
            return File.Exists(path);
        }

        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SurrogateSelector selector = new SurrogateSelector();

            Vector3SerializationSurrgote vector3Surrgote = new Vector3SerializationSurrgote();
            QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();
            ColorSerializationSurrogate colorSurrogate = new ColorSerializationSurrogate();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrgote);
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);
            selector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), colorSurrogate);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}