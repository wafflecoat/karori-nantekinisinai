using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit2 : MonoBehaviour
{
    [SerializeField]
    private player2 player;

    private GameObject hitObj;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("L1_2"))
        {
            if(hitObj!=null)
            {
                hitObj.SetActive(false);
                player.force_variable += 1;
            }
            else
            {
                Debug.Log(hitObj+"hitObjÇ™ë∂ç›ÇµÇƒÇ¢Ç‹ÇπÇÒ");
            }
           
        }
        if (Input.GetButtonDown("R1_2"))
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
