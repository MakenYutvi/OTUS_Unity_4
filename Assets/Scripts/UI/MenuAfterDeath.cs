using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace DefaultNamespace
{ 
public class MenuAfterDeath : MonoBehaviour
{
        private PlayerHP _playerHP;
        [SerializeField] private Canvas _MenuAfterDeath;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _exitButton;

       // Start is called before the first frame update
       void Start()
    {
            _playerHP = GetComponent<PlayerHP>();
            _MenuAfterDeath.gameObject.SetActive(false);
    }
        private void Awake()
        {
            _resumeButton.onClick.AddListener(OnResumeButtonClick); 
            _exitButton.onClick.AddListener(OnExitButtonClick); 


        }
        // Update is called once per frame
        void Update()
    {
        if (_playerHP.Health <= 0)
            {
                _MenuAfterDeath.gameObject.SetActive(true);
            }
    }

        private void OnExitButtonClick()
        {
            Debug.Log("Exit");
            PhotonNetwork.Disconnect();
            gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("InitialScene");

        }
        private void OnResumeButtonClick()
        {
            Debug.Log("Resume");
            //PhotonNetwork.JoinRoom();
            gameObject.SetActive(false);
            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}
