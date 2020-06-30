using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class InputNameOfPlayer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public Canvas _lobbyCanvac;
    public Canvas _InputNameCanvac;
    public TMP_InputField InputField;
    void Start()
    {
        _lobbyCanvac.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InputName()
    {
        _InputNameCanvac.gameObject.SetActive(false);
        Debug.Log(InputField.text);
        _lobbyCanvac.gameObject.SetActive(true);
        PlayerPrefs.SetString("NameOfPlayer", InputField.text);
        PhotonNetwork.NickName = InputField.text;
    }
}
