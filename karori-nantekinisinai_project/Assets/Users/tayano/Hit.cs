using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    [SerializeField]
    private player player;

    private GameObject hitObj;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("L1"))
        {
            if(hitObj!=null)
            {
                hitObj.SetActive(false);
                player.force_variable += 1;
            }
            else
            {
                Debug.Log(hitObj+"hitObjが存在していません");
            }
           
        }
        if (Input.GetButtonDown("R1"))
        {
            if( hitObj!=null)
            {
                hitObj.SetActive(false);
                player.force_variable++;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag ("ground"))
        {
            hitObj = other.gameObject;
         
        }
        if (other.gameObject.CompareTag("wall"))
        {
            hitObj = other.gameObject;
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "floor")
        {
            hitObj = null;
            
        }
        if(other.gameObject.name == "wall")
        {
            hitObj = null;
        }
    }

   
}
