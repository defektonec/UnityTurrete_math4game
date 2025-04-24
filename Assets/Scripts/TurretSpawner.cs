using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    [SerializeField] private GameObject turretInstance;
    [Range(5,20)][SerializeField] private float distance;
    [Range(0,180)][SerializeField] private float maxSlope;

    private Vector3 directionY = default;
    private Vector3 directionX = default;
    private Vector3 directionZ = default;
    private RaycastHit hit;


    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, layers))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle <= maxSlope && Input.GetKeyDown(KeyCode.E))
            {
                //Surface-aligned placement position (slightly off to avoid z-fighting)
                Vector3 spawnPosition = hit.point + hit.normal * 0.05f;

                //Calculate player horizontal look direction projected onto surface
                Vector3 playerForward = transform.forward;
                Vector3 projectedForward = Vector3.ProjectOnPlane(playerForward, hit.normal).normalized;

                //Create rotation: forward = projected look, up = surface normal
                Quaternion spawnRotation = Quaternion.LookRotation(projectedForward, hit.normal);

                //Spawn the turret
                Instantiate(turretInstance, spawnPosition, spawnRotation);
            }
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
