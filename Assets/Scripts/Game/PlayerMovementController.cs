/* Author : Mehmet Bedirhan U?ak*/
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

public class PlayerMovementController : MonoBehaviour
{

    private float? lastMousePoint = null;
    [BoxGroup("Player Options ")]
    public float RestirictionX = 3.5f;
    [BoxGroup("Player Mesh Transform")]
    public GameObject PlayerHolder;
    [BoxGroup("Camera Options")]
    public GameObject PlayerFollower;
    private Vector3 oldPos;
    private Quaternion oldRot;
    private bool mouseControl;
    private PlayerManager _playerManager;

    [BoxGroup("Rotation Mode Options")]
    public bool PlayerRotateEnabled;
    [BoxGroup("Rotation Mode Options")]
    public float RotatationDegree;
    [BoxGroup("Rotation Mode Options")]
    public float RotationSpeed;
    private float _oldPosition;

    private CharacterController _characterController;


    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        PlayerFollower.gameObject.transform.parent = this.transform;
        _characterController.enabled = false;
    }

    private void LateUpdate()
    {
        _oldPosition = PlayerHolder.transform.localPosition.x;
    }

    private void Update()
    {

        transform.Translate(Vector3.forward * Time.deltaTime * _playerManager.PlayerSpeed);
      
        if (!mouseControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePoint = Input.mousePosition.x;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lastMousePoint = null;
            }
            if (lastMousePoint != null)
            {
                float difference = Input.mousePosition.x - lastMousePoint.Value;
                PlayerHolder.transform.position = new Vector3(PlayerHolder.transform.position.x + (difference / 188) * Time.deltaTime * _playerManager.PlayerSideSpeed, PlayerHolder.transform.position.y, PlayerHolder.transform.position.z);
                lastMousePoint = Input.mousePosition.x;
            }

            float xPos = Mathf.Clamp(PlayerHolder.transform.position.x, -RestirictionX, RestirictionX);
            PlayerHolder.transform.position = new Vector3(xPos, PlayerHolder.transform.position.y, PlayerHolder.transform.position.z);
            Vector3 movement = oldRot * (PlayerHolder.transform.position - oldPos);

            if (PlayerRotateEnabled)
            {
                if (PlayerHolder.transform.localPosition.x > _oldPosition)
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, -RotatationDegree, 0f), RotationSpeed);
                }

                else if (PlayerHolder.transform.localPosition.x < _oldPosition)
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, RotatationDegree, 0f), RotationSpeed);
                }
                else
                {
                    PlayerHolder.transform.DORotate(new Vector3(0f, 0f, 0f), RotationSpeed);
                }
                _oldPosition = PlayerHolder.transform.localPosition.x;
            }

        }
    }
}
