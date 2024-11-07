using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombinationValidator 
{
    bool ValidateDigit(int digit);
    void ResetCombination();
    bool LastDigitReached { get; }
}
