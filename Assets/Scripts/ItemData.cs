using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ����
public enum ItemType
{
    arrow,      //ȭ��
    key,        //����
    life,	   //����
}

public class ItemData : MonoBehaviour
{
    public ItemType type;           //�������� ����
    public int count = 1;           //������ ��

    public int arrangeId = 0;       //�ĺ��� ���� ��

    SaveLoadManager saveLoadManager;

    // Start is called before the first frame update
    void Start()
    {
        saveLoadManager = GameObject.Find("SaveLoadManager").GetComponent<SaveLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //���� (����)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == ItemType.key)
            {
                //����
                ItemKeeper.hasKeys += 1;
            }
            else if (type == ItemType.arrow)
            {
                //ȭ��
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if (type == ItemType.life)
            {
                //����
                if (PlayerController.hp < 3)
                {
                    //HP�� 3���ϸ� �߰�
                    PlayerController.hp++;
                }
            }
            saveLoadManager.ChangeProps(this.gameObject.name, false);

            //++++ ������ ȹ�� ���� ++++
            //�浹 ���� ��Ȱ��
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            //�������� Rigidbody2D��������
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            //�߷� ����
            itemBody.gravityScale = 2.5f;
            //���� Ƣ������� ����
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            //0.5�� �ڿ� ����
            Destroy(gameObject, 0.5f);
        }
    }
}
