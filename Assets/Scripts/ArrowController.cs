using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 몬스터의 몸에 충돌하는 경우, 몬스터에 꽂힌 상태로 움직여야 하기 때문에
        // 충돌객체의 자식으로 설정함
        transform.SetParent(collision.transform);
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
