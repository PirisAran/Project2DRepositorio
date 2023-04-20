using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageFire
{
    public float DamageDealt { get; }
    public void Destroy();
}
