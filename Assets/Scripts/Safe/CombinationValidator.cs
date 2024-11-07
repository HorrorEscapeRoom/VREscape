using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationValidator : ICombinationValidator
{
    [SerializeField] List<int> correctNumbers; 
    int indexOfCorrectDigit;

    public CombinationValidator(List<int> combination)
    {
        correctNumbers = combination;
        ResetCombination();
    }

    public bool ValidateDigit(int digit)
    {
        if (digit == correctNumbers[indexOfCorrectDigit])
        {
            indexOfCorrectDigit++;
            return LastDigitReached;
        }
        return false;
    }

    public void ResetCombination() => indexOfCorrectDigit = 0;

    public bool LastDigitReached => indexOfCorrectDigit == correctNumbers.Count - 1;
}
