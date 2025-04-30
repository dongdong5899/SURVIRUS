using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector2 dir;

    [SerializeField]
    private GameObject _player;


    void Update()
    {
        dir = _player.transform.position - transform.position;

        if (dir.y > 20)
            transform.position += Vector3.up * 20;
        else if(dir.y < -20)
            transform.position += Vector3.down * 20;
        else if (dir.x > 20)
            transform.position += Vector3.right * 20;
        else if (dir.x < -20)
            transform.position += Vector3.left * 20;
    }
}
