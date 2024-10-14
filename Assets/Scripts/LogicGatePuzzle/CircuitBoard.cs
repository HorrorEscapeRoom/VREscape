using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LogicGatePuzzle
{

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

		public void UpdateAllSocketState()
		{

            foreach (var switchitem in SwitchList)
			{
				foreach(var socketItem in switchitem.socketMapList)
				{
					var sockindex = socketItem.socketIndex;
					var inputindex = socketItem.inputIndex;

					UpdateInputOnSocket(socketItem, sockindex, inputindex, switchitem.active);
					UpdateSocketOuput(sockindex);
                }
			}

			foreach(var socket in ICList)
			{
                foreach(var socketItem in socket.ToUpdateIndexs)
				{
                    var sockindex = socketItem.socketIndex;
                    var inputindex = socketItem.inputIndex;

                    UpdateInputOnSocket(socketItem, sockindex, inputindex, socket.output);
                    UpdateSocketOuput(sockindex);
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
                            UpdateLogicGateState(socketIndex, socketref.inputA, EnumInputOuputType.InputA);                            
                            break;
                        case 1:
                            socketref.inputB = newState;
                            UpdateLogicGateState(socketIndex, socketref.inputB, EnumInputOuputType.InputB);                            
                            break;
                    }                    
                    break;
            }

            Debug.Log($"UpdateInputOnSocket - SocketIndex:({socketIndex}), OutputType:{SocketMap.outputType}, inputIndex:{inputIndex}, newState:{newState}");
        }

		private void UpdateSocketOuput(int socketIndex)
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
            Debug.Log($"UpdateSocketOuput - SocketIndex:({socketIndex}), GateType:{ICList[socketIndex].logicType}, Inputs:{ICList[socketIndex].inputA}-{ICList[socketIndex].inputB}, Output:{ICList[socketIndex].output}");
            UpdateLogicGateState(socketIndex, ICList[socketIndex].output, EnumInputOuputType.Output);
            
        }

        private void UpdateLogicGateState(int socketIndex, bool newState, EnumInputOuputType InputOutputType)
        {
            if (ICList[socketIndex].ICObject == null)
            {
                Debug.LogError($"CircuitBoard - Invalid ICObject in UpdateLogicGateState: ({socketIndex})");
                return;
            }
            
            switch (InputOutputType)
            {
                case EnumInputOuputType.InputA:                
                    ICList[socketIndex].ICObject.GetComponent<LogicGateScript>().SetInputOutputState(EnumInputOuputType.InputA, newState);
                    break;
                case EnumInputOuputType.InputB:
                    ICList[socketIndex].ICObject.GetComponent<LogicGateScript>().SetInputOutputState(EnumInputOuputType.InputB, newState);
                    break;
                case EnumInputOuputType.Output:
                    ICList[socketIndex].ICObject.GetComponent<LogicGateScript>().SetInputOutputState(EnumInputOuputType.Output, newState);
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

            UpdateAllSocketState();
        }

        public void AddICToSocket(int socketIndex, GameObject ICObject)
        {			
			if (ICList.Count == 0 || socketIndex > ICList.Count - 1)
            {
				Debug.LogError($"CircuitBoard - Invalid socketIndex in AddICToSocket: ({socketIndex})");
				return;
			}

            ICList[socketIndex].ICObject = ICObject;                             
            UpdateAllSocketState();
        }

        public void RemoveICFromSocket(int socketIndex) 
        {
			if (ICList.Count == 0 || socketIndex > ICList.Count - 1)
			{
				Debug.LogError($"CircuitBoard - Invalid socketIndex in RemoveICToSocket: ({socketIndex})");
				return;
			}

            ICList[socketIndex].ICObject = null;                   
            UpdateAllSocketState();
        }               
    }
}
