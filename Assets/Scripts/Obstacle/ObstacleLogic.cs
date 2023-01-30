using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLogic : MonoBehaviour
{
    //[SerializeField] Animator animator;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public bool SetDestroyed(bool destroyed = false)
    {
        Destroy(gameObject);
        return destroyed; 
    }
}
