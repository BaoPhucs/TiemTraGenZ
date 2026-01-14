using UnityEngine;
using UnityEngine.AI; 

public class XeTuLai : MonoBehaviour
{
    public float banKinhTimKiem = 50f;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        DiDenDiemNgauNhien();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            DiDenDiemNgauNhien(); 
        }
    }

    void DiDenDiemNgauNhien()
    {
        Vector3 randomDirection = Random.insideUnitSphere * banKinhTimKiem;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, banKinhTimKiem, 1))
        {
            agent.SetDestination(hit.position); 
        }
    }
}