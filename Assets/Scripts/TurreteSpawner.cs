using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class TurreteSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    [Range(5,20)][SerializeField] private float distance;

    private Vector3 directionY;
    private Vector3 directionX;
    private Vector3 directionZ;
    private RaycastHit hit;


    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, layers))
        {
            directionY = hit.normal;
            directionX = Vector3.Cross(directionY, transform.position - hit.point).normalized;
            directionZ = Vector3.Cross(directionY, directionX).normalized;
        }
    }

    private void OnDrawGizmos()
    {
        if (hit.collider != null)
        {
            //Draw local Y
            Gizmos.color = Color.green;
            Gizmos.DrawLine(hit.point, hit.point + directionY * 2);

            //Draw local X
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hit.point, hit.point + directionX * 2);

            //Draw local Z
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(hit.point, hit.point + directionZ * 2);
        }
    }
}
