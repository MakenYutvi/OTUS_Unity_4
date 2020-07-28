using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DefaultNamespace
{
    public sealed class PlayerHP : MonoBehaviourPunCallbacks, IPunObservable
    {
        public float MaxHealth = 100.0f;
        public TextMesh HealthBar;
        private HitEffectAnimation _hitEffectAnimation;
        public float Health => _health;
        private float _health;
        bool isBot;

        private void Awake()
        {
            _health = MaxHealth;
            _hitEffectAnimation = GetComponentInChildren<HitEffectAnimation>();
        }

        private void Start()
        {
            GetDamageRPC(0.0f);
            isBot = GetComponent<BotUtility>() != null;

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_health);
            }
            else
            {
                _health = (float)stream.ReceiveNext();
            }
        }

        [PunRPC]
        private void GetDamageRPC(float damage)
        {
            _health -= damage;
            
            HealthBar.text = _health <= 0.0f ? "0" : $"{_health}";
            if(damage != 0.0f)
            {
                _hitEffectAnimation.PlayEffect();
            }
           
            if (_health <= 0.0f && photonView.IsMine)
                
            {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                SendOptions sendOptions = new SendOptions { Reliability = true };

                if (isBot)
                {
                    PhotonNetwork.RaiseEvent(1, gameObject.name, raiseEventOptions, sendOptions);
                    gameObject.SetActive(false);
                }
                else
                {
                    PhotonNetwork.RaiseEvent(1, PlayerPrefs.GetString("NameOfPlayer"), raiseEventOptions, sendOptions);
                    PhotonNetwork.RaiseEvent(1, PhotonNetwork.NickName, raiseEventOptions, sendOptions);
                   // gameObject.SetActive(false);
                }
                // PhotonNetwork.RaiseEvent(1, gameObject.name, raiseEventOptions, sendOptions);
              //  PhotonNetwork.LoadLevel("InitialScene");
              //  SceneManager.LoadScene("InitialScene");
            }
        }

        public void AddHealth(float add)
        {
            GetDamageRPC(-add);
        }
    }
}
