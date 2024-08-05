using UnityEngine;

public enum EnumLogicGateType
{
    UNSET,
    AND,
    OR,
    NAND,
    NOR,
    XOR
}


public class LogicGateScript : MonoBehaviour
{
    public GameObject inputA;
    public GameObject inputB;
    public GameObject output;

    public EnumLogicGateType gateType;

    public EnumLogicGateType GetGateType()
    {
        return gateType;
    }

    public void ToggleInputA(bool value)
    {
        inputA.SetActive(value);
    }

    public void ToggleInputB(bool value)
    {
        inputB.SetActive(value);
    }

    public void ToggleOutput(bool value)
    {
        output.SetActive(value);
    }

}
