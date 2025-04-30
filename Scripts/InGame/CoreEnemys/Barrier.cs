using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 180) * Time.deltaTime);
    }
}
