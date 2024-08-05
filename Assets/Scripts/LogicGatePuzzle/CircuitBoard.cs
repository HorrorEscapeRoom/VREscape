using JetBrains.Annotations;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.LogicGatePuzzle
{

    [System.Serializable]
    public struct GateState
    {
        public bool inputA;
        public bool inputB;
        public bool output;        
        public GameObject socket;
    }

    public class CircuitBoard: MonoBehaviour
    {
        public GameObject SumOutLight;
        public GameObject CarryOutLight;

        public List<GateState> ListOfIC = new();

        public bool SwitchAIn;
        public bool SwitchBIn;
        public bool SwitchCIn;

        public void GetNewState(GateState ICState)
        {

            LogicGateScript ScriptOfIC = null;// ICState.IC.GetComponent<StorageVolume>()..GetComponent<LogicGateScript>();
            switch (ScriptOfIC.gateType)
            {
                case EnumLogicGateType.NAND:
                    ICState.output = !(ICState.inputA & ICState.inputA);
                    break;
            }


            ScriptOfIC.ToggleInputA(ICState.inputA);
            ScriptOfIC.ToggleInputB(ICState.inputB);
            ScriptOfIC.ToggleOutput(ICState.output);

        }

        public void UpdateLogic(int socketIndex, EnumLogicGateType ICType) {

            //Do some checks for out of range etc.
            ListOfIC[socketIndex].socket.GetComponent<LogicGateScript>().gateType = ICType;

            // Hide/Unhide IC in Socket
            if (ICType == EnumLogicGateType.UNSET)
            {
                ListOfIC[socketIndex].socket.SetActive(false);
            }
            else
            {
                ListOfIC[socketIndex].socket.SetActive(true);
            }
            
            // Update Overall Socket state
            





            
            //objectChanged.GetComponent<LogicGateScript>().
            

                
            //IC1State.inputA = SwitchAIn;


            //foreach (GateState IC in ICs) {
            //    GetNewState(IC);
            //}


        }



        public void AddICToSocket(int socketIndex, EnumLogicGateType ICType)
        {
            if (ListOfIC.Count == 0 || socketIndex > ListOfIC.Count - 1)
            {
                //Invalid
            }
            
            UpdateLogic(null);
        }

        public void RemoveICToSocket(int socketIndex) 
        {
            UpdateLogic(null);
        }               
    }
}
