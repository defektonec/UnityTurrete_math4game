using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject NewGameObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NewGameObject.SetActive(!NewGameObject.activeSelf);
        }
    }
}
