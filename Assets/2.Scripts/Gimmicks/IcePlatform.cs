using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IcePlatform : MonoBehaviour 
{
    
    [Header("감속 설정")]
    [SerializeField, Range(0.9f,1f)] private float slideDamping   = 0.99f;
    [SerializeField, Range(1f,30f)]  private float iceAcceleration = 20f;
    [SerializeField, Range(1f,20f)]  private float maxIceSpeed     = 8f;
    
    public void ApplyEffect(Rigidbody rb, Vector3 inputDir, float moveSpeed)
    {
        Vector3 horiz = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (inputDir.magnitude < 0.1f)
        {
            horiz *= slideDamping;
        }
        else
        {
            Vector3 target = inputDir * maxIceSpeed;
            horiz = Vector3.MoveTowards(horiz, target, iceAcceleration * Time.fixedDeltaTime);
        }

        rb.velocity = new Vector3(horiz.x, rb.velocity.y, horiz.z);
    }
}
