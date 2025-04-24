using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurreteScript : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    private Transform playerTransform;
    private float distance;
    private float currentHorizontalAngle;
    private float currentVerticalAngle;

    [SerializeField] [Range(20f, 180f)] private float turreteHorizontalAngle;
    [SerializeField] [Range(0f, 90)] private float turreteVerticalAngle;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        distance = directionToPlayer.magnitude;

        Vector3 flatDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z);
        currentHorizontalAngle = Mathf.Acos(Vector3.Dot(transform.forward, flatDirection.normalized)) * Mathf.Rad2Deg;
        currentVerticalAngle = Vector3.Angle(flatDirection, directionToPlayer);

        if (CheckPlayerVisibility())
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f); // 2f = speed of rotation
        }


        if (transform.rotation.eulerAngles != Vector3.zero && isActiveAndEnabled 
            && !CheckPlayerVisibility())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 2f);
        }
    }


    private bool CheckPlayerVisibility()
    {
        return distance < radius && currentHorizontalAngle < turreteHorizontalAngle && currentVerticalAngle < turreteVerticalAngle;
    }
}
