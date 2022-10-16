using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Scriptable Object/Object", order = 1)]
public class ObjectData : ScriptableObject
{
	public string Name => _name;
	public float Max_time_required_to_destroy => _max_time_required_to_destroy;
	public float TimeSpeed_acceleration => _timeSpeed_acceleration;
	public float Calories_required_per_hour => _calories_required_per_hour;
	public float Exhaustion_required_per_hour => _exhaustion_required_per_hour;
	public GameObject Prefab_interact_progress => _prefab_interact_progress;
	// public GameObject 

	[SerializeField] string _name;
	[SerializeField] float _max_time_required_to_destroy;
	[SerializeField] float _timeSpeed_acceleration;
	[SerializeField] float _calories_required_per_hour;
	[SerializeField] float _exhaustion_required_per_hour;
	[SerializeField] GameObject _prefab_interact_progress;
}
