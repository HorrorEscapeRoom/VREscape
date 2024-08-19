using TMPro;
using UnityEngine;

public class LogicGateScript : MonoBehaviour
{
    public GameObject inputA;
    public GameObject inputB;
    public GameObject output;
    public Color ActiveColour = Color.green;
    public Color InactiveColour = Color.red;

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
        switch (IntputOutputType)
        {
            case EnumInputOuputType.InputA:
                inputA.GetComponent<MeshRenderer>().materials[0].color = value == true ? ActiveColour : InactiveColour;
                break;
            case EnumInputOuputType.InputB:
                inputB.GetComponent<MeshRenderer>().materials[0].color = value == true ? ActiveColour : InactiveColour;
                break;
            case EnumInputOuputType.Output:
                output.GetComponent<MeshRenderer>().materials[0].color = value == true ? ActiveColour : InactiveColour;
                break;
        }
    }
}
