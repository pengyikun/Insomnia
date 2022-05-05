using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    Item Item { get; }

    void OnInteract();
}
