using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementManager : MonoBehaviour
{
    private CharacterController cc;
    private AnimManager am;

    [Header("Camera")]
    public Transform camCenter; // Tham chiếu đến camera
    public CinemachineFreeLook freeLook;

    [Space]
    [Header("Aim")]
    private bool isAiming = false;
    private float normalFieldOfView = 40f;
    private float aimFieldOfView = 20f;

    RaycastHit hit;
    Ray ray;
    [SerializeField] private LayerMask aimLayers;

    [Space]
    [Header("Spine")]
    [SerializeField] private Transform spine;
    [SerializeField] private Vector3 spineOffset;

    [Space]
    [Header("Sound")]
    [SerializeField] private AudioSource footStep;
    [SerializeField] private AudioClip footClip;
    [SerializeField] private AudioClip runFootClip;

    public Bow bow;
    private bool isBowEquipped = false;

    bool hitDetected;

    [SerializeField] private float Gravity = -9.81f;

    private float timeSinceLastShoot = 0f;
    private float shootDelay = 1.5f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        cc = GetComponent<CharacterController>();
        am = GetComponent<AnimManager>();

        if(freeLook != null)  
            freeLook.m_Lens.FieldOfView = normalFieldOfView;
    }

    private void Update()
    {
        timeSinceLastShoot += Time.deltaTime;
        bool _leftMouse = Input.GetMouseButton(0); 

        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        if(isAiming)
        {
            Aim();
            bow.EquipBow();
            am.CharacterPullString(_leftMouse);
            if (Input.GetMouseButtonUp(0) && timeSinceLastShoot >= shootDelay)
            {
                am.CharacterFireArrow();
                if(hitDetected)
                {
                    bow.Fire(hit.point);
                }else
                {
                    bow.Fire(ray.GetPoint(300f));
                }
                timeSinceLastShoot = 0f;
            }
        }else
        {
            bow.UnEquipBow();
            bow.CancelCrossHair();
            DisableArrow();
            Release();
        }

        if (freeLook != null)
        {
            float _targetFOV = isAiming ? aimFieldOfView : normalFieldOfView;
            freeLook.m_Lens.FieldOfView = Mathf.Lerp(freeLook.m_Lens.FieldOfView, _targetFOV, Time.deltaTime * 10f); // Chuyển FOV mượt mà
        }

        am.CharacterAim(isAiming);        
    }

    private void FixedUpdate()
    {
        float _hInput = Input.GetAxisRaw("Horizontal");
        float _vInput = Input.GetAxisRaw("Vertical");
        bool _sprint = Input.GetKey(KeyCode.LeftShift);

        Vector3 _forward = camCenter.forward; 
        Vector3 _right = camCenter.right;

        _forward.y = 0;
        _right.y = 0;

        _forward.Normalize();
        _right.Normalize();

        Vector3 _direction = (_forward * _vInput + _right * _hInput).normalized;



    //     /*        // Chỉ xoay khi đi thẳng
    //             if(_vInput > 0 && _direction.magnitude > 0.1f) 
    //             {
    //                 Quaternion _targetRotation = Quaternion.LookRotation(_direction);
    //                 transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.fixedDeltaTime * 10f); 
    //             }*/


        // Xoay nhân vật theo camera bất kể di chuyển hay không
        if (_direction.magnitude > 0.1f || isAiming || _hInput != 0 || _vInput != 0)
        {
            Quaternion _targetRotation = Quaternion.LookRotation(_forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.fixedDeltaTime * 10f); // Quay theo camera
        }

        am.AnimateCharacter(_hInput, _vInput, _sprint);
    }


    private void LateUpdate()
    {
        if (isAiming) RotateCharacterSpine();
    }

    void Aim()
    {
        Vector3 _camPos = camCenter.position;
        Vector3 _dir = camCenter.forward;

        ray = new Ray(_camPos, _dir);
        if(Physics.Raycast(ray, out hit, 500f, aimLayers))
        {
            hitDetected = true;
            Debug.DrawLine(ray.origin, hit.point, Color.yellow);
            bow.ShowCrossHair(hit.point);
        }else
        {
            hitDetected = false;
            bow.CancelCrossHair();
        }
    }

    void RotateCharacterSpine()
    {
        spine.LookAt(ray.GetPoint(50));
        spine.Rotate(spineOffset);
    }

    public void Pull()
    {
        bow.PullString();
    }

    public void EnableArrow()
    {
        bow.PickArrow();
    }

    public void DisableArrow()
    {
        bow.DisableArrow();
    }

    public void Release()
    {
        bow.ReleaseString();
    }

    public void BowPullSound()
    {
        bow.PullSound();
    }

    public void PlayerFootSound()
    {
        footStep.PlayOneShot(footClip);
    }
    public void PlayerRunFootSound()
    {
        footStep.PlayOneShot(runFootClip);
    }
}
