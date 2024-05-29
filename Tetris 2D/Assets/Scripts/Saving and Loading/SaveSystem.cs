using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayerStats(HighScoreData highScoreData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = System.IO.Directory.GetCurrentDirectory() + "/game.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(highScoreData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayerStats()
    {
        string path = System.IO.Directory.GetCurrentDirectory() + "/game.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
