using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypothermiaButton : MonoBehaviour
{
	public DiseasesMNG diseasesMNG;

	public void OnMouseUpAsButton() {
		diseasesMNG.Hypothermia_Occur();
	}
	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
