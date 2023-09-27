using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePos;
    public GameObject bullet;

    public float bulletSpeed;
    public float bulletLife;
    public int bulletCount;

    public float coolDown;

    private void Update()
    {
        
    }
}
