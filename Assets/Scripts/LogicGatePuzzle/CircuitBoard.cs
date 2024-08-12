using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings.Switch;
using static UnityEditor.Rendering.CameraUI;
using UnityEngine.Windows;

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
		public GameObject SwitchObject;
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
	}

    public class CircuitBoard: MonoBehaviour
    {
		[SerializeField]
		public List<InputSwitch> SwitchList = new();
		[SerializeField]
		public List<GameObject> LightList = new();
		[SerializeField]
        public List<SocketState> ICList = new();

        public void Start()
        {
            UpdateAllSocketState();
        }

        public void UpdateLogic(int socketIndex, EnumLogicGateType ICType)
		{
	
			UpdateAllSocketState();
		}

		public void UpdateAllSocketState()
		{

            foreach (var switchitem in SwitchList)
			{
				foreach(var socketItem in switchitem.socketMap)
				{
					var sockindex = socketItem.socketIndex;
					var inputindex = socketItem.inputIndex;

					UpdateInputOnSocket(socketItem, sockindex, inputindex, switchitem.active);
					UpdateSocketOuput(sockindex, inputindex);
                }
			}

			foreach(var socket in ICList)
			{
                foreach(var socketItem in socket.ToUpdateIndexs)
				{
                    var sockindex = socketItem.socketIndex;
                    var inputindex = socketItem.inputIndex;

                    UpdateInputOnSocket(socketItem, sockindex, inputindex, socket.output);
                    UpdateSocketOuput(sockindex, inputindex);
                }				
            }
		}

        private void UpdateInputOnSocket(SocketMap SocketMap, int socketIndex, int inputIndex, bool newState)
        {
			var socketref = ICList[socketIndex];			
			
			switch (SocketMap.outputType)
            {
                case EnumOutputType.Light:
                    UpdateLightState(SocketMap.socketIndex, newState);
                    break;
                case EnumOutputType.Socket:
                    switch (inputIndex)
                    {
                        case 0:
                            socketref.inputA = newState;
							Debug.Log($"{socketref.socket.name} InputA({newState}");
                            break;
                        case 1:
                            socketref.inputB = newState;
                            Debug.Log($"{socketref.socket.name} InputB({newState}");
                            break;
                    }
                    break;
            }			
        }

		private void UpdateSocketOuput(int socketIndex, int inputIndex)
		{            
            // Hide/show ic lights
            switch (ICList[socketIndex].logicType)
            {
                case EnumLogicGateType.AND:
                    ICList[socketIndex].output = ICList[socketIndex].inputA & ICList[socketIndex].inputB;
                    break;
                case EnumLogicGateType.OR:
                    ICList[socketIndex].output = ICList[socketIndex].inputA | ICList[socketIndex].inputB;
                    break;
                case EnumLogicGateType.XOR:
                    ICList[socketIndex].output = ICList[socketIndex].inputA ^ ICList[socketIndex].inputB;
                    break;
                case EnumLogicGateType.NAND:
                    ICList[socketIndex].output = !(ICList[socketIndex].inputA & ICList[socketIndex].inputB);
                    break;
                case EnumLogicGateType.NOR:
                    ICList[socketIndex].output = !(ICList[socketIndex].inputA | ICList[socketIndex].inputB);
                    break;
                case EnumLogicGateType.UNSET:
                default:
                    ICList[socketIndex].output = false;
                    break;
            }
        }

        private void UpdateLightState(int index, bool newState)
		{
			if (LightList.Count == 0 || index > LightList.Count - 1)
			{
				Debug.LogError($"CircuitBoard - Invalid index in UpdateLightState: ({LightList})");
				return;
			}

			//LightList[index].SetActive(newState);

			if (newState)
			{
				LightList[index].GetComponent<MeshRenderer>().materials[0].color = Color.green;
			}
			else
			{
                LightList[index].GetComponent<MeshRenderer>().materials[0].color = Color.red;
            }
		}

		private void UpdateSwitchState(int index, bool newState)
		{
			if (SwitchList.Count == 0 || index > SwitchList.Count - 1)
			{
				Debug.LogError($"CircuitBoard - Invalid switchIndex in UpdateSwitchState: ({index})");
				return;
			}

			SwitchList[index].active = newState;

            if (newState)
            {
                SwitchList[index].SwitchObject.GetComponent<MeshRenderer>().materials[0].color = Color.green;
            }
            else
            {
                SwitchList[index].SwitchObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            }

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
