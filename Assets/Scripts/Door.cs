using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeId = 0;
    SaveLoadManager saveLoadManager;

    // Start is called before the first frame update
    void Start()
    {
        saveLoadManager = GameObject.FindObjectOfType<SaveLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ItemKeeper.hasKeys > 0)
            {
                ItemKeeper.hasKeys--;
                saveLoadManager.SetSceneData(this.gameObject.name, false);
                Destroy(this.gameObject);                
            }
        }
    }
}
