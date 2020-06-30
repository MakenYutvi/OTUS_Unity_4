using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMesh HealthBar;
    GunAmmo ammo;

    void Start()
    {
        ammo = GetComponent<GunAmmo>();
    }

    // Update is called once per frame
    void Update()
    {
      // HealthBar.text = ammo.Count <= 0.0f ? "0" : $"{ammo.Count}";
        HealthBar.text = ammo.Count.ToString();
    }
}
