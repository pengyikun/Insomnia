using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INonAITarget
{
    void OnDamaged(int amount);
    void OnKilled();
}
