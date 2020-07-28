
using UnityEngine;
using UnityEngine.AI;

public class BotUtility : MonoBehaviour
{
    //Gun gun;
    Spell spell;
    NavMeshAgent navMeshAgent;

    void Awake()
    {
     //   gun = GetComponentInChildren<Gun>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spell = GetComponentInChildren<Spell>();
    }

    T FindClosest<T>()
        where T : Component
    {
        Vector3 myPosition = transform.position;
        T closest = null;
        float closestSqrDistance = 0.0f;
        foreach (var obj in FindObjectsOfType<T>()) {
            if (obj.gameObject == gameObject)
                continue;

            float sqrDist = (obj.transform.position - myPosition).sqrMagnitude;
            if (closest == null || sqrDist < closestSqrDistance) {
                closest = obj;
                closestSqrDistance = sqrDist;
            }
        }
        return closest;
    }

    public HealthPack FindClosestHealth()
    {
        return FindClosest<HealthPack>();
    }

    public AmmoPack FindClosestAmmo()
    {
        return FindClosest<AmmoPack>();
    }

    public PlayerCollider FindClosestPlayer()
    {
        return FindClosest<PlayerCollider>();
    }

    public float GetDistanceToClosestEnemy()
    {
        PlayerCollider closestEnemy = FindClosestPlayer();
        if (closestEnemy == null)
            return Mathf.Infinity;
        return (closestEnemy.transform.position - transform.position).magnitude;
    }

    public bool NavigateTo(Component target)
    {
        if (navMeshAgent == null || target == null)
            return false;

        navMeshAgent.SetDestination(target.transform.position);
        navMeshAgent.isStopped = false;

        return true;
    }

    public bool IsNavigating()
    {
        if (navMeshAgent.isStopped)
            return false;

        if (!navMeshAgent.pathPending &&
            navMeshAgent.remainingDistance <= 1.0f &&
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.000001f))
                return false;

        return true;
    }

    public bool BeginAttack(PlayerCollider target)
    {
        if (target == null)
            return false;

        navMeshAgent.isStopped = true;

        Vector3 start = transform.position;
        Vector3 end = target.transform.position + new Vector3(Random.Range(-0.5f, 0.5f),0, Random.Range(-0.5f, 0.5f));
        start.y += 1.0f;
        end.y += 1.0f;

        Vector3 direction = (end - start).normalized;
        start += direction * 0.5f;

        transform.rotation = Quaternion.LookRotation(direction);
        Ray ray = new Ray(start, direction);
        Debug.DrawLine(start, end, Color.white, 5.0f);

        //gun.BeginAnimateShoot();
        spell.BeginAnimateSpellCast();
        spell.SpellCast(ray);
        return true;
       // return gun.Shoot(ray);
    }

    public void EndAttack()
    {
       // gun.EndAnimateShoot();
        spell.EndAnimateSpellCast();
    }
}
