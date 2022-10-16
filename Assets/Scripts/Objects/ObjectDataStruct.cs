using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataStruct : MonoBehaviour
{
	public static ObjectDataStruct singleton;

    void Start()
    {
		singleton = this;
    }

	[Header("Objects")]
	public ObjectData Tree;

}
