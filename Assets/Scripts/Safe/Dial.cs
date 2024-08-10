using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Dial : MonoBehaviour
{
    const int _sections = 12;
    const float _rotationSpeed = 100;
    const float _waitTime = 1.0f;

    Dictionary<int, int> _angleToNumberMap;
    Dictionary<int, int> _correctCombinationOrder;
    Dictionary<int, int> _currentCombination;
    List<int> _positions;

    float _timer;
    bool _isRotatable;
    int _anglePerSection;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        _angleToNumberMap = new Dictionary<int, int>();
        _correctCombinationOrder = new Dictionary<int, int>
        {
            { 7, 1 },
            { 2, 2 },
            { 5, 3 },
            { 9, 4 }
        };
        _currentCombination = new Dictionary<int, int>();
        _positions = new List<int>();

        _anglePerSection = 360 / _sections;

        for (int i = 1; i <= _sections; i++)
        {
            int angle = i * _anglePerSection;
            _angleToNumberMap[angle] = i;
        }
    }

    void UpdateGameState(int number) => _positions.Add(IsCorrectDialPosition(number) ? 1 : 0);

    void UpdateTimer()
    {
        _timer += Time.deltaTime;
        if (_timer == _waitTime)
        {
            _isRotatable = true;
            _timer = 0.0f;
        }
        _isRotatable = false;
    }

    bool CheckNumberExists(int number) => _correctCombinationOrder.ContainsKey(number);

    bool IsCorrectDialPosition(int number) =>
        CheckNumberExists(number) && _correctCombinationOrder[number] == _currentCombination[number];

    void PlaySound(int number)
    {
        if (IsCorrectDialPosition(number))
        {
            // Play sound to indicate the bolt has locked
        }
        // Play other sound
    }

    void UpdateCurrentCombination(int number)
    {
        if (_currentCombination.Count >= _correctCombinationOrder.Count)
        {
            _currentCombination.Clear();
        }

        if (CheckNumberExists(number) && !_currentCombination.ContainsKey(number))
        {
            _currentCombination.Add(number, _currentCombination.Count + 1);
        }
    }

    void ManageRotation(int number)
    {
        if (_isRotatable && _currentCombination.Count < _correctCombinationOrder.Count)
        {
            PlaySound(number);
            UpdateCurrentCombination(number);
            UpdateGameState(number);
        }
    }

    public void OnRotate(float currentAngle)
    {
        UpdateTimer();

        if (_angleToNumberMap.TryGetValue((int)currentAngle, out int number))
        {
            ManageRotation(number);

            float rotationInput = Input.GetAxis("Horizontal");
            transform.Rotate(rotationInput * _rotationSpeed * Time.deltaTime, 0, 0);
        }
    }

    void Update()
    {
        
    }
}
