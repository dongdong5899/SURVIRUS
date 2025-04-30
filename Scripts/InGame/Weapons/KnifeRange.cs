using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeRange : MonoBehaviour
{
    public List<GameObject> Enemy = new List<GameObject>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy.Add(collision.gameObject);
    }
}
