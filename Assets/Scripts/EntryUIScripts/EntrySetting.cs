using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class EntrySetting : MonoBehaviour
{
    public static SettingDataHolder Instance { get; private set; }
    private static string path;
 
    void Awake()
    {
        path = Application.persistentDataPath + "/SettingData.asset";
        Debug.Log(path);
        if (Instance == null)
        {
            Instance = new SettingDataHolder();
            DontDestroyOnLoad(gameObject);
            LoadSettingData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

     //读取数据
    public static void LoadSettingData()
    {
        //若是路径上有文件，就读取文件
        if (File.Exists(path))
        {
            //读取数据
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Instance = (SettingDataHolder)bf.Deserialize(file);
            file.Close();
        }

    }

        //保存数据
    public static void SavePlayerData()
    {  
        //保存数据 
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        FileStream file = File.Create(path);      
        bf.Serialize(file, Instance);
        file.Close();       
    }
}