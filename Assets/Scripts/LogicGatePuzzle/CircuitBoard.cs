using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.LogicGatePuzzle
{
	public enum EnumOutputType
	{
		Socket,
		Light
	}

	[Serializable]
	public class SocketMap {
		public int socketIndex;
		public int inputIndex;
		public EnumOutputType outputType;
	}

	[Serializable]
	public class InputSwitch
	{
		public bool active = false;
		[SerializeField]
		public List<SocketMap> socketMap;
	}

	[Serializable]
    public class SocketState
    {
        public bool inputA;
        public bool inputB;
        public bool output;
		public EnumLogicGateType logicType;
		public List<SocketMap> ToUpdateIndexs;
		//public int socketIndex;
		public GameObject socket;
		
		public void UpdateOutput() 
		{					
			
			// Hide/show ic lights
			switch (logicType)
			{
				case EnumLogicGateType.AND:
					output = inputA & inputB;
					break;
				case EnumLogicGateType.OR:
					output = inputA | inputB;
					break;
				case EnumLogicGateType.XOR:
					output = inputA ^ inputB;
					break;
				case EnumLogicGateType.NAND:
					output = !(inputA & inputB);
					break;
				case EnumLogicGateType.NOR:
					output = !(inputA | inputB);					
					break;
				case EnumLogicGateType.UNSET:
				default:
					output = false;
					break;
			}
		}
	}

    public class CircuitBoard: MonoBehaviour
    {
		//public GameObject SumOutLight;
		//public GameObject CarryOutLight;

		public void Update()
		{

			UpdateSwitchState(0, false);
			UpdateSwitchState(1, true);
			UpdateSwitchState(2, false);

			UpdateAllSocketState();

		}

		[SerializeField]
		public List<InputSwitch> SwitchList = new();
		[SerializeField]
		public List<GameObject> LightList = new();
		[SerializeField]
        public List<SocketState> ICList = new();
		
		public void UpdateLogic(int socketIndex, EnumLogicGateType ICType)
		{
	
			// Hide/Unhide IC in Socket
			//bool isSet = !(ICType == EnumLogicGateType.UNSET);
			//ICList[socketIndex].socket.SetActive(isSet);
	
			// Get switch state
			// This should already be set from the switches themselves.

			// Update all socket states
			UpdateAllSocketState();
		}

		private void UpdateAllSocketState()
		{
			// Propergate Switch states
			foreach (var switchitem in SwitchList) 
			{
				foreach(var SocketMap in switchitem.socketMap)
				{
					var socketToUpdate = ICList[SocketMap.socketIndex];

					switch (SocketMap.outputType)
					{
						case EnumOutputType.Light:
							UpdateLightState(SocketMap.socketIndex, socketToUpdate.output);
							break;
						case EnumOutputType.Socket:
							switch (SocketMap.inputIndex)
							{
								case 0:
									socketToUpdate.inputA = socketToUpdate.output;
									break;
								case 1:
									socketToUpdate.inputB = socketToUpdate.output;
									break;
							}
							break;
					}
				}
			}

			// Update Overall Socket state            
			foreach (SocketState state in ICList)
			{				
				state.UpdateOutput();

				foreach (var SocketMap in state.ToUpdateIndexs)
				{
					var socketToUpdate = ICList[SocketMap.socketIndex];

					switch (SocketMap.outputType)
					{
						case EnumOutputType.Light:
							UpdateLightState(SocketMap.socketIndex, socketToUpdate.output);
							break;
						case EnumOutputType.Socket:
							switch (SocketMap.inputIndex)
							{
								case 0:
									socketToUpdate.inputA = socketToUpdate.output;
									break;
								case 1:
									socketToUpdate.inputB = socketToUpdate.output;
									break;
							}
							break;
					}
				}
			}
		}
	

		private void UpdateLightState(int index, bool newState)
		{
			if (LightList.Count == 0 || index > LightList.Count - 1)
			{
				Debug.LogError($"CircuitBoard - Invalid index in UpdateLightState: ({LightList})");
				return;
			}

			LightList[index].SetActive(newState);
		}

		private void UpdateSwitchState(int index, bool newState)
		{
			if (SwitchList.Count == 0 || index > SwitchList.Count - 1)
			{
				Debug.LogError($"CircuitBoard - Invalid switchIndex in UpdateSwitchState: ({index})");
				return;
			}

			SwitchList[index].active = newState;
			UpdateLogic(0, 0);
		}

        public void AddICToSocket(int index, EnumLogicGateType ICType)
        {			
			if (ICList.Count == 0 || index > ICList.Count - 1)
            {
				Debug.LogError($"CircuitBoard - Invalid socketIndex in AddICToSocket: ({index})");
				return;
			}

			ICList[index].logicType = ICType;
			UpdateLogic(0,0);
        }

        public void RemoveICFromSocket(int socketIndex) 
        {
			if (ICList.Count == 0 || socketIndex > ICList.Count - 1)
			{
				Debug.LogError($"CircuitBoard - Invalid socketIndex in RemoveICToSocket: ({socketIndex})");
				return;
			}

			ICList[socketIndex].logicType = EnumLogicGateType.UNSET;
			UpdateLogic(0,0);
        }               
    }
}
