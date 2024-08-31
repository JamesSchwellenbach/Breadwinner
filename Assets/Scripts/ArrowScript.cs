using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class ArrowScript : MonoBehaviour
{
    public Camera MainCamera;
    public string dir = "Up";
    private float moveSpeed = 5f; //switch to 1f for gameplay
    public Transform movePoint;
    private Vector2 screenBounds;
    private int i = 1;
    public bool safe = true;
    private int lineNum = 0;
    private GameObject[,] TileArray = new GameObject[160, 90];
    public GameObject LineParent;
    int germCount;
    public void setGermCount(int count){
        germCount = count;
    }
    
    //win/loss condition
    int coveredTiles;
    int lives = 3;

    Vector3 safePosition;
    Quaternion safeRotation;
    

    public GameObject Line;

    GameObject currLine;
    LineRenderer lr;
    EdgeCollider2D edgeCollider2D;
    List<Vector2> colliderPoints;
    Vector2 currLineOffset;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        movePoint.position = new Vector3(5.85f, 0.15f, - 0.1f);
        transform.position = new Vector3(5.85f, 0.15f, - 0.1f);
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        safePosition = transform.position;
        safeRotation = transform.rotation;
        

        coveredTiles = 0;

        for (int x = 0; x < 160; x++){
            for (int y = 0; y < 90; y++){
                TileArray[x,y] = GameObject.Find($"Tile {x} {y}");
                //TileArray[x,y] = TileMap.GetTile(Vector3(x,y,0)).gameObject();
            }
        }
    }
    void Reset(){
        movePoint.position = new Vector3(5.85f, 0.15f, - 0.1f);
        transform.position = new Vector3(5.85f, 0.15f, - 0.1f);
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        safePosition = transform.position;
        safeRotation = transform.rotation;

        coveredTiles = 0;
    }
    void createLine(){
        currLine = Instantiate(Line, transform.position, Quaternion.identity);
        currLine.name = $"Line {lineNum}";
        currLineOffset = new Vector2(transform.position.x, transform.position.y);
        lineNum += 1;
        lr = currLine.GetComponent<LineRenderer>();
        edgeCollider2D = currLine.GetComponent<EdgeCollider2D>();
        lr.enabled = true;
        lr.transform.parent = LineParent.transform;

        lr.SetPosition(0, new Vector3(Mathf.Round((transform.position.x + 0.05f) * 10) / 10 - 0.05f, Mathf.Round((transform.position.y + 0.05f) * 10) / 10 - 0.05f, transform.position.z));
        lr.SetPosition(1, movePoint.position);
        colliderPoints = new List<Vector2>();
        colliderPoints.Add(new Vector2(transform.position.x - currLineOffset.x,transform.position.y - currLineOffset.y));
        colliderPoints.Add(new Vector2(movePoint.transform.position.x - currLineOffset.x,movePoint.position.y - currLineOffset.y));
        edgeCollider2D.points = colliderPoints.ToArray();
        i = 1;

        lr.material.SetColor("_Color", Color.red);
    }
    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -0.05f, screenBounds.x);
        viewPos.y = Mathf.Clamp(viewPos.y, -0.05f, screenBounds.y);
        transform.position = viewPos;
        Vector3 movePos = movePoint.position;
        movePos.x = Mathf.Clamp(movePos.x, -0.05f, screenBounds.x);
        movePos.y = Mathf.Clamp(movePos.y, -0.05f, screenBounds.y);
        movePoint.position = movePos;
    }

    // Update is called once per frame
    void Update()
    {
        movePoint.position = new Vector3(Mathf.Round((movePoint.position.x - 0.05f) * 10)/10 + 0.05f, Mathf.Round((movePoint.position.y - 0.05f) * 10)/10 + 0.05f, movePoint.position.z);
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position,movePoint.position) <= 0.05f){
            switch(dir)
            {
            case "Up":
                if (Input.GetKey(KeyCode.UpArrow)){
                    goUp();
                }
                else if (Input.GetKey(KeyCode.LeftArrow)){
                    dir = "Left";
                    goLeft();
                }else if (Input.GetKey(KeyCode.RightArrow)){
                    dir = "Right";
                    goRight();
                }else if (Input.GetKey(KeyCode.DownArrow)){
                    if(safe){
                        dir = "Down";
                        goDown();
                    }
                }
                break;
            case "Down":
                if (Input.GetKey(KeyCode.DownArrow)){
                    goDown();
                }
                else if (Input.GetKey(KeyCode.LeftArrow)){
                    dir = "Left";
                    goLeft();
                }
                else if (Input.GetKey(KeyCode.RightArrow)){
                    dir = "Right";
                    goRight();
                }else if (Input.GetKey(KeyCode.UpArrow)){
                    if(safe){
                        dir = "Up";
                        goUp();
                    }
                }
                break;
            case "Left":
                if (Input.GetKey(KeyCode.LeftArrow)){
                    goLeft();
                }
                else if (Input.GetKey(KeyCode.UpArrow)){
                    dir = "Up";
                    goUp();
                }
                else if (Input.GetKey(KeyCode.DownArrow)){
                    dir = "Down";
                    goDown();
                }else if (Input.GetKey(KeyCode.RightArrow)){
                    if(safe){
                        dir = "Right";
                        goRight();
                    }
                }
                break;
            case "Right":
                if (Input.GetKey(KeyCode.RightArrow)){
                    goRight();
                }
                else if (Input.GetKey(KeyCode.UpArrow)){
                    dir = "Up";
                    goUp();
                }
                else if (Input.GetKey(KeyCode.DownArrow)){
                    dir = "Down";
                    goDown();
                }else if (Input.GetKey(KeyCode.LeftArrow)){
                    if(safe){
                        dir = "Left";
                        goLeft();
                    }
                }
                break;

            }
        }
    }
    void goUp(){
        movePoint.position += new Vector3(0,0.1f,0);
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        if(!safe){
            staySameDirection();
            safe = isSafe(movePoint.position.x, movePoint.position.y);
            if(safe){
                finishLine();
            }
        }else{ // safe
            safe = isSafe(movePoint.position.x, movePoint.position.y);
            safeRotation = transform.rotation;
            if(!safe){
                createLine();
            }
        }
    }
    void goDown(){
        movePoint.position += new Vector3(0,-0.1f,0);
        transform.rotation = Quaternion.Euler(0f,0f,180f);
        if(!safe){
            staySameDirection();
            safe = isSafe(movePoint.position.x, movePoint.position.y);
            if(safe){
                finishLine();
            }
        }else{ // safe
            safe = isSafe(movePoint.position.x, movePoint.position.y);
            safeRotation = transform.rotation;
            if(!safe){
                createLine();
            }
        }
    }
    void goLeft(){
        movePoint.position += new Vector3(-0.1f,0,0);
        transform.rotation = Quaternion.Euler(0f,0f,90f);
        if(!safe){
            staySameDirection();
            safe = isSafe(movePoint.position.x, movePoint.position.y);
            if(safe){
                finishLine();
            }
        }else{ // safe
            safe = isSafe(movePoint.position.x, movePoint.position.y);
            safeRotation = transform.rotation;
            if(!safe){
                createLine();
            }
        }
    }
    void goRight(){
        movePoint.position += new Vector3(0.1f,0,0);
        transform.rotation = Quaternion.Euler(0f,0f,270f);
        if(!safe){
            staySameDirection();
            safe = isSafe(movePoint.position.x, movePoint.position.y);
            if(safe){
                finishLine();
            }
        }else{ // safe
            safe = isSafe(movePoint.position.x, movePoint.position.y);
            safeRotation = transform.rotation;
            if(!safe){
                createLine();
            }
        }
    }
    void staySameDirection(){
        Vector2 newPoint = new Vector2((float)Mathf.Round((movePoint.position.x - currLineOffset.x - 0.05f) * 10)/10 + 0.05f,(float)Math.Round((movePoint.position.y - currLineOffset.y - 0.05f) * 10)/10 + 0.05f);
        if(i > 2 && colliderPoints.GetRange(0,colliderPoints.Count-2).Contains(newPoint)){
            Debug.Log(colliderPoints);
            Debug.Log(newPoint);
            germCollision();
            return;
        }
        i += 1;
        lr.positionCount += 1;
        lr.SetPosition(i, movePoint.position);
        colliderPoints.Add(newPoint);
        edgeCollider2D.points = colliderPoints.ToArray();
    }
    bool isSafe(float x, float y){
        x *= 10;
        y *= 10;
        if(x < 0 || x > 160 || y < 0 || y > 90){
            safePosition = movePoint.transform.position;
            safeRotation = transform.rotation;
            return true;
        }
        
        bool ret = TileArray[(int)Math.Round(x-0.5f),(int)Math.Round(y-0.5f)].GetComponent<SpriteRenderer>().color == TileArray[0,0].GetComponent<SpriteRenderer>().color;
        ret = ret || TileArray[(int)Math.Round(x-0.5f),(int)Math.Round(y+0.5f)].GetComponent<SpriteRenderer>().color == TileArray[0,0].GetComponent<SpriteRenderer>().color;
        ret = ret || TileArray[(int)Math.Round(x+0.5f),(int)Math.Round(y-0.5f)].GetComponent<SpriteRenderer>().color == TileArray[0,0].GetComponent<SpriteRenderer>().color;
        ret = ret || TileArray[(int)Math.Round(x+0.5f),(int)Math.Round(y+0.5f)].GetComponent<SpriteRenderer>().color == TileArray[0,0].GetComponent<SpriteRenderer>().color;
        ret = ret || !(bool)Variables.Object(TileArray[(int)Math.Round(x-0.5f), (int)Math.Round(y-0.5f)]).Get("checkRight") || !(bool)Variables.Object(TileArray[(int)Math.Round(x-0.5f), (int)Math.Round(y-0.5f)]).Get("checkUp"); 
        ret = ret || !(bool)Variables.Object(TileArray[(int)Math.Round(x+0.5f), (int)Math.Round(y+0.5f)]).Get("checkLeft") || !(bool)Variables.Object(TileArray[(int)Math.Round(x+0.5f), (int)Math.Round(y+0.5f)]).Get("checkDown");
        if(ret){
            safePosition = movePoint.transform.position;
            safeRotation = transform.rotation;
        }
        return ret;
        }

    void finishLine(){
        lr.material.SetColor("_Color", Color.black);
        currLine.GetComponent<EdgeCollider2D>().isTrigger = false;
        
        floodFill();

        currLine = null;
        lr = null;
        edgeCollider2D = null;
    }
    public void germCollision(){
        transform.position = safePosition;
        transform.rotation = safeRotation;
        movePoint.transform.position = safePosition;


        Destroy(currLine);
        currLine = null;
        lr = null;
        edgeCollider2D = null;
        safe = true;

        lives -= 1;
        if(lives == 0){
            GameObject.Find($"GermSpawner").GetComponent<GermSpawner>().Reset(1);
            for (int x = 2; x < 126; x++){
                for (int y = 2; y < 88; y++){
                    TileArray[x,y].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
                    Variables.Object(TileArray[x, y]).Set("checkLeft", true);
                    Variables.Object(TileArray[x, y]).Set("checkRight", true);
                    Variables.Object(TileArray[x, y]).Set("checkUp", true);
                    Variables.Object(TileArray[x, y]).Set("checkDown", true);
                }
            }
            foreach (Transform child in GameObject.Find($"Lines").transform){
                if(child.name != "Outer Walls"){
                    Destroy(child.gameObject);
                }
            }
            Reset();
            Debug.Log("You Lose");
            lives = 3;
        }
    }

    void floodFill(){
        // check both sides of line, stopping if side contains a germ, otherwise, append entire list to fill list
        List<Tuple<int,int>> leftSide = new List<Tuple<int,int>>();
        List<Tuple<int,int>> rightSide = new List<Tuple<int,int>>();
        Queue<Tuple<int,int>> leftFill = new Queue<Tuple<int,int>>();
        Queue<Tuple<int,int>> rightFill = new Queue<Tuple<int,int>>();
        Vector3[] lrPositions = new Vector3[lr.positionCount];
        Tuple<int, int> Left;
        Tuple<int, int> Right;
        GameObject[] Germs = new GameObject[germCount];
        bool fill = true;

        for (int i = 0; i < germCount; i++){
            Germs[i] = GameObject.Find($"Germ {i}");
        }

        bool[,] checkedTiles = new bool[160, 90];

        lr.GetPositions(lrPositions);
        for(int i = 0; i < lr.positionCount-1; i++){
            
            if(Math.Round(lrPositions[i].x * 10 - 0.5) == Math.Round(lrPositions[i+1].x * 10 - 0.5)){
                int x = (int)Math.Round(lrPositions[i].x*10 - 0.5);
                int y = (int)Math.Round((lrPositions[i].y + lrPositions[i+1].y)/0.2f);
                if(lrPositions[i].y < lrPositions[i+1].y){   //going up: left side lesser x
                    Left = new Tuple<int, int>(x,y);
                    leftSide.Add(Left);
                    leftFill.Enqueue(Left);

                    Right = new Tuple<int, int>(x+1,y);
                    rightSide.Add(Right);
                    rightFill.Enqueue(Right);

                    Variables.Object(TileArray[x, y]).Set("checkRight", false);
                    Variables.Object(TileArray[x+1, y]).Set("checkLeft", false);
                    
                    checkedTiles[x,y] = true;
                    checkedTiles[x+1,y] = true;
                }else{  //going down: right side lesser x
                    Right = new Tuple<int, int>(x,y);
                    rightSide.Add(Right);
                    rightFill.Enqueue(Right);
                    Left = new Tuple<int, int>(x+1,y);
                    leftSide.Add(Left);
                    leftFill.Enqueue(Left);

                    Variables.Object(TileArray[x, y]).Set("checkRight", false);
                    Variables.Object(TileArray[x+1, y]).Set("checkLeft", false);
                    
                    checkedTiles[x,y] = true;
                    checkedTiles[x+1,y] = true;
                }
            }else{
                int x = (int)Math.Round((lrPositions[i].x + lrPositions[i+1].x)/0.2f);
                int y = (int)Math.Round(lrPositions[i].y * 10 - 0.5);
                if(lrPositions[i].x < lrPositions[i+1].x){   //going right: right side lesser y
                    
                    Right = new Tuple<int, int>(x,y);
                    rightSide.Add(Right);
                    rightFill.Enqueue(Right);
                    Left = new Tuple<int, int>(x,y+1);
                    leftSide.Add(Left);
                    leftFill.Enqueue(Left);

                    Variables.Object(TileArray[x, y]).Set("checkUp", false);
                    Variables.Object(TileArray[x, y+1]).Set("checkDown", false);
                    
                    checkedTiles[x,y] = true;
                    checkedTiles[x,y+1] = true;
                }else{  //going left: left side lesser y
                    Left = new Tuple<int, int>(x,y);
                    leftSide.Add(Left);
                    leftFill.Enqueue(Left);
                    Right = new Tuple<int, int>(x,y+1);
                    rightSide.Add(Right);
                    rightFill.Enqueue(Right);

                    Variables.Object(TileArray[x, y]).Set("checkUp", false);
                    Variables.Object(TileArray[x, y+1]).Set("checkDown", false);

                    checkedTiles[x,y] = true;
                    checkedTiles[x,y+1] = true;
                }
            }
        }
        while(leftFill.Count > 0 && fill){
            Tuple<int,int> curr = leftFill.Dequeue();
            int x = curr.Item1;
            int y = curr.Item2;
            if(!checkedTiles[x+1,y] && TileArray[x+1, y].GetComponent<SpriteRenderer>().color != TileArray[0,0].GetComponent<SpriteRenderer>().color && (bool)Variables.Object(TileArray[x, y]).Get("checkRight")){
                foreach(GameObject germ in Germs){
                    if (Vector3.Distance(germ.transform.position, TileArray[x+1,y].transform.position) < 0.1f) {
                        leftSide.Clear();
                        leftFill.Clear();
                        fill = false;
                        break;
                    }
                }
                checkedTiles[x+1,y] = true;
                Left = new Tuple<int, int>(x+1,y);
                leftSide.Add(Left);
                leftFill.Enqueue(Left);
            }
            if(!checkedTiles[x-1,y] && TileArray[x-1, y].GetComponent<SpriteRenderer>().color != TileArray[0,0].GetComponent<SpriteRenderer>().color && (bool)Variables.Object(TileArray[x, y]).Get("checkLeft")){
                foreach(GameObject germ in Germs){
                    if (Vector3.Distance(germ.transform.position, TileArray[x-1,y].transform.position) < 0.1f) {
                        leftSide.Clear();
                        leftFill.Clear();
                        fill = false;
                        break;
                    }
                }
                checkedTiles[x-1,y] = true;
                Left = new Tuple<int, int>(x-1,y);
                leftSide.Add(Left);
                leftFill.Enqueue(Left);
            }
            if(!checkedTiles[x,y+1] && TileArray[x, y+1].GetComponent<SpriteRenderer>().color != TileArray[0,0].GetComponent<SpriteRenderer>().color && (bool)Variables.Object(TileArray[x, y]).Get("checkUp")){
                foreach(GameObject germ in Germs){
                    if (Vector3.Distance(germ.transform.position, TileArray[x,y+1].transform.position) < 0.1f) {
                        leftSide.Clear();
                        leftFill.Clear();
                        fill = false;
                        break;
                    }
                }
                checkedTiles[x,y+1] = true;
                Left = new Tuple<int, int>(x,y+1);
                leftSide.Add(Left);
                leftFill.Enqueue(Left);
            }
            if(!checkedTiles[x,y-1] && TileArray[x, y-1].GetComponent<SpriteRenderer>().color != TileArray[0,0].GetComponent<SpriteRenderer>().color && (bool)Variables.Object(TileArray[x, y]).Get("checkDown")){
                foreach(GameObject germ in Germs){
                    if (Vector3.Distance(germ.transform.position, TileArray[x,y-1].transform.position) < 0.1f) {
                        leftSide.Clear();
                        leftFill.Clear();
                        fill = false;
                        break;
                    }
                }
                checkedTiles[x,y-1] = true;
                Left = new Tuple<int, int>(x,y-1);
                leftSide.Add(Left);
                leftFill.Enqueue(Left);
            }
        }
        if(fill){
            foreach(Tuple<int,int> var in leftSide){
                TileArray[var.Item1, var.Item2].GetComponent<SpriteRenderer>().color = TileArray[0,0].GetComponent<SpriteRenderer>().color;
                coveredTiles += 1;
            }
        }
        fill = true;
        while(rightFill.Count > 0 && fill){
            Tuple<int,int> curr = rightFill.Dequeue();
            int x = curr.Item1;
            int y = curr.Item2;
            if(!checkedTiles[x+1,y] && TileArray[x+1, y].GetComponent<SpriteRenderer>().color != TileArray[0,0].GetComponent<SpriteRenderer>().color && (bool)Variables.Object(TileArray[x, y]).Get("checkRight")){
                foreach(GameObject germ in Germs){
                    if (Vector3.Distance(germ.transform.position, TileArray[x+1,y].transform.position) < 0.1f) {
                        rightSide.Clear();
                        rightFill.Clear();
                        fill = false;
                        break;
                    }
                }
                checkedTiles[x+1,y] = true;
                Right = new Tuple<int, int>(x+1,y);
                rightSide.Add(Right);
                rightFill.Enqueue(Right);
            }
            if(!checkedTiles[x-1,y] && TileArray[x-1, y].GetComponent<SpriteRenderer>().color != TileArray[0,0].GetComponent<SpriteRenderer>().color && (bool)Variables.Object(TileArray[x, y]).Get("checkLeft")){
                foreach(GameObject germ in Germs){
                    if (Vector3.Distance(germ.transform.position, TileArray[x-1,y].transform.position) < 0.1f) {
                        rightSide.Clear();
                        rightFill.Clear();
                        fill = false;
                        break;
                    }
                }
                checkedTiles[x-1,y] = true;
                Right = new Tuple<int, int>(x-1,y);
                rightSide.Add(Right);
                rightFill.Enqueue(Right);
            }
            if(!checkedTiles[x,y+1] && TileArray[x, y+1].GetComponent<SpriteRenderer>().color != TileArray[0,0].GetComponent<SpriteRenderer>().color && (bool)Variables.Object(TileArray[x, y]).Get("checkUp")){
                foreach(GameObject germ in Germs){
                    if (Vector3.Distance(germ.transform.position, TileArray[x,y+1].transform.position) < 0.1f) {
                        rightSide.Clear();
                        rightFill.Clear();
                        fill = false;
                        break;
                    }
                }
                checkedTiles[x,y+1] = true;
                Right = new Tuple<int, int>(x,y+1);
                rightSide.Add(Right);
                rightFill.Enqueue(Right);
            }
            if(!checkedTiles[x,y-1] && TileArray[x, y-1].GetComponent<SpriteRenderer>().color != TileArray[0,0].GetComponent<SpriteRenderer>().color && (bool)Variables.Object(TileArray[x, y]).Get("checkDown")){
                foreach(GameObject germ in Germs){
                    if (Vector3.Distance(germ.transform.position, TileArray[x,y-1].transform.position) < 0.1f) {
                        rightSide.Clear();
                        rightFill.Clear();
                        fill = false;
                        break;
                    }
                }
                checkedTiles[x,y-1] = true;
                Right = new Tuple<int, int>(x,y-1);
                rightSide.Add(Right);
                rightFill.Enqueue(Right);
            }
        }
        if(fill){
            foreach(Tuple<int,int> var in rightSide){
                TileArray[var.Item1, var.Item2].GetComponent<SpriteRenderer>().color = TileArray[0,0].GetComponent<SpriteRenderer>().color;
                coveredTiles += 1;
            }
        }
        if(coveredTiles > 8531){
            GameObject.Find($"GermSpawner").GetComponent<GermSpawner>().Reset(Math.Min(germCount+1, 10));
            for (int x = 2; x < 126; x++){
                for (int y = 2; y < 88; y++){
                    TileArray[x,y].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
                    Variables.Object(TileArray[x, y]).Set("checkLeft", true);
                    Variables.Object(TileArray[x, y]).Set("checkRight", true);
                    Variables.Object(TileArray[x, y]).Set("checkUp", true);
                    Variables.Object(TileArray[x, y]).Set("checkDown", true);
                }
            }
            foreach (Transform child in GameObject.Find($"Lines").transform){
                if(child.name != "Outer Walls"){
                    Destroy(child.gameObject);
                }
            }
            Reset();
            lives += 1;
            Debug.Log("You Win!");
        }
    }
}
