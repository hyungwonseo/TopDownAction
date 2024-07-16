using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    BossController boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindObjectOfType<BossController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null)
        {
            Destroy(gameObject);
        }
    }
}
