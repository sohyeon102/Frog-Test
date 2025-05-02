using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private Vector3 moveOffset = new Vector3(5, 0, 0); 
    [SerializeField] private float moveSpeed = 2f; 
    
    private Vector3 startPos; 
    private Vector3 targetPos; 
    private bool movingToTarget = true;

    private void Start()
    {
        startPos = transform.position - moveOffset * 0.5f;
        targetPos = transform.position + moveOffset * 0.5f;
    }

    private void Update()
    {
        Vector3 destination = movingToTarget ? targetPos : startPos;
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destination) < 0.05f)
        {
            movingToTarget = !movingToTarget;
        }
    }
}
