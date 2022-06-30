/* Author : Mehmet Bedirhan U?ak*/
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Animations.Rigging;

public class HitDetection : MonoBehaviour
{
    private CollectManager _collectManager;
    private GameManager _gameManager;
    private PlayerManager _playerManager;
    private ComponentManager _componentManager;
    [BoxGroup("Player Options")]
    public PlayerMovementController playerMovementController;
    [BoxGroup("Player GameObject")]
    public GameObject Player;

    public GameObject Skate;
    public GameObject Bicycle;
    public GameObject Motorcycle;

    public GameObject MotorcycleRig;
    public GameObject BicycleRig;

    public ParticleSystem vehicleParticle;
    //public ParticleSystem winConfetti;

    [BoxGroup("Player Value Options")]
    public int PlayerDamageCount = 1;
    [BoxGroup("Player Value Options")]
    public int CollectedCoinCount = 1;

    [BoxGroup("Player Value Options")]
    public int CollectedCrystalCount = 1;

    [Header("Character Controller Required")]
    [BoxGroup("Player Jump Value Options")]
    public float JumpForce;
    [BoxGroup("Player Value Options")]
    public float JumpHeight;

    

    private void Awake()
    {
        _collectManager = CollectManager.Instance;
        _gameManager = GameManager.Instance;
        _playerManager = PlayerManager.Instance;
        _componentManager = ComponentManager.Instance;
        
        
        CancelBiking();
        CancelSkating();
        CancelMotorcycle();
    }

    void Update()
    {
        if (_componentManager.TimerSliderComponent.value <= 0)
        {
            CancelBiking();
            CancelSkating();
            CancelMotorcycle();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            Vibration.VibratePop();
            vehicleParticle.Play();

            CancelSkating();
            CancelBiking();
            CancelMotorcycle();

            if (other.GetComponent<DeadOnHit>())
            {
                _playerManager.AddDamage(_playerManager.PlayerMaxDeadCount);
                _collectManager.ObstacleObjects.Add(other.gameObject);
                //other.gameObject.SetActive(false);
            }
            else{
                _playerManager.AddDamage(PlayerDamageCount);
                _collectManager.ObstacleObjects.Add(other.gameObject);
                //other.gameObject.SetActive(false);
            }
           
        }

        if (other.tag == "Collectable" || other.tag == "Skateboard" || other.tag == "Bicycle" || other.tag == "Motorcycle" || other.tag == "TimePickup")
        {
            other.gameObject.SetActive(false);
            _collectManager.CollectedObjects.Add(other.gameObject);

            if (other.GetComponent<CrystalMode>())
            {
                _collectManager.AddCrystal(CollectedCrystalCount);
                _collectManager.CollectedObjects.Add(other.gameObject);
                
            }
            else
            {
                other.gameObject.SetActive(false);

                if (other.tag == "Collectable")
                {
                _collectManager.AddCoin(CollectedCoinCount);
                
                }
                
            }

            if (other.GetComponent<TimeMode>())
            {
                if (other.GetComponent<TimeMode>().isIncrease)
                {
                    _gameManager.UpdateGameTimeState(TimeState.AddTime, other.GetComponent<TimeMode>().IncreaseTimeAmount);
                    _collectManager.CollectedObjects.Add(other.gameObject);
                    other.gameObject.SetActive(false);
                    Debug.Log("time increase picked");
                }
                else
                {
                    _gameManager.UpdateGameTimeState(TimeState.DecreaseTime, other.GetComponent<TimeMode>().DecreaseTimeAmount);
                    _collectManager.CollectedObjects.Add(other.gameObject);
                    other.gameObject.SetActive(false);
                    Debug.Log("time decrease picked");
                }
            }
        }

        if (other.tag == "Skateboard")
        {
            Debug.Log("Skateboard Picked");
            vehicleParticle.Play();
            Skate.gameObject.SetActive(true);

            _playerManager.isSkating = true;
            _playerManager.PlayerSpeed = _playerManager.DefaultPlayerSpeed;

            _playerManager.PlayerAnimationController.SetBool("isSkating", true);

            CancelBiking();
            CancelMotorcycle();

            if (_playerManager.HavePlayerAnimations)
            {
                if (_playerManager.isSkating)
                {
                    _playerManager.PlayerSpeed += 2;
                    _playerManager.PlayerAnimationController.SetBool("isBackState", false);
                    
                }
            }

            CollectManager.Instance.CollectedCoin -= 100;
        }
        
        if (other.tag == "Bicycle")
        {
            Debug.Log("Bicycle Picked");

            vehicleParticle.Play();
            _playerManager.isBiking = true;
            Bicycle.gameObject.SetActive(true);
            _playerManager.PlayerSpeed = _playerManager.DefaultPlayerSpeed;
            _playerManager.PlayerAnimationController.SetBool("isBiking", true);
           
            CancelSkating();
            CancelMotorcycle();

            BicycleRig.GetComponent<Rig>().weight = 1;

            if (_playerManager.HavePlayerAnimations)
            {
                if (_playerManager.isBiking)
                {
                    _playerManager.PlayerSpeed += 3;
                    //_playerManager.PlayerAnimationController.SetBool("isBackState", false); 
                }
            }

            CollectManager.Instance.CollectedCoin -= 250;
        }
        
        if (other.tag == "Motorcycle")
        {
            Debug.Log("Motorcycle Picked");

            vehicleParticle.Play();
            _playerManager.isMotorcycle = true;
            _playerManager.PlayerSpeed = _playerManager.DefaultPlayerSpeed;
            _playerManager.PlayerAnimationController.SetBool("isMotorcycle", true);

            Motorcycle.gameObject.SetActive(true);
            MotorcycleRig.GetComponent<Rig>().weight = 1;

            CancelBiking();
            CancelSkating();

            if (_playerManager.HavePlayerAnimations)
            {
                if (_playerManager.isMotorcycle)
                {
                    _playerManager.PlayerSpeed += 5;
                }
            }

            CollectManager.Instance.CollectedCoin -= 500;

        }

        if (other.tag == "Finish")
        {
            _gameManager.winConfetti.gameObject.SetActive(true);
            _gameManager.winConfetti.Play();
            CancelBiking();
            CancelSkating();
            CancelMotorcycle();
            
            _gameManager.UpdateGameState(GameState.WinGame);
        }
    }

    public void CancelSkating()
    {
        _playerManager.PlayerAnimationController.SetBool("isSkating", false);
        Skate.gameObject.SetActive(false);
        _playerManager.isSkating = false;
        
    }

    public void CancelBiking()
    {
        BicycleRig.GetComponent<Rig>().weight = 0;
        Bicycle.gameObject.SetActive(false);
        _playerManager.PlayerAnimationController.SetBool("isBiking", false);
        _playerManager.isBiking = false;
    }

    public void CancelMotorcycle()
    {
        _playerManager.PlayerAnimationController.SetBool("isMotorcycle", false);
        MotorcycleRig.GetComponent<Rig>().weight = 0;
        Motorcycle.gameObject.SetActive(false);
        _playerManager.isMotorcycle = false;
    }
}