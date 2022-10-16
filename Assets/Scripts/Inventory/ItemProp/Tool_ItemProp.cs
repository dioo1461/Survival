using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_ItemProp : ItemProp
{
	public new Tool_ItemData Data { get; private set; }

	public Tool_ItemProp(Tool_ItemData data, int amount) : base(data, amount) {

	}
}
