using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpin : MonoBehaviour
{
    public float rotateSpeed;
 
    private void Update()
    {
        Debug.Log("im spining");
        transform.Rotate(0, rotateSpeed, 0);
    }
}
