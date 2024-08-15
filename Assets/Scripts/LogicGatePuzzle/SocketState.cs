using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LogicGatePuzzle
{
    [Serializable]
    public class SocketState
    {
        public bool inputA;
        public bool inputB;
        public bool output;
		public EnumLogicGateType logicType 
        { 
            get
            {
                if (ICObject != null)
                {
                    if (ICObject.TryGetComponent<LogicGateScript>(out var scriptObject))
                    {
                        return scriptObject.GetGateType();
                    }
                    else
                    {
                        //Throw object away or throw error?
                        Debug.LogError("SocketState - logicType - ICObject doest not contain component 'LogicGateScript'");
                        ICObject = null;                        
                        return EnumLogicGateType.UNSET;
                    }                    
                }
                else
                {
                    return EnumLogicGateType.UNSET;
                }
            }            
        }
        public GameObject socket;
        public GameObject ICObject;
        public List<SocketMap> ToUpdateIndexs;
	
	}
}
