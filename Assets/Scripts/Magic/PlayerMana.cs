using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMesh ManaBar;
    Mana _mana;

    void Start()
    {
        _mana = GetComponent<Mana>();
    }

    // Update is called once per frame
    void Update()
    {
        // HealthBar.text = ammo.Count <= 0.0f ? "0" : $"{ammo.Count}";
        ManaBar.text = _mana.Count.ToString();
    }
}
