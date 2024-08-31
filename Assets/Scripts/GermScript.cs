using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class GermScript : MonoBehaviour
{
    public float speed;
    Vector2 direction;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start() // spawn at random point on board, move in random diagonal direction
    {
        rb = GetComponent<Rigidbody2D>();
        int x = Random.Range(0,2);
        int y = Random.Range(0,2);
        direction = new Vector2(1.1f,1.0f).normalized;
        
        rb.velocity = direction * speed;
    }

    // void OnTriggerEnter2D(Collider2D col){
    //     //Variables.Object(col).Set("touchingGerm", true);
    //     //Debug.Log(col.name);
    // }
    // void OnTriggerExit2D(Collider2D col){
    //     //Variables.Object(col).Set("touchingGerm", false);
        
    //     //Debug.Log("exit");
    // }
}
