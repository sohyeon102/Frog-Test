using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedFallPlatform : MonoBehaviour
{
    [Header("설정")] 
    [SerializeField] private float fallDelay = 3f; 
    
    private bool triggered = false; 

    private void OnCollisionEnter(Collision other)
    {
        if (!triggered && other.gameObject.CompareTag("Player"))
        {
            triggered = true;
            Invoke(nameof(Fall), fallDelay);
        }
    }

    private void Fall()
    {
        gameObject.SetActive(false); 
    }
}
