using Cinemachine;
using Cinemachine.Utility;
using DG.Tweening;
using System;
using System.Collections;
using System.Reflection;
using TigerForge;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;

    bool _canJump;
    bool _canDrawPath;
    bool _isShowPath;
    bool _IsElastic;
    bool _isDeath;
    bool _isPlaysound;

    public bool JumpAfterInterAFK;

    public Vector3 _startSpawn;


    [SerializeField] private float _Force;
    [SerializeField] private int LastBlockID = 1;

    [SerializeField] E_TypeSpawnBlock TypeSpawn;
    [SerializeField] E_TypeSpawnBlock LastTypeJump;


    [Header("Draw JumpForce")]
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 Dir;
    public Vector3 Force;
    float distance;

    Sequence TweenJump;
    Tweener tweenElastic;
    public Trajectory Trajectory;

    public float MaxRange = 3.5f;
    public int limitDir;

    private void Awake()
    {
        _startSpawn = transform.position;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // DOTween.Play(TweenJump);

        _rb = GetComponent<Rigidbody>();
        InitStatePlayer();

    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && GameManager.ins.CanPlayLevel && !_isDeath && JumpAfterInterAFK)
        {
            Jump(TypeSpawn);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && GameManager.ins.CanPlayLevel && !_isDeath && JumpAfterInterAFK)
        {
            if (!_canDrawPath || !GameManager.ins.CanPlayLevel && !_canJump) return;

            if (_Force > 0.5f)
            {
                DrawPathForward(_Force, TypeSpawn);
            }

            AddForceToJump();
            springy();
        }
    }

    public void InitStatePlayer()
    {
        TypeSpawn = E_TypeSpawnBlock.Forward;
        LastTypeJump = E_TypeSpawnBlock.Forward;
        FLip(TypeSpawn);
        LastBlockID = 1;
        JumpAfterInterAFK = true;
        _isDeath = false;
        transform.localScale = Vector3.one * 5f;
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void ResetStatePlayer()
    {
        MaxRange = 3.5f;
        transform.SetParent(null);
        Trajectory.HideTrajection();
        transform.position = _startSpawn;

        TypeSpawn = E_TypeSpawnBlock.Forward;
        LastTypeJump = E_TypeSpawnBlock.Forward;

        FLip(TypeSpawn);
        LastBlockID = 1;

        JumpAfterInterAFK = true;
        _canJump = true;
        _canDrawPath = true;
        _isDeath = false;

        transform.localScale = Vector3.one * 5f;
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void ResetForce()
    {
        Trajectory.HideTrajection();
        _Force = 0;
        transform.localScale = Vector3.one * 5f;
        _isShowPath = false;
        _canJump = true;
        _canDrawPath = true;
        JumpAfterInterAFK = true;
    }

    public void Death()
    {
        _animator.SetInteger("Transition", 3);
        _isDeath = true;
    }


    public void ResetRevive()
    {
        transform.SetParent(null);
        int index = GameManager.ins.CurrentLevel.L_BlockInLevel.Count;

        if (index - 2 == 0)
        {
            transform.position = _startSpawn;
        }
        else
        {
            transform.position = new Vector3(GameManager.ins.CurrentLevel.L_BlockInLevel[index - 2].transform.position.x, GameManager.ins.CurrentLevel.L_BlockInLevel[index - 2].Point_y.position.y + 1f, GameManager.ins.CurrentLevel.L_BlockInLevel[index - 2].transform.position.z);
        }

        _Force = 0;
        _isShowPath = false;
        _canJump = true;
        _canDrawPath = true;
        _isDeath = false;
        JumpAfterInterAFK = true;

        _animator.SetInteger("Transition", 1);

    }

    public void springy()
    {
        float ScaleForce = transform.localScale.y;

        if (ScaleForce <= 2.5f) return;

        ScaleForce -= 0.024f;

        transform.localScale = new Vector3(transform.localScale.x, ScaleForce, transform.localScale.z);
    }

    public void AddForceToJump()
    {
        if (!_isPlaysound)
        {
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.HoldJump);
            _isPlaysound = true;
        }
        _Force += 0.1f;
        _Force = Mathf.Clamp(_Force, 0, 12f);
    }


    public void Jump(E_TypeSpawnBlock DirJump)
    {
        _canDrawPath = false;

        if (!_canJump || !GameManager.ins.CanPlayLevel) return;

        if (!JumpAfterInterAFK) return;

        switch (DirJump)
        {
            case E_TypeSpawnBlock.Left:
                SpawnTrail();

                transform.localScale = Vector3.one * 5f;

                int index = GameManager.ins.CurrentLevel.L_BlockInLevel.Count;


                TweenJump = transform.DOJump(new Vector3(GameManager.ins.CurrentLevel.L_BlockInLevel[index - 1].Point_y.position.x, GameManager.ins.CurrentLevel.L_BlockInLevel[index - 1].Point_y.position.y, transform.position.z + _Force), 2, 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundNewBlock);
                });

                Trajectory.HideTrajection();
                _animator.SetInteger("Transition", 2);

                if (_Force == 10f)
                {
                    SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundFly);
                }
                else
                {
                    SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundJump);
                }

                _Force = 0;
                _canJump = false;
                _isShowPath = false;

                break;
            case E_TypeSpawnBlock.Forward:
                SpawnTrail();

                transform.localScale = Vector3.one * 5f;
                int index1 = GameManager.ins.CurrentLevel.L_BlockInLevel.Count;

                TweenJump = transform.DOJump(new Vector3(transform.position.x + _Force, GameManager.ins.CurrentLevel.L_BlockInLevel[index1 - 1].Point_y.position.y, GameManager.ins.CurrentLevel.L_BlockInLevel[index1 - 1].Point_y.position.z), 2, 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundNewBlock);
                });

                Trajectory.HideTrajection();
                _animator.SetInteger("Transition", 2);

                if (_Force == 10f)
                {
                    SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundFly);
                }
                else
                {
                    SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundJump);
                }

                _Force = 0;
                _canJump = false;
                _isShowPath = false;

                break;
            default:
                break;
        }
    }

    public void SpawnTrail()
    {
        ParticleSystem Trail = Instantiate(PrefabStorage.ins.FxTrail);
        Trail.transform.SetParent(transform);
        Trail.transform.localPosition = new Vector3(0, 0.05f, -0.1f);
        Trail.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    public void FLip(E_TypeSpawnBlock TypeSpawn)
    {
        if (TypeSpawn == E_TypeSpawnBlock.Forward)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Finish"))
        {
            _animator.SetInteger("Transition", 3);
            _isDeath = true;
            StartCoroutine(IE_delayChangeRevive());
            Trajectory.HideTrajection();
            SoundManager.Instance.SoundAudio.Stop();

            SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundRoixuong);
            return;
        }

        Block block = collision.gameObject.GetComponent<Block>();

        if (block != null && !_IsElastic && block.id == LastBlockID)
        {
            GameManager.ins.CountCombo = 0;

            _IsElastic = true;
            _canJump = true;
            _canDrawPath = true;
            _animator.SetInteger("Transition", 1);

            block.Elastic();
            Elastic();
            ParticleSystem JumpFx = SimplePool.Spawn(PrefabStorage.ins.FxJump);
            JumpFx.transform.position = transform.position;
            JumpFx.transform.rotation = Quaternion.Euler(-90f, 0, 0);
            limitDir = 1;
            _isPlaysound = false;
        }


        if (block && block.id != LastBlockID && block.id != 1 && block.id > LastBlockID)
        {
            _IsElastic = true;
            _canJump = true;
            _canDrawPath = true;
            _animator.SetInteger("Transition", 1);


            blockElastic(block);
            Elastic();

            _isPlaysound = false;


            if (GameManager.ins.uiController.UigamePlay.MaxTime <= 5)
            {
                SoundManager.Instance.SoundAudio.Stop();
                GameManager.ins.uiController.UigamePlay.isClockCoutDown = false;
            }

            if (GameManager.ins.CurrentGameState == E_GameState.GamePlay)
            {
                CheckDistance(block, TypeSpawn);
            }

            GameManager.ins.CurrentLevel.L_BlockInLevel[GameManager.ins.CurrentLevel.L_BlockInLevel.Count - 2].tweenSink?.Kill();

            block.SickBlock();

            float randPosBlock = UnityEngine.Random.Range(3f, MaxRange);

            LastTypeJump = TypeSpawn;


            int rand = UnityEngine.Random.Range(0, 2);
            TypeSpawn = rand == 1 ? E_TypeSpawnBlock.Left : E_TypeSpawnBlock.Left;

            CountDirSpawn(block, TypeSpawn);



            GameManager.ins.CurrentLevel.CreateBlock(block.id + 1, TypeSpawn, randPosBlock);
            EventManager.EmitEvent(EventContains.UPDATE_TIME_JUMP, 0.1f);

            LastBlockID = block.id;
            FLip(TypeSpawn);

            PrefabStorage.ins.CamFake.UpdateCamera(block);

            if (GameManager.ins.PlayMode == E_PlayMode.EndlessMode) return;

            GameManager.ins.AmoutBlockCurrent++;
            EventManager.EmitEvent(EventContains.CURRENT_BLOCK);

            /*            if (GameManager.ins.AmoutBlockCurrent == GameManager.ins.CurrentLevel.AmoutBlockInlevel)
                        {
                            StartCoroutine(IE_DelayWinLevel());
                            return;
                        }*/
        }
    }

    public void KillCurrentBrick()
    {
        GameManager.ins.CurrentLevel.L_BlockInLevel[GameManager.ins.CurrentLevel.L_BlockInLevel.Count - 2].tweenSink?.Kill();
    }

    public void CheckDistance(Block block, E_TypeSpawnBlock typespawn)
    {
        float distance = CheckDistaceBlock(block, typespawn);

        if (distance <= 0.2f)
        {
            GameManager.ins.uiController.PopupCombo.InitCombo(3);
            GameManager.ins.uiController.PopupCombo.gameObject.SetActive(true);

            ParticleSystem JumpFx = SimplePool.Spawn(PrefabStorage.ins.FxJumpPerfect);
            JumpFx.transform.position = transform.position;
            JumpFx.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        }
        else if (distance > 0.2f && distance <= 0.45f)
        {
            GameManager.ins.uiController.PopupCombo.InitCombo(2);
            GameManager.ins.uiController.PopupCombo.gameObject.SetActive(true);

            ParticleSystem JumpFx = SimplePool.Spawn(PrefabStorage.ins.FxJumpGrate);
            JumpFx.transform.position = transform.position;
            JumpFx.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        }
        else
        {
            GameManager.ins.uiController.PopupCombo.InitCombo(1);
            GameManager.ins.uiController.PopupCombo.gameObject.SetActive(true);

            ParticleSystem JumpFx = SimplePool.Spawn(PrefabStorage.ins.FxJumpGood);
            JumpFx.transform.position = transform.position;
            JumpFx.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        }
    }

    public float CheckDistaceBlock(Block block, E_TypeSpawnBlock Typespawn)
    {
        if (Typespawn == E_TypeSpawnBlock.Left)
        {
            Vector3 globalPos = block.Point_y.transform.position;
            return distance = Mathf.Abs(globalPos.z - transform.position.z);
        }
        else
        {
            Vector3 globalPos = block.Point_y.transform.position;
            return distance = Mathf.Abs(globalPos.x - transform.position.x);
        }
    }
    public void Elastic()
    {
        tweenElastic = transform.DOScaleY(3f, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            _IsElastic = false;
        });
    }

    public void blockElastic(Block block)
    {
        block.Elastic();
    }

    public void CountDirSpawn(Block block, E_TypeSpawnBlock typespawn)
    {
        if (block && block.id != LastBlockID && block.id != 1 && block.id > LastBlockID && TypeSpawn == LastTypeJump)
        {
            limitDir++;

            if (limitDir >= 3 && limitDir <= 5)
            {
                int rand = UnityEngine.Random.Range(0, 2);

                if (rand == 1)
                {
                    TypeSpawn = negativeTypeSpawn(typespawn);
                }

                else if (limitDir == 5)
                {
                    TypeSpawn = negativeTypeSpawn(typespawn);
                }
            }
        }
        else
        {
            limitDir = 1;
        }
    }

    public E_TypeSpawnBlock negativeTypeSpawn(E_TypeSpawnBlock typeSpawn)
    {

        if (typeSpawn == E_TypeSpawnBlock.Forward)
        {
            return E_TypeSpawnBlock.Left;
        }
        else
        {
            return E_TypeSpawnBlock.Forward;
        }
    }

    IEnumerator IE_delayChangeRevive()
    {
        yield return new WaitForSeconds(2f);
        GameManager.ins.uiController.ProcessWinLose(E_LevelResult.Revive);
    }


    public void DrawPathForward(float ForceJump, E_TypeSpawnBlock typeSpawn)
    {
        if (!_isShowPath)
        {
            Trajectory.ShowTrajection();
            _isShowPath = true;
        }

        if (typeSpawn == E_TypeSpawnBlock.Forward)
        {
            int index = GameManager.ins.CurrentLevel.L_BlockInLevel.Count;
            distance = (transform.position.y + _Force) - GameManager.ins.CurrentLevel.L_BlockInLevel[index - 1].Point_y.position.y;

            endPos = new Vector3(transform.position.x + ForceJump, GameManager.ins.CurrentLevel.L_BlockInLevel[index - 1].Point_y.position.y, transform.position.z);
            distance = Vector3.Distance(transform.position, endPos);
            Dir = new Vector3(0.4f, 0.8f, 0f);
            Force = Dir * distance * 2f;

            Trajectory.UpdateDots(this, Force);
        }
        else
        {
            int index = GameManager.ins.CurrentLevel.L_BlockInLevel.Count;
            distance = (transform.position.y + _Force) - GameManager.ins.CurrentLevel.L_BlockInLevel[index - 1].Point_y.position.y;

            endPos = new Vector3(transform.position.x, GameManager.ins.CurrentLevel.L_BlockInLevel[index - 1].Point_y.position.y, transform.position.z + ForceJump);
            distance = Vector3.Distance(transform.position, endPos);
            Dir = new Vector3(0f, 0.8f, 0.4f);
            Force = Dir * distance * 2f;

            Trajectory.UpdateDotsZ(this, Force);
        }
    }

    IEnumerator IE_DelayWinLevel()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.ins.uiController.ProcessWinLose(E_LevelResult.Win);
        ResetStatePlayer();
        PrefabStorage.ins.CameraVirtual.ResetStartPosVirtualCamera();
        yield return new WaitForSeconds(0.6f);
        GameManager.ins.IncreaseLevel(GameManager.ins.LevelPlaying);
        EventManager.EmitEvent(EventContains.UPDATEMAINUI);
    }

    public void OndisableListDots()
    {
        for (int i = Trajectory.L_dots.Count - 1; i >= 0; i--)
        {
            Destroy(Trajectory.L_dots[i].gameObject);
        }
    }

    public void ChangeJumpAFK()
    {
        StartCoroutine(IE_DelayChangeJumpAfterAFK());
    }

    IEnumerator IE_DelayChangeJumpAfterAFK()
    {
        yield return new WaitForSeconds(0.1f);
        JumpAfterInterAFK = true;
    }


    private void OnDisable()
    {
        tweenElastic?.Kill();
        TweenJump?.Kill();
    }
}
