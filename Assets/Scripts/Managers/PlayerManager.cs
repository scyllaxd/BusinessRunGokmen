/* Author : Mehmet Bedirhan U?ak*/
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

public class PlayerManager : Singleton<PlayerManager>
{
    private GameManager _gameManager;
    [BoxGroup("Player Gameobject")]
    public GameObject Player;
    //[HideInInspector]
    public float PlayerSpeed;
    [BoxGroup("Player Movement Options")]
    public float DefaultPlayerSpeed = 6;
    [BoxGroup("Player Movement Options")]
    public float PlayerSideSpeed = 50;
    private float _backupSideSpeed;
    [BoxGroup("Player Dead Count Options")]
    public int PlayerMaxDeadCount = 3;
    [BoxGroup("Player Dead Count Options")]
    [OnValueChanged("OnValueChangedCallback")]
    public int PlayerCurrentDeadCount = 0;
    [BoxGroup("Player Animation Controller")]
    public bool HavePlayerAnimations;
    [BoxGroup("Player Animation Controller")]
    public Animator PlayerAnimationController;
    [BoxGroup("Player Animation Controller")]
    public bool PlayerWalkingMode;
    [BoxGroup("Player Animation Controller")]
    public bool PlayerWinDance;
    [BoxGroup("Player Animation Controller")]
    public bool PlayerWinNoDance;
    public bool isSkating;
    public bool isBiking;
    public bool isMotorcycle;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _backupSideSpeed = PlayerSideSpeed;
    }

    #region Player Start And Stop Options
    public void StopPlayer(bool isLose,bool isWinDance,bool isWin)
    {
        PlayerSpeed = 0;
        PlayerSideSpeed = 0;
        if (HavePlayerAnimations)
        {
            PlayerAnimationController.SetBool("isBackState", false);

            if (isWinDance)
                PlayerAnimationController.SetBool("isWinDance", true);
            if (isWin)
                PlayerAnimationController.SetBool("isWin", true);
            if (isLose)
                PlayerAnimationController.SetBool("isLose", true);
        }
    }

    /// <summary>
    /// IdleStatePlayer();
    /// For Joypad controls only.
    /// </summary>
    public void IdleStatePlayer()
    {
        if (HavePlayerAnimations)
        {
           PlayerAnimationController.SetBool("isWalking", false);
           PlayerAnimationController.SetBool("isRuning", false);
        }
    }

    public void StartPlayer(bool isRuning)
    {
        PlayerSpeed = DefaultPlayerSpeed;
        PlayerSideSpeed = _backupSideSpeed;
        if (HavePlayerAnimations)
        {
            PlayerAnimationController.SetBool("isBackState", false);
            if (!isRuning)
                PlayerAnimationController.SetBool("isWalking", true);
            else
                PlayerAnimationController.SetBool("isRuning", true);
        }
       
    }

    public async void RestartPlayer()
    {
        resetPlayerAnimationStates();
        Player.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Player.transform.position = new Vector3(0, 1, 0);
        Player.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
        PlayerCurrentDeadCount = 0;
        await Task.Delay(100);
        Player.transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().enabled = true;
        _gameManager.UpdateGameState(GameState.StartGame);
    }

    private void resetPlayerAnimationStates()
    {
        if (HavePlayerAnimations)
        {
            PlayerAnimationController.SetBool("isWalking", false);
            PlayerAnimationController.SetBool("isRuning", false);
            PlayerAnimationController.SetBool("isLose", false);
            PlayerAnimationController.SetBool("isWinDance", false);
            PlayerAnimationController.SetBool("isWin", false);
            PlayerAnimationController.SetBool("isBackState", true);
        }
  
    }
    #endregion

    #region Player Health Options
    public void AddDamage(int amount)
    {
        PlayerCurrentDeadCount += amount;
        OnValueChangedCallback();
    }
    #endregion

    private void OnValueChangedCallback()
    {
        if(PlayerCurrentDeadCount >= PlayerMaxDeadCount)
        {
            _gameManager.UpdateGameState(GameState.LoseGame);
        }
    }

}
