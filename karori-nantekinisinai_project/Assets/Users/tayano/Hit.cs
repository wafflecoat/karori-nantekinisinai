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
           // GameObject child = transform.GetChild(0).gameObject;
            //child.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "floor")
        {
            hitObj = other.gameObject;
            // collision.transform.SetParent(this.gameObject.transform);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    //if (collision.gameObject.name == "Hit_ground")
    //    if (collision.gameObject.name == "floor")
    //    {
    //        hitObj = collision.gameObject;
    //       // collision.transform.SetParent(this.gameObject.transform);
    //    }
    //    Debug.Log("ìñÇΩÇ¡ÇƒÇ¢ÇÈ");
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "floor")
        {
            hitObj = null;
            //this.gameObject.transform.DetachChildren();
        }
    }

   /* private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.name == "floor")
        if (collision.gameObject.name == "Hit_ground")
        {
            hitObj = null;
            //this.gameObject.transform.DetachChildren();
        }
        Debug.Log("ó£ÇÍÇΩ");
    }*/
}
