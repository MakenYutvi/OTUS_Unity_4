using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeroGenerator : MonoBehaviour
{
    public BotUtility Prefab;
    public Transform Cube;

    public Transform SpawnPoint;
    public Transform[] WayPoint;
    private float Timer;
    private float Interval = 8;
    private List<Hero> Heroes = new List<Hero>();
    private int _Length;

    private void Start()
    {
         Instantiate(Cube);
      //  _Length = WayPoint.Length;

    }

    private void Update()
    {
        if (Timer < 0)
        {
            Timer = Interval;
            //var pers = Instantiate(Prefab, SpawnPoint.position, Quaternion.identity, transform);
            var pers = PhotonNetwork.Instantiate(Prefab.name, SpawnPoint.position, Quaternion.identity, 0);
          //  Heroes.Add(pers);
          //  var point = WayPoint[Random.Range(0, WayPoint.Length)];
            //Debug.Log(_Length);
         //   pers.Agent.SetDestination(point.position);
          //  Heroes[Random.Range(0, WayPoint.Length)].Agent.SetDestination(point.position);
        }
        else Timer -= Time.deltaTime;
    }
}
