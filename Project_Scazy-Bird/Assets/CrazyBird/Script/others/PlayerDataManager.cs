using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public static class PlayerDataManager
{
    private static string ALL_DATA_LEVEL = "ALL_DATA_LEVEL";
    private static string ALL_DATA_PLAYER = "ALL_DATA_PLAYER";

    private static string COIN = "COIN";
    private static string BEST_SCORE = "BEST_SCORE";
    private static string MONEY = "MONEY";
    private static string THUNDER = "THUNDER";
    private static string Sound = "SOUND";
    private static string Music = "MUSIC";

    public static DataLevel DataLevelModel;
    public static DataShopPlayer DataShopPlayerModel;

    static PlayerDataManager()
    {
        DataLevelModel = JsonConvert.DeserializeObject<DataLevel>(PlayerPrefs.GetString(ALL_DATA_LEVEL));
        DataShopPlayerModel = JsonConvert.DeserializeObject<DataShopPlayer>(PlayerPrefs.GetString(ALL_DATA_PLAYER));

        if (DataLevelModel == null)
        {
            DataLevelModel = new DataLevel();
            DataLevelModel.SetLevel(1);
        }

        if (DataShopPlayerModel == null)
        {
            DataShopPlayerModel = new DataShopPlayer();
            DataShopPlayerModel.SetCurrentSKinUsing(E_TypePlayer.Peepers);
            DataShopPlayerModel.AddSkin(E_TypePlayer.Peepers);
        }

        SaveData();
        SaveDataShopPlayer();
    }

    private static void SaveData()
    {
        string Json = JsonConvert.SerializeObject(DataLevelModel);
        PlayerPrefs.SetString(ALL_DATA_LEVEL, Json);

    }


    public static void SaveDataShopPlayer()
    {
        string Json = JsonConvert.SerializeObject(DataShopPlayerModel);

        PlayerPrefs.SetString(ALL_DATA_PLAYER, Json);
    }

    public static int GetCurrentLevel()
    {
        return DataLevelModel.GetCurrentLevel();
    }

    public static void SetLevel(int Level)
    {
        DataLevelModel.SetLevel(Level);
        SaveData();
    }

    public static int GetTortalLevel(string path)
    {
        return DataLevelModel.GetTotalLevel(path);
    }

    public static E_TypePlayer GetCurrentSkinUsing()
    {
        return DataShopPlayerModel.GetCurrentSKinUsing();
    }

    public static void SetCurrentSkinUsing(E_TypePlayer id)
    {
        DataShopPlayerModel.SetCurrentSKinUsing(id);
        SaveDataShopPlayer();
    }

    public static List<E_TypePlayer> GetListSkinOwend()
    {
        return DataShopPlayerModel.GetListSkinOnwed();
    }

    public static void AddSkin(E_TypePlayer skin)
    {
        DataShopPlayerModel.AddSkin(skin);
        SaveDataShopPlayer();
    }

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(BEST_SCORE, 0);
    }

    public static void SetBestScore(int Score)
    {
        PlayerPrefs.SetInt(BEST_SCORE, Score);
    }

    public static int GetCoin()
    {
        return PlayerPrefs.GetInt(COIN, 0);
    }

    public static void SetCoin(int coin)
    {
        PlayerPrefs.SetInt(COIN, coin);
    }

    public static int GetMoney()
    {
        return PlayerPrefs.GetInt(MONEY, 0);
    }

    public static void SetMoney(int Money)
    {
        PlayerPrefs.SetInt(MONEY, Money);
    }

    public static int GetThunder()
    {
        return PlayerPrefs.GetInt(THUNDER, 0);
    }

    public static void SetThunder(int Thunder)
    {
        PlayerPrefs.SetInt(THUNDER, Thunder);
    }

    public static bool GetSound()
    {
        return PlayerPrefs.GetInt(Sound, 1) == 1;
    }

    public static void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt(Sound, isOn ? 1 : 0);
    }

    public static bool GetMusic()
    {
        return PlayerPrefs.GetInt(Music, 1) == 1;
    }

    public static void SetMusic(bool isOn)
    {
        PlayerPrefs.SetInt(Music, isOn ? 1 : 0);
    }
}

public class DataLevel
{
    public int CurrentLevel;

    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public void SetLevel(int Level)
    {
        CurrentLevel = Level;
    }

    public int GetTotalLevel(string path)
    {
        int _count = 0;

        GameObject[] Resources1 = Resources.LoadAll<GameObject>(path);
        _count = Resources1.Length;

        return _count;
    }
}

public class DataShopPlayer
{
    public E_TypePlayer CurrentSkinUsing;

    public List<E_TypePlayer> L_SkinOnwed = new List<E_TypePlayer>();

    public E_TypePlayer GetCurrentSKinUsing()
    {
        return CurrentSkinUsing;
    }

    public void SetCurrentSKinUsing(E_TypePlayer id)
    {
        CurrentSkinUsing = id;
    }

    public List<E_TypePlayer> GetListSkinOnwed()
    {
        return L_SkinOnwed;
    }

    public void AddSkin(E_TypePlayer Skin)
    {
        if (L_SkinOnwed.Contains(Skin)) return;

        L_SkinOnwed.Add(Skin);
    }
}
