using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameObject myControlBlock;
    public Tower myTower;
    public GameStateController myGameStateController;

    // Start is called before the first frame update
    void Start()
    {
        myBrickRenderer = gameObject.AddComponent<BrickRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        myBrickRenderer.Populate(myControlBlock.GetComponent<ControlBrickScript>().GetCurrentBrickArr(),1.0f);
        UpdatePosition();
    }

    void UpdatePosition()
    {
        ControlBrickScript myCtrBrick = myControlBlock.GetComponent<ControlBrickScript>();
        int iTargetX = myCtrBrick.GetXIndex();
        int iTargetY = myCtrBrick.GetYIndex();

        for(int i = 0;i < 100;i++) {
            iTargetY++;
            for(int x = 0;x < 4;x++) {
                for(int y = 0;y < 4;y++) {
                    if(myCtrBrick.GetCurrentBrickArr()[x,y] != 0) {
                        if(myTower.GetBrick(iTargetX + x,iTargetY + y) != 0) {
                            iTargetY--;
                            myBrickRenderer.vPosition = new Vector3((iTargetX) - 0.55f,(iTargetY),0);
                            return;
                        }
                    }
                }
            }
        }
    }

    private BrickRenderer myBrickRenderer;
}
