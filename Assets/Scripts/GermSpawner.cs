using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermSpawner : MonoBehaviour
{
    public GameObject Germ;
    int germCount;
    public GameObject[] Germs = new GameObject[10];

    float rightPos;
    float leftPos;
    float upPos;
    float downPos;
    // Start is called before the first frame update
    void Start()
    {
        rightPos = transform.position.x + 5;
        leftPos = transform.position.x - 4;
        upPos = transform.position.y + 4;
        downPos = transform.position.y - 2;
        for (int i = 0; i < 10; i++){
            Germs[i] = Instantiate(Germ, new Vector3(Random.Range(leftPos, rightPos), Random.Range(downPos, upPos), 0), transform.rotation);
            Germs[i].name = $"Germ {i}";
            if(i > 0){
                Germs[i].GetComponent<Rigidbody2D>().simulated = false;
                Germs[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        
        germCount = 1;
        GameObject.Find($"pointer").GetComponent<ArrowScript>().setGermCount(germCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reset(int x){
        germCount = x;
        for (int i = 0; i < 10; i++){
            if(i < x){
                Germs[i].transform.position = new Vector3(Random.Range(leftPos, rightPos), Random.Range(downPos, upPos), 0);
                Germs[i].GetComponent<Rigidbody2D>().simulated = true;
                Germs[i].GetComponent<SpriteRenderer>().enabled = true;
            }else{
                Germs[i].GetComponent<Rigidbody2D>().simulated = false;
                Germs[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            
        }
        GameObject.Find($"pointer").GetComponent<ArrowScript>().setGermCount(germCount);
    }
}
