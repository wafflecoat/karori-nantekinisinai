using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{

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
            }
            else
            {
                Debug.Log(hitObj+"hitObjÇ™ë∂ç›ÇµÇƒÇ¢Ç‹ÇπÇÒ");
            }
           
        }
        if (Input.GetButtonDown("R1"))
        {
            if( hitObj!=null)
            {
                hitObj.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "floor")
        {
            hitObj = other.gameObject;
         
        }
        if (other.gameObject.name == "wall")
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
