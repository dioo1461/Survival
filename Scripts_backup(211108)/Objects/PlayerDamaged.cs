using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    Rigidbody2D rigid2D;
	HealthMNG healthMNG;
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }
	
    // Update is called once per frame
    void Update()
    {
		   
    }
}
