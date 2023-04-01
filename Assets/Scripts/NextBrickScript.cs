using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBrickScript : MonoBehaviour
{

    public GameObject myControlBrick;
    public Tower myTower;

    int[,] layout = new int[4, 4];

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                layout[x, y] = 0;
            }
        }
        /*        NextBricks = new BrickRenderer[5];
                for (int i = 0; i < 5; i++)
                {
                    NextBricks[i] = gameObject.AddComponent<BrickRenderer>();
                    NextBricks[i].vPosition = new Vector3(12f, (float)i * 0.2f, 0.0f);
                }*/

        brickRenderer = gameObject.AddComponent<BrickRenderer>();
        brickRenderer.vPosition = new Vector3(12f, 0.05f, 0.0f);
    }

    // Should be invoked every time a new control piece is generated
    void Rebuild()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                layout[x, y] = 0;
            }
        }


        string sActiveColor = "red";
        int NextPieceId = myControlBrick.GetComponent<ControlBrickScript>().NextNext;
        if (NextPieceId == 1)
        { // I piece
            layout[1, 0] = 1;
            layout[1, 1] = 1;
            layout[1, 2] = 1;
            layout[1, 3] = 1;
//            activeMaterial = tealMaterial;
            sActiveColor = "teal";
        }
        else if (NextPieceId == 2)
        { // BL piece
            layout[1, 0] = 1;
            layout[1, 1] = 1;
            layout[1, 2] = 1;
            layout[0, 2] = 1;
 //           activeMaterial = blueMaterial;
            sActiveColor = "blue";
        }
        else if (NextPieceId == 3)
        { // orange L
            layout[1, 3] = 1;
            layout[1, 2] = 1;
            layout[1, 1] = 1;
            layout[0, 1] = 1;
 //           activeMaterial = orangeMaterial;
            sActiveColor = "orange";
        }
        else if (NextPieceId == 4)
        { // square
            layout[1, 1] = 1;
            layout[2, 1] = 1;
            layout[2, 2] = 1;
            layout[1, 2] = 1;
    //        activeMaterial = yellowMaterial;
            sActiveColor = "yellow";
        }
        else if (NextPieceId == 5)
        { // s
            layout[0, 1] = 1;
            layout[1, 1] = 1;
            layout[1, 0] = 1;
            layout[2, 0] = 1;
       //     activeMaterial = greenMaterial;
            sActiveColor = "green";
        }
        else if (NextPieceId == 6)
        {
            layout[1, 1] = 1;
            layout[2, 0] = 1;
            layout[2, 1] = 1;
            layout[3, 1] = 1;
       //     activeMaterial = purpleMaterial;
            sActiveColor = "purple";
        }
        else if (NextPieceId == 7)
        {
            layout[0, 0] = 1;
            layout[1, 0] = 1;
            layout[1, 1] = 1;
            layout[2, 1] = 1;
         //   activeMaterial = redMaterial;
         sActiveColor = "red";
        }
        brickRenderer.Populate(layout, 1.0f);
        brickRenderer.SetMaterial(myTower.GetComponent<Tower>().GetMaterial(sActiveColor));
    }

    // Update is called once per frame
    void Update()
    {
        Rebuild();
    }

    private BrickRenderer brickRenderer;
    BrickRenderer[] NextBricks;
}
