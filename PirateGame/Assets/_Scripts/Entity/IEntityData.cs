using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityData
{
    EntityData GetEntityData ();
    void OnDamageReceive (float damage = 0);
    void CallDisableObject ();
}