using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable  
{
    Item Item { get; }
    void Use();
}
