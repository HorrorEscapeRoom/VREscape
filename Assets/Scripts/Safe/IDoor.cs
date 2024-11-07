using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoor 
{
    bool IsOpen { get; }
    bool IsLocked { get; }
    void Open();
    void Close();
}
