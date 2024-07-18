using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;

    public LevelManager CurrentLevel;
    public E_PlayMode PlayMode;

    [Header("Level Mode ")]
    public int LevelPlaying;
    public int TotalLevel;
    public int AmoutBlockCurrent;
    public bool CanPlayLevel;
    public bool isOpenShop;

    [Header("EndLess Mode")]
    public int YourScore;
    public int CountCombo;

    [Header("UI")]
    public UiController uiController;

    public E_GameState CurrentGameState;
    public bool _isBackToHome;

    public int TimeInGame;

    public int TimeSoundBird;

    public Block Kiwi;

    private void Awake()
    {
        if (ins == null)
            ins = this;
    }

    private void Start()
    {
        AdManager.instance.ShowBanner();
        /*        InitLevel();
                EventManager.EmitEvent(EventContains.CURRENT_BLOCK);*/
        PlayMode = E_PlayMode.EndlessMode;
        InitPlayer();
        GameState(E_GameState.Home);

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && AdsHandle.instance.CanShowInterAFK)
        {
            uiController.remainingShowAds.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(0) && !AdsHandle.instance.Detected && !isOpenShop)
        {
            PrefabStorage.ins.player.ChangeJumpAFK();
            AdsHandle.instance.Detected = true;
            AdsHandle.instance.ShowInterAFK();
        }

        if(AdsHandle.instance.CanShowInterAFK)
        {
            PrefabStorage.ins.player.JumpAfterInterAFK = false;
        }


/*        if (Input.GetMouseButton(0) && AdsHandle.instance.CanShowInterAFK)
        {

            AdManager.instance.ShowInter(() =>
            {
                CanPlayGame();
            }, null, "ShowInter");

            AdsHandle.instance.CanShowInterAFK = false;
            AdsHandle.instance.ShowInterAFK();
        }*/

    }
    public void InitPlayer()
    {
        GameObject PlayerObj = Resources.Load<GameObject>("Player/Player_" + (int)PlayerDataManager.GetCurrentSkinUsing());
        GameObject PlayerInGame = Instantiate(PlayerObj, PrefabStorage.ins.StartPos.transform.position, Quaternion.identity);

        Player player = PlayerInGame.GetComponent<Player>();
        player.Trajectory.dotsParent = PrefabStorage.ins.TrajectionParent;

        PrefabStorage.ins.player = player;
    }

    public void LoadPlayerWhenChooseInShop()
    {
        PrefabStorage.ins.player.OndisableListDots();
        Destroy(PrefabStorage.ins.player.gameObject);
        Debug.Log(PlayerDataManager.GetCurrentSkinUsing());
        GameObject PlayerObj = Resources.Load<GameObject>("Player/Player_" + (int)PlayerDataManager.GetCurrentSkinUsing());
        GameObject PlayerInGame = Instantiate(PlayerObj, PrefabStorage.ins.StartPos.transform.position, Quaternion.identity);

        Player player = PlayerInGame.GetComponent<Player>();
        player.Trajectory.dotsParent = PrefabStorage.ins.TrajectionParent;

        PrefabStorage.ins.player = player;
    }


    #region EndlessMode
    #endregion

    #region LevelMode

    public void InitLevel()
    {
        TotalLevel = PlayerDataManager.GetTortalLevel("Level");
    }

    public void LoadLevelMode(E_PlayMode Playmode)
    {
        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel.gameObject);
        }

        if (Playmode == E_PlayMode.LevelMode)
        {

            AmoutBlockCurrent = 0;

            LevelPlaying = PlayerDataManager.GetCurrentLevel();
            LevelManager Level = Resources.Load<LevelManager>("Level/Level_" + LevelPlaying);

            LevelManager LevelObj = Instantiate(Level, Vector3.zero, Quaternion.identity);

            CurrentLevel = LevelObj;
        }
        else
        {
            LevelManager Level = Resources.Load<LevelManager>("EndlessMode/Endless_mode");

            LevelManager LevelObj = Instantiate(Level, Vector3.zero, Quaternion.identity);

            CurrentLevel = LevelObj;
        }
    }

    public void IncreaseLevel(int Level)
    {
        if (PlayerDataManager.GetCurrentLevel() > TotalLevel)
        {
            PlayerDataManager.SetLevel(1);
            LoadLevelMode(E_PlayMode.LevelMode);

            PrefabStorage.ins.player.ResetStatePlayer();
            return;
        };

        Level = PlayerDataManager.GetCurrentLevel();
        Level++;

        PlayerDataManager.SetLevel(Level);
        LoadLevelMode(E_PlayMode.LevelMode);

        PrefabStorage.ins.player.ResetStatePlayer();
    }
    public void ResetLevel()
    {
        LoadLevelMode(PlayMode);
        PrefabStorage.ins.player.ResetStatePlayer();
        PrefabStorage.ins.CameraVirtual.ResetStartPosVirtualCamera();
        EventManager.EmitEvent(EventContains.UPDATEMAINUI, 0.5f);
    }

    #endregion

    public void GameState(E_GameState GameState)
    {
        switch (GameState)
        {
            case E_GameState.Home:

                CurrentGameState = GameState;
                CanPlayLevel = false;
                _isBackToHome = true;
                TimeInGame = 0;
                CountCombo = 0;
                uiController.ActiveUiHome();
                uiController.DeActiveUiGamePlay();
                ResetLevel();
                ResetEndlessMode();
                PrefabStorage.ins.player.ResetStatePlayer();
                PrefabStorage.ins.player.ResetForce();
                PrefabStorage.ins.CamFake.InitCamera();
                PrefabStorage.ins.CamFake.UpdateVirtualCam();
                StopCoroutine(uiController.IE_DelayActivePopupRevive());
                StopCoroutine(IE_DelayPlayGame());

                break;
            case E_GameState.GamePlay:

                CurrentGameState = GameState;
                _isBackToHome = false;
                uiController.ActiveUiGamePlay();
                uiController.DeActiveUiHome();
                StartCoroutine(IE_DelayPlayGame());
                break;
            case E_GameState.Pause:

                CurrentGameState = GameState;
                CanPlayLevel = false;
                _isBackToHome = false;

                PrefabStorage.ins.player.ResetForce();
                PrefabStorage.ins.player.Trajectory.HideTrajection();
                uiController.ActivePopupPause();
                break;
            case E_GameState.Revive:

                CurrentGameState = GameState;

                CanPlayLevel = true;
                _isBackToHome = false;

                uiController.DeActivePopupRevive();
                PrefabStorage.ins.player.ResetRevive();
                PrefabStorage.ins.player.Trajectory.HideTrajection();
                break;
            default:
                break;
        }
    }

    public void ResetEndlessMode()
    {
        YourScore = 0;
        ins.uiController.UigamePlay.InitScoreEndlessMode();
    }

    public void CanPlayGame()
    {
        StartCoroutine(IE_DelayPlayGame());
    }

    IEnumerator IE_DelayPlayGame()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if (CurrentGameState != E_GameState.Home)
        {
            CanPlayLevel = true;
        }
    }
}
