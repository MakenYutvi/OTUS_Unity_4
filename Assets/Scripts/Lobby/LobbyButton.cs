using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;

public class LobbyButton : MonoBehaviour
{
    RoomInfo Info;

    public void Init(RoomInfo info)
    {
        Info = info;
        GetComponentInChildren<TextMeshProUGUI>().text = info.Name;
    }
    [PunRPC]
    public void Join()
    {
        PhotonNetwork.JoinRoom(Info.Name);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(2, PlayerPrefs.GetString("NameOfPlayer"), raiseEventOptions, sendOptions);
    }
}
