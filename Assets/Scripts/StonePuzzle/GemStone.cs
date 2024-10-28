using UnityEngine;

    public class GemStone: MonoBehaviour
    {
        public EnumGemColour color;
        public EnumGemType gemType;
        public int value;

        private MaterialColourTransitionWithCurve childObject;
        private void Start()
        {
            childObject = GetComponentInChildren<MaterialColourTransitionWithCurve>();
            if (childObject == null)
            {
                Debug.LogError($"GemStone - MaterialColourTransitionWithCurve not found in children");
            }

            Color newEndColor = Color.black;
            switch (color)
            {
                case EnumGemColour.Red:
                    ColorUtility.TryParseHtmlString("#FF0000", out newEndColor);
                    break;
                case EnumGemColour.Yellow:
                    ColorUtility.TryParseHtmlString("#FFFF00", out newEndColor);
                    break;
                case EnumGemColour.Blue:
                    ColorUtility.TryParseHtmlString("#0000FF", out newEndColor);
                    break;
                case EnumGemColour.Green:
                    ColorUtility.TryParseHtmlString("#00FF00", out newEndColor);
                    break;
                case EnumGemColour.Purple:
                    ColorUtility.TryParseHtmlString("#9600FF", out newEndColor);
                    break;
                case EnumGemColour.Orange:
                    ColorUtility.TryParseHtmlString("#FF8900", out newEndColor);
                    break;
                default:
                    Debug.LogError($"GemStone - MaterialColourTransitionWithCurve SetEndColor has invalid Enum value");
                    return;
            }

            if (newEndColor == Color.black)
            {
                Debug.LogError($"GemStone - Invalid Color for endColor");
                return;
            }
            
            childObject.endColor = newEndColor;
        }
    }
