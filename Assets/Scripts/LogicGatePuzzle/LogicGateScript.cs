using Assets.Scripts.LogicGatePuzzle;
using TMPro;
using UnityEngine;

public class LogicGateScript : MonoBehaviour
{
    public GameObject inputA;
    public GameObject inputB;
    public GameObject output;
    public Material ActiveColour;
    public Material InactiveColour;

    public EnumLogicGateType gateType;
    [SerializeField] TextMeshProUGUI TextObject;

    public void Start()
    {
        TextObject.text = gateType.ToString();
    }

    public EnumLogicGateType GetGateType()
    {
        return gateType;
    }

    public void SetInputOutputState(EnumInputOuputType IntputOutputType, bool value)
    {
        //Material currentMaterial;

        //if (value==true)
        //{
        //    currentMaterial = ActiveColour;
        //}
        //else
        //{
        //    currentMaterial = InactiveColour;
        //}

        switch (IntputOutputType)
        {
            case EnumInputOuputType.InputA:
                inputA.GetComponent<MeshRenderer>().material = value ? ActiveColour : InactiveColour;
                break;
            case EnumInputOuputType.InputB:
                inputB.GetComponent<MeshRenderer>().material = value ? ActiveColour : InactiveColour;
                break;
            case EnumInputOuputType.Output:
                output.GetComponent<MeshRenderer>().material = value ? ActiveColour : InactiveColour;
                break;
        }
    }
}
