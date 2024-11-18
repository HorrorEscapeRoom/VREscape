using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LogicGatePuzzle
{
    public class CircuitBoard: PuzzleBase {
		public List<InputSwitch> SwitchList = new();
		public List<GameObject> LightList = new();
        public List<SocketState> ICList = new();
        [SerializeField]
        Material lightRed;
        [SerializeField]
        Material lightGreen;

        private bool _verboseLog;
        public void Start()
        {
	        _verboseLog = false;
	        RegisterWithOrchestrator();
            UpdateAllSocketState();
        }

		public void UpdateAllSocketState()
		{

            foreach (var switchItem in SwitchList)
			{
				foreach(var socketItem in switchItem.socketMapList)
				{
					var sockIndex = socketItem.socketIndex;
					var inputIndex = socketItem.inputIndex;

					UpdateInputOnSocket(socketItem, sockIndex, inputIndex, switchItem.active);
					UpdateSocketOutput(sockIndex);
                }
			}

			foreach(var socket in ICList)
			{
                foreach(var socketItem in socket.ToUpdateIndexs)
				{
                    var sockIndex = socketItem.socketIndex;
                    var inputIndex = socketItem.inputIndex;

                    UpdateInputOnSocket(socketItem, sockIndex, inputIndex, socket.output);
                    UpdateSocketOutput(sockIndex);
                }				
            }

            CheckIsComplete();
        }

        private void UpdateInputOnSocket(SocketMap SocketMap, int socketIndex, int inputIndex, bool newState)
        {
			var socketRef = ICList[socketIndex];			
			
			switch (SocketMap.outputType)
            {
                case EnumOutputType.Light:
                    UpdateLightState(SocketMap.socketIndex, newState);
                    break;
                case EnumOutputType.Socket:
                    switch (inputIndex)
                    {
                        case 0:
	                        socketRef.inputA = newState;
                            UpdateLogicGateState(socketIndex, socketRef.inputA, EnumInputOuputType.InputA);                            
                            break;
                        case 1:
	                        socketRef.inputB = newState;
                            UpdateLogicGateState(socketIndex, socketRef.inputB, EnumInputOuputType.InputB);                            
                            break;
                    }                    
                    break;
            }
			
			if (_verboseLog)
			{
				Debug.Log($"UpdateInputOnSocket - SocketIndex:({socketIndex}), OutputType:{SocketMap.outputType}, inputIndex:{inputIndex}, newState:{newState}");
			}
        }

		private void UpdateSocketOutput(int socketIndex)
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

            if (_verboseLog)
            {
	            Debug.Log(
		            $"UpdateSocketOutput - SocketIndex:({socketIndex}), GateType:{ICList[socketIndex].logicType}, Inputs:{ICList[socketIndex].inputA}-{ICList[socketIndex].inputB}, Output:{ICList[socketIndex].output}");
            }

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

	        LightList[index].GetComponent<MeshRenderer>().material = newState ? lightGreen : lightRed;
        }

		private void UpdateSwitchState(int index, bool newState)
		{
			if (SwitchList.Count == 0 || index > SwitchList.Count - 1)
			{
				Debug.LogError($"CircuitBoard - Invalid switchIndex in UpdateSwitchState: ({index})");
				return;
			}

			SwitchList[index].active = newState;

            SwitchList[index].SwitchObject.GetComponent<MeshRenderer>().material = newState ? lightGreen : lightRed;

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
        
        private void CheckIsComplete()
        {
            if (LightList.All(x => x.GetComponent<MeshRenderer>().material == lightGreen))
            {
	            if (_verboseLog)
	            {
		            Debug.Log($"CircuitBoard - Puzzle Is Completed.");
	            }
	            OnPuzzleComplete(true);
            }
        }
    }
}
