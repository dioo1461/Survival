using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataStruct : MonoBehaviour
{
	public static ItemDataStruct singleton;

	public const int WEAPONS_START_ID = 1;
	public const int WEAPONS_END_ID = 999;
	public const int TOOLS_START_ID = 1000;
	public const int TOOLS_END_ID = 1999;
	public const int MATERIALS_START_ID = 2000;
	public const int MATERIALS_END_ID = 2999;
	public const int FOODS_START_ID = 3000;
	public const int FOODS_END_ID = 3999;
	public const int MEDS_START_ID = 4000;
	public const int MEDS_END_ID = 4999;

	void Awake() {
		singleton = this;
		empty = new ItemProp(empty_data, 0);
	}
	[SerializeField] ItemData empty_data;
	public ItemProp empty; 

	[Header("Weapons - 0001 ~ 0999")]
	public ItemData hatchet;

	[Header("Tools - 1000 ~ 1999")]
	public Tool_ItemData something_1;

	[Header("Materials - 2000 ~ 2999")]
	public ItemData branch;
	public ItemData stone;

	[Header("Foods - 3000 ~ 3999")]
	public Food_ItemData raw_fish;
	public Food_ItemData spoiled_fish;

	[Header("Meds - 4000 ~ 4999")]
	public ItemData something_2;
}
