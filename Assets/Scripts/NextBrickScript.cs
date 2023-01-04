using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBrickScript : MonoBehaviour
{
    
    int [,] layout = new int[4,4];

    // Start is called before the first frame update
    void Start()
    {
        layout[1,0] = 1;
        layout[1,1] = 1;
        layout[1,2] = 1;
        layout[1,3] = 1;
        for(int x = 0;x < 4;x++) {
            for(int y = 0;y < 4;y++) {
                layout[x,y] = 0;
            }
        }
        NextBricks = new BrickRenderer[5];
         for(int i = 0; i < 5;i++) {
            NextBricks[i] = gameObject.AddComponent<BrickRenderer>();
            NextBricks[i].Populate(layout,1.0f);
            NextBricks[i].vPosition = new Vector3(0.5f, (float)i * 0.2f, 0.0f);
         }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    BrickRenderer [] NextBricks;
}
