using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBrickScript : MonoBehaviour
{

    private Stack<PieceType>myPieceStack;
    private BrickRenderer [] myUpcomingBricks;

    private const float TIMER_MAX_TICKS = 1;
    private float currentTicks = TIMER_MAX_TICKS;

    private string sActiveColor;
    public Material fadedMaterial;
    public Material activeMaterial;
    public Material blueMaterial;

    public Material tealMaterial;

    public Material greenMaterial;

    public Material purpleMaterial;
    public Material orangeMaterial;
    public Material yellowMaterial;
    public Material redMaterial;


    public GameObject towerObject;

    public AudioSource rotateSound;

    public GameStateController myGameStateController;

    int iPosX = 3, iPosY = 0;
    int [,]iBrickLayout;
    GameObject [,] myBricks;
    // Start is called before the first frame update
    void Start()
    {
        myUpcomingBricks = new BrickRenderer[5];
        myPieceStack = new Stack<PieceType>();
        for(int i = 0;i < 5;i++) {
            myPieceStack.Push((PieceType)Random.Range(1,7));
        }
        iBrickLayout = new int[4,4];
        myBricks = new GameObject[4,4];

        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                iBrickLayout[x,y] = 0;
                myBricks[x,y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            }
        }

        RebuildShape();

//        Tower.GetComponent<ControlBrickScript>().Tower.GetBrick(0,0);
    }

    public int [,] GetCurrentBrickArr()
    {
        return iBrickLayout;
    }

    enum PieceType {
        PIECE_I = 1,
        PIECE_BL = 2,
        PIECE_OL = 3,
        PIECE_SQ = 4,
        PIECE_S = 5,
        PIECE_Z = 6,
        PIECE_T = 7,
    };

    void RebuildShape()
    {
        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                iBrickLayout[x,y] = 0;
            }
        }
        int piece = Random.Range(1,8);

        if(piece == 1) { // I piece
            iBrickLayout[1,0] = 1;
            iBrickLayout[1,1] = 1;
            iBrickLayout[1,2] = 1;
            iBrickLayout[1,3] = 1;
            activeMaterial = tealMaterial;
            sActiveColor = "teal";
        }
        else if(piece == 2) { // BL piece
            iBrickLayout[1,0] = 1;
            iBrickLayout[1,1] = 1;
            iBrickLayout[1,2] = 1;
            iBrickLayout[0,2] = 1;
            activeMaterial = blueMaterial;
            sActiveColor = "blue";
        }
        else if(piece == 3) { // orange L
            iBrickLayout[1,3] = 1;
            iBrickLayout[1,2] = 1;
            iBrickLayout[1,1] = 1;
            iBrickLayout[0,1] = 1;
            activeMaterial = orangeMaterial;
            sActiveColor = "orange";
        }
        else if(piece == 4) { // square
            iBrickLayout[1,1] = 1;
            iBrickLayout[2,1] = 1;
            iBrickLayout[2,2] = 1;
            iBrickLayout[1,2] = 1;
            activeMaterial = yellowMaterial;
            sActiveColor = "yellow";
        }
        else if(piece == 5) { // s
            iBrickLayout[0,1] = 1;
            iBrickLayout[1,1] = 1;
            iBrickLayout[1,0] = 1;
            iBrickLayout[2,0] = 1;
            activeMaterial = greenMaterial;
            sActiveColor = "green";
        }
        else if(piece == 6) {
            iBrickLayout[1,1] = 1;
            iBrickLayout[2,0] = 1;
            iBrickLayout[2,1] = 1;
            iBrickLayout[3,1] = 1;
            activeMaterial = purpleMaterial;
            sActiveColor = "purple";
        }
        else if(piece == 7) {
            iBrickLayout[0,0] = 1;
            iBrickLayout[1,0] = 1;
            iBrickLayout[1,1] = 1;
            iBrickLayout[2,1] = 1;
            activeMaterial = redMaterial;
            sActiveColor = "red";
        }


        // Below loads a brick for debug
        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                iBrickLayout[x,y] = 0;
            }
        }

        iBrickLayout[1,1] = 1;
        iBrickLayout[2,1] = 1;
        iBrickLayout[2,2] = 1;
        iBrickLayout[1,2] = 1;
        
    }

    void RegenerateCubes()
    {
        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                if(iBrickLayout[x,y] == 0) {
//                    myBricks[x,y].SetActive(false);
                }
                else {
                    myBricks[x,y].SetActive(true);
                }
                myBricks[x,y].transform.position = new Vector3(10 - (iPosX + x) - 0.55f,20 - (iPosY + y),0);
            }
        }
    }

    void Rotate()
    {
        FindObjectOfType<AudioManager>().Play("rotate");
        int[,] copy = (int[,])iBrickLayout.Clone();
        for(int x = 0;x < 4;x++)
        {
            for(int y = 0;y < 4;y++)
            {
                iBrickLayout[x,y] = copy[y,3-x];
            }
        }
    }

    void CheckForCollision()
    {
        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                if(iBrickLayout[x,y] != 0) {
                    if(iPosX + x > 9) {
                        iPosX--;
                    }
                    else if(iPosX + x < 0) {
                        iPosX++;
                    }
                }
            }
        }
    }

    bool DecrementPos()
    {
        iPosY++;
        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                if(iBrickLayout[x,y] != 0) {
                    if(towerObject.GetComponent<Tower>().GetBrick(iPosX + x, iPosY + y) != 0) {
                        iPosY--;
                        FinalizeBrick();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void FinalizeBrick()
    {
        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                if(iBrickLayout[x,y] != 0) {
                    towerObject.GetComponent<Tower>().SetBrick(iPosX + x, iPosY + y,1);
                    towerObject.GetComponent<Tower>().SetColor(iPosX + x, iPosY + y,sActiveColor);
                }
            }
        }
        iPosY = 0;
        RebuildShape();
        FindObjectOfType<AudioManager>().Play("drop");
    }

    // Update is called once per frame
    void Update()
    {
        if(myGameStateController.CurrentGameState != GameStateController.GameState.STATE_PLAYING)
            return;
        
        currentTicks -= Time.deltaTime;
        if(currentTicks < 0) {
//            DecrementPos();
            currentTicks = TIMER_MAX_TICKS;
        }
        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                if(iBrickLayout[x,y] == 1) {
                    myBricks[x,y].GetComponent<Renderer>().material = activeMaterial;
                    myBricks[x,y].SetActive(true);

                    myBricks[x,y].transform.Translate(new Vector3((x - 2) * 4, 0, (y - 2) * 4));
                }
                else {
                    myBricks[x,y].GetComponent<Renderer>().material = fadedMaterial;
                    myBricks[x,y].SetActive(false);
                }
            }
        }

        if(Input.GetKeyDown("left"))
        {
            iPosX--;
        }
        else if(Input.GetKeyDown("right"))
        {
            iPosX++;
        }
        CheckForCollision();

        if(Input.GetKeyDown("up"))
        {
            Rotate();
        }
        else if(Input.GetKeyDown("down"))
        {
            DecrementPos();
        }

        if(Input.GetKeyDown("space"))
        {
            for(int i = 0;i < 1000;i++) {
                if(DecrementPos()) {
                    break;
                }
            }
        }
        RegenerateCubes();
    }

    public int GetXIndex()
    {
        return iPosX;
    }
    public int GetYIndex()
    {
        return iPosY;
    }
}
