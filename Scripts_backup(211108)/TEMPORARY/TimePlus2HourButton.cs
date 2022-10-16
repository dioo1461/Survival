using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlus2HourButton : MonoBehaviour
{
	public void OnMouseUpAsButton() {
		GameManager.timeElapsed += 4f * 3600f;
	}

	void Start()
    {
        
    }


    void Update()
    {
        
    }
}
