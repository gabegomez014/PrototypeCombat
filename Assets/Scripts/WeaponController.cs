using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject hitVFX;
    public int damage = 5;
    public GameObject GetHitVFX() {
        return hitVFX;
    }

    public int GetDamage() {
        return damage;
    }
}
