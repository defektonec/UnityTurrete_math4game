using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField][Range(20f, 180f)] private float turretHorizontalAngle;
    [SerializeField][Range(0f, 90)] private float turretVerticalAngle;
    [SerializeField][Range(0.1f, 2)] private float turreteRotationSpeed;
    [SerializeField] private string[] targetTags;

    private float currentHorizontalAngle;
    private float currentVerticalAngle;

    private Vector3 directionToTarget;
    private Quaternion defaultLookingDirection;

    private List<GameObject> targetList = new List<GameObject>();
    private Transform currentTargetTransform;

    void Start()
    {
        defaultLookingDirection = transform.rotation;

        if (targetTags.Length > 0 )
        {
            Debug.LogWarning("No enemy tags!");
        }
    }

    void Update()
    {
        //Return to the original position
        if (transform.rotation.eulerAngles != Vector3.zero && isActiveAndEnabled
        && !CheckEnemyVisibility())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultLookingDirection, Time.deltaTime * 2f);
        }
        // If there is no enemy in list just return
        if (targetList.Count == 0) 
            return;
        else
        {
            CalculateTheNearestTarget();
            currentTargetTransform = targetList[0].transform;
        }

        directionToTarget = currentTargetTransform.position - transform.position;

        Vector3 flatDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);
        currentHorizontalAngle = Vector3.Angle(transform.forward, flatDirection.normalized);
        currentVerticalAngle = Vector3.Angle(flatDirection, directionToTarget);

        if (CheckEnemyVisibility())
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentTargetTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turreteRotationSpeed);
        }
    }

    private bool CheckEnemyVisibility()
    {
        // TODO
        // Need to develop obstacle cheking  

        //Check if enemy is within angle of turret
        if (currentHorizontalAngle < turretHorizontalAngle && currentVerticalAngle < turretVerticalAngle)
        {
            return true;
        }

        return false;
    }

    private void CalculateTheNearestTarget()
    {
        if (targetList.Count == 0) return;

        GameObject nearestTarget = targetList[0];
        float minDistance = Vector3.Distance(transform.position, targetList[0].transform.position);
        float currentDistance;
        int index = 0;

        for (int i = 0; i < targetList.Count - 1; i++)
        {
            currentDistance = Vector3.Distance(transform.position, targetList[i].transform.position);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                index = i;
            }
        }

        if (index != 0)
        {
            targetList[0] = targetList[index];
            targetList[index] = nearestTarget;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetTags.Any(tag => other.CompareTag(tag)))
        {
            targetList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetTags.Any(tag => other.CompareTag(tag)))
        {
            targetList.Remove(other.gameObject);
        }
    }
}
