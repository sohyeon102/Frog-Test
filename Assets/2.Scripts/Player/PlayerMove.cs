using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType
{
    Ground,
    IceGround
}
public class PlayerMove : MonoBehaviour
{
    //개구리 이동속도
    [SerializeField]private float moveSpeed = 3f;
    //메인 카메라
    [SerializeField] Camera playerCamera;
    //개구리 프리팹
    [SerializeField] private Transform player;
    //혀의 로프액션 클래스
    [SerializeField] private RopeAction ropeAction;
    
    private float cameraRotationSpeed = 3;
    private float mouseX, mouseY;
    private Rigidbody rb;
    private Vector3 cameraOffset = new Vector3(0, 2.5f, -4.5f);
    private bool _isGround;
    private Material mat;
    private GroundType groundType = GroundType.Ground;
    private int groundCheckCount = 0;
    
    public bool IsGround => _isGround;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;
    }
    

    private void Update()
    {
        CameraMovement();
        PlayerMovement();
        Debug.Log("지면확인"+_isGround);
        Debug.Log(groundType);
    }
    
    #region 플레이어 동작
    private void PlayerMovement()
    {
        // 키보드 입력감지
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // 마우스 입력 감지 * 마우스 회전속도
        mouseX += Input.GetAxis("Mouse X")*cameraRotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y")*cameraRotationSpeed;
        //카메라의 최대 각도 설정
        mouseY = Mathf.Clamp(mouseY, -90, 50);
        //카메라의 방향을 정의하고 y값을 초기화
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        //플레이어의 움직임과 카메라의 방향 동기화
        Vector3 movement = (cameraRight * moveHorizontal + cameraForward * moveVertical);
        movement.Normalize();
        //카메라의 전방과 플레이어 오브젝트의 전방을 동기화하는 변수
        Quaternion playerRotation = Quaternion.Euler(0,mouseX,0);
        transform.rotation = playerRotation;
        //플레이어 이동 + 회전
        if (ropeAction.IsGrappling && !_isGround)
        {
            rb.AddForce(movement, ForceMode.Acceleration);
        }
        else if( _isGround)
        {
            if (movement.magnitude >= 0.1f)
            {
                Vector3 targetVelocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);

                //테스트 이동 
                //rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
                switch (groundType)
                {
                    case GroundType.Ground:
                        rb.velocity = targetVelocity;
                        break;
                    case GroundType.IceGround:
                        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 2f * Time.deltaTime);
                        break;
                }
            }
            else
            {
                //rb.velocity = new Vector3(rb.velocity.x*0.9f, rb.velocity.y*0.9f, rb.velocity.z*0.9f);
                switch (groundType)
                {
                    case GroundType.Ground:
                        rb.velocity = new Vector3(rb.velocity.x*0.5f, rb.velocity.y*0.5f, rb.velocity.z*0.5f);
                        break;
                    case GroundType.IceGround:
                        rb.velocity = new Vector3(rb.velocity.x*0.98f, rb.velocity.y*0.98f, rb.velocity.z*0.98f);
                        if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude < 0.0001f)
                        {
                            rb.velocity = new Vector3(0, rb.velocity.y, 0);
                        }
                        break;
                }
            }
        }
        
    }

    private void CameraMovement()
    {
        //카메라 회전 계산
        Quaternion targetRotation = Quaternion.Euler(mouseY, mouseX, 0);
        //카메라 위치 선정
        playerCamera.transform.position = player.position + Vector3.up * 1.5f  + targetRotation * cameraOffset;
        //카메라의 방향을 타겟의 포지션에 고정
        playerCamera.transform.LookAt(player.position + Vector3.up * 2.5f);

        float heightDiff = playerCamera.transform.position.y - (player.position.y + 1.5f);

        float alpha = Mathf.Clamp01((heightDiff + 2f) / 2f);
        mat.SetColor("_Color",
            new Color(
                mat.color.r,
                mat.color.g,
                mat.color.b,
                //알파값을 조정해주는 코드 
                alpha));
        //(Math.Clamp(playerCamera.transform.position.y, -2, 0) + 2) / 2));
    }

    private void OnCollisionEnter(Collision other)
    {
        int layer = other.gameObject.layer;
        
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("IceGround"))
        {
            groundCheckCount++;
            _isGround = true;
            
            if(layer == LayerMask.NameToLayer("Ground"))
                groundType = GroundType.Ground;
            else if(layer == LayerMask.NameToLayer("IceGround"))
                groundType = GroundType.IceGround;
            
        }
    }

    private void OnCollisionExit(Collision other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("IceGround"))
        {
            groundCheckCount--;
            if (groundCheckCount <= 0)
            {
                _isGround = false;
                groundType = GroundType.Ground;
            }
        }
    }

    #endregion
    
    
}
