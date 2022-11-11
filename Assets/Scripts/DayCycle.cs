using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public float rotation;
    public float speed;

    void Update()
    {
        transform.Rotate( transform.eulerAngles.x + rotation * speed, 0, 0);
    }
}
