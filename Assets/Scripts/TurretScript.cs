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
    [SerializeField] private string[] targetTags;

    private float currentHorizontalAngle;
    private float currentVerticalAngle;

    private Vector3 directionToTarget;
    private Quaternion defaultLookingDirection;

    private List<GameObject> enemiesList = new List<GameObject>();
    private Transform currentEnemyTransform;

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
        if (enemiesList.Count == 0) 
            return;
        else 
            currentEnemyTransform = enemiesList[0].transform;

        directionToTarget = currentEnemyTransform.position - transform.position;

        Vector3 flatDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);
        currentHorizontalAngle = Vector3.Angle(transform.forward, flatDirection.normalized);
        currentVerticalAngle = Vector3.Angle(flatDirection, directionToTarget);

        if (CheckEnemyVisibility())
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemyTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        }
    }

    private bool CheckEnemyVisibility()
    {
        //Check if enemy is within angle of turret
        if (currentHorizontalAngle < turretHorizontalAngle && currentVerticalAngle < turretVerticalAngle)
        {
            //Check if there is no obstacles between enemy and turrete
            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, directionToTarget, out hit)
            //    && hit.transform.tag == enemiesList[0].gameObject.tag)
            //{
                return true;
            //}
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetTags.Any(tag => other.CompareTag(tag)))
        {
            enemiesList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetTags.Any(tag => other.CompareTag(tag)))
        {
            enemiesList.Remove(other.gameObject);
        }
    }
}
