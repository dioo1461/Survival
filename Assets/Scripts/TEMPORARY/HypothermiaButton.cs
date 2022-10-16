using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypothermiaButton : MonoBehaviour
{
	public DiseasesMGR diseasesMNG;

	public void OnMouseUpAsButton() {
		diseasesMNG.Incur_Hypothermia();
	}
	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
