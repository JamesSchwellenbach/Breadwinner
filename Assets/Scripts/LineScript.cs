using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    private GameObject pointer;
    // Start is called before the first frame update
    void Start()
    {
        pointer = GameObject.Find($"pointer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(){
        //Debug.Log("trigger");
        pointer.GetComponent<ArrowScript>().germCollision();
    }
}
