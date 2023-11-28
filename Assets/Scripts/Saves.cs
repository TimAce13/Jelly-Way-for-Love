using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Saves : MonoBehaviour
{
    private Dictionary<string, string> _defaultSave = new Dictionary<string, string>
    {
        { "Save", "str#1"},
        { "Level", "int#1"},
        {"JellyCoin", "int#100000"},
        {"PlayerObjects","str#" }
    };

    private Dictionary<int, int> _defaultPlayerObjects = new Dictionary<int, int>()
    {
        // {id, state} state=0(none) state=0(bought) state=0(selected and bought)
        {0,2}, {1,0}, {2,0}
    };

    [SerializeField] private MainUI mainUI;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Save"))
        {
            SetDefaultSettings();
        };
    }

    void SetDefaultSettings()
    {
        foreach (var item in _defaultSave)
        {
            if (item.Value.Contains("str#"))
            {
                PlayerPrefs.SetString(item.Key, item.Value.Substring(4));
            }
            else
            {
                PlayerPrefs.SetInt(item.Key, int.Parse(item.Value.Substring(4)));
            }
        }

        SavePlayerObjects(_defaultPlayerObjects);
        PlayerPrefs.Save();
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("Level");

    }
    public int GetJellyCoins()
    {
        return PlayerPrefs.GetInt("JellyCoin");
    }

    public void EditJellyCoins(int editedValue)
    {
        PlayerPrefs.SetInt("JellyCoin", PlayerPrefs.GetInt("JellyCoin") + editedValue);
        mainUI.UpdateJellyCoinsText();
        PlayerPrefs.Save();
    }

    public string GetPlayerObjectsStatus()
    {
        return PlayerPrefs.GetString("PlayerObjects");
    }

    public void SavePlayerObjects(Dictionary<int, int> dictionary)
    {
        // Convert Dictionary<int, int> to a serializable format
        List<SerializableKeyValuePair> serializableList = new List<SerializableKeyValuePair>();

        foreach (var kvp in dictionary)
        {
            serializableList.Add(new SerializableKeyValuePair(kvp.Key, kvp.Value));
        }

        string json = JsonUtility.ToJson(new SerializableList<SerializableKeyValuePair>(serializableList));

        Debug.Log(json);

        // Save the JSON string to PlayerPrefs
        PlayerPrefs.SetString("PlayerObjects", json);
        PlayerPrefs.Save();
    }

    public Dictionary<int, int> GetPlayerObjects()
    {
        // Load the JSON string from PlayerPrefs
        string json = PlayerPrefs.GetString("PlayerObjects", "");

        if (!string.IsNullOrEmpty(json))
        {
            // Convert JSON back to List<SerializableKeyValuePair>
            SerializableList<SerializableKeyValuePair> loadedData = JsonUtility.FromJson<SerializableList<SerializableKeyValuePair>>(json);

            // Convert List<SerializableKeyValuePair> back to Dictionary<int, int>
            Dictionary<int, int> loadedDictionary = new Dictionary<int, int>();

            foreach (var kvp in loadedData.list)
            {
                loadedDictionary.Add(kvp.Key, kvp.Value);
            }

            return loadedDictionary;
        }

        return null;
    }


    // SerializableKeyValuePair class to help with JSON serialization
    [System.Serializable]
    public class SerializableKeyValuePair
    {
        public int Key;
        public int Value;

        public SerializableKeyValuePair(int key, int value)
        {
            Key = key;
            Value = value;
        }
    }

    // SerializableList class to help with JSON serialization
    [System.Serializable]
    public class SerializableList<T>
    {
        public List<T> list;

        public SerializableList(List<T> sourceList)
        {
            list = sourceList;
        }
    }
}
