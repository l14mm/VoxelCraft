using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour {

    public float wanderRadius;
    public float wanderTimer;
    public GameObject _arrow;
    public Transform arrowFirePosition;
    private float lastArrowFireTime = 0;
    public float arrowFireCooldown = 5;
    public Camera myCam;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    public bool wander = false;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        StartCoroutine(StartWandering(1));
    }

    private IEnumerator StartWandering(float time)
    {
        yield return new WaitForSeconds(time);
        wander = true;
    }

    void SearchForTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject temp in targets)
        {
            if (temp.GetComponent<Renderer>())
            {
                target = temp.transform;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer && wander)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        if(target)
        {
            // Check if the mob's camera can see the target
            if(target.GetComponent<Renderer>().IsVisibleFrom(myCam))
            {
                if(lastArrowFireTime + arrowFireCooldown < Time.time)
                {
                    lastArrowFireTime = Time.time;
                    transform.LookAt(target);
                    FireArrow();
                }
                else
                {

                }
            }
        }
        else
        {
            SearchForTarget();
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void ChargeArrow()
    {
        //arrowCharge += 0.05f;
    }

    void FireArrow()
    {
        // Rotate rotation to match forward of bow
        GameObject arrow = Instantiate(_arrow, arrowFirePosition.position, arrowFirePosition.rotation * Quaternion.Euler(0, 180, 0));

        float force = 20;
        arrow.GetComponent<Rigidbody>().AddForce(arrowFirePosition.forward * force, ForceMode.Impulse);
    }
}
