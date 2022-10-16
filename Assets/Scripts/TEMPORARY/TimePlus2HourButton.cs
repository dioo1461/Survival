using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlus2HourButton : MonoBehaviour
{
	public void OnMouseUpAsButton() {
		GameManager.singleton.TimeElapsed += 4f * 3600f;
	}

	void Start()
    {
        
    }


    void Update()
    {
        
    }
}
