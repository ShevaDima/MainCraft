using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int health {get; set;}
    [SerializeField]
    private BlockTypes blockType;

    void Start()
    {
        health = (int)blockType;
    }


    void Update()
    {
        
    }

    public void DestroyBehaviour()
    {
        GameObject miniBlock = Resources.Load<GameObject>("mini" + blockType.ToString());
        Instantiate(miniBlock, transform.position, Quaternion.identity);
        miniBlock.GetComponent<Rigidbody>().AddForce(800f,0,0, ForceMode.Impulse);
        miniBlock.GetComponent<Rigidbody>().velocity = new Vector3(5f, 10f, 0f);
        Destroy(gameObject);
        
    }
}
