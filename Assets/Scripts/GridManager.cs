using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    public GameObject Tile;
    public GameObject TileParent;

    [SerializeField] private Transform _cam;
    // Start is called before the first frame update
    void Start(){
        GenerateGrid();
    }
    void GenerateGrid(){
        for (int x = 0; x < _width; x++){
            for (int y = 0; y < _height; y++){
                if(x < 2 || x > 125 || y < 2 || y > 87){
                    var spawnedTile = Instantiate(Tile, new Vector3((float)x/10,(float)y/10), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";
                    spawnedTile.transform.parent = TileParent.transform;
                }else{
                    var spawnedTile = Instantiate(Tile, new Vector3((float)x/10,(float)y/10), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";
                    Variables.Object(spawnedTile).Set("x", x);
                    Variables.Object(spawnedTile).Set("y", y);
                    spawnedTile.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
                    spawnedTile.transform.parent = TileParent.transform;
                }
            }
        }
        _cam.transform.position = new Vector3((float)_width/20 -0.05f, (float)_height/20 -0.05f, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reset(){
        GenerateGrid();
    }
}
