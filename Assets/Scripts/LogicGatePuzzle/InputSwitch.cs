using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LogicGatePuzzle
{
    [Serializable]
	public class InputSwitch
	{
		public bool active = false;
		public GameObject SwitchObject;
		[SerializeField]
		public List<SocketMap> socketMapList;
	}
}
