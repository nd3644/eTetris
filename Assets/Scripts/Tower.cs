using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public Material blueMaterial;
    public Material tealMaterial;
    public Material greenMaterial;

    public Material purpleMaterial;
    public Material orangeMaterial;
    public Material yellowMaterial;
    public Material redMaterial;
    public GameObject myParticlePrefab;

    public int[,] iGrid;
    public string[,] sColors;
    public GameObject[,] myObjects;
    int iGridWidth = 10;
    int iGridHeight = 20;

    public void SetBrick(int x, int y, int i)
    {
        iGrid[9-x,19-y] = i;
    }
    public void SetColor(int x, int y, string s)
    {
        sColors[9-x,19-y] = s;

        // Manually update the material because it isn't updated every frame, only when
        // the active state changes
        myObjects[9-x,19-y].GetComponent<Renderer>().material = GetMaterial(s);
    }

    public string GetColor(int x, int y)
    {
        return sColors[9-x,19-y];
    }

    public int GetBrick(int x, int y)
    {
        if(x < 0 || x > 9 || y < 0 || y > 19)
        {
            return 1;
        }
        return iGrid[9-x,19-y];
    }
    // Start is called before the first frame update
    void Start()
    {
        iGrid = new int[10,20];
        sColors = new string[10,20];
        myObjects = new GameObject[10,20];
        for(int x = 0;x < iGridWidth;x++)
        {
            for(int y = 0;y < iGridHeight;y++)
            {
                iGrid[x,y] = 0;//Random.Range(0,2);
                myObjects[x,y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                myObjects[x,y].transform.parent = transform;
                myObjects[x,y].transform.position = new Vector3(x + 0.6f,y + 0.65f,0);
            }
        }
    }

    Material GetMaterial(string s)
    {
        if(s == "blue")
        {
            return blueMaterial;
        }
        else if(s == "teal")
        {
            return tealMaterial;
        }
        else if(s == "green")
        {
            return greenMaterial;
        }
        else if(s == "purple")
        {
            return purpleMaterial;
        }
        else if(s == "orange")
        {
            return orangeMaterial;
        }
        else if(s == "yellow")
        {
            return yellowMaterial;
        }
        else if(s == "red")
        {
            return redMaterial;
        }
        return redMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        for(int x = 0;x < iGridWidth;x++)
        {
            for(int y = 0;y < iGridHeight;y++)
            {
                if(iGrid[x,y] == 0)
                {
                    if(myObjects[x,y].activeSelf == true)
                    {
                        myObjects[x,y].SetActive(false);
                    }
                }
                else {
                    if(myObjects[x,y].activeSelf == false)
                    {
                        myObjects[x,y].SetActive(true);
                        myObjects[x,y].GetComponent<Renderer>().material = GetMaterial(sColors[x,y]);
                    }
                }
            }
        }
        TryClearSolidLines();

        vTowerOffset = Vector2.Lerp(vTowerOffset, new Vector2(0,0), Time.deltaTime * 10);
        transform.position = vTowerOffset;
    }

    void TryClearSolidLines()
    {
        int iLineStart = -1;
        int iLineEnd = -1;

        for(int y = 0;y < 20;y++) {
            if(IsLineSolid(y)) {
                iLineStart = y;
                break;
            }
        }
        for(int y = 19;y > 1;y--) {
            if(IsLineSolid(y)) {
                iLineEnd = y;
                break;
            }
        }

        int lineHeight = (iLineEnd - iLineStart) + 1;

        if(iLineStart == -1 || iLineEnd == -1)
            return;

        // Remove the bricks
        for(int y = iLineStart;y <= iLineEnd;y++) {
            for(int x = 0;x < 10;x++) {
//                iGrid[x,19-y] = 0;
                CreateParticles(x,y);
                SetBrick(x,y,0);
            }
        }

        for(int y = iLineEnd;y > 1;y--) {
            for(int x = 0;x < 10;x++) {
                SetColor(x,y,GetColor(x,y-lineHeight));
                SetBrick(x,y,GetBrick(x,y-lineHeight));
            }
        }


//        Debug.Log("Start: " + iLineStart);
//        Debug.Log("End: " + iLineEnd);

//        vTowerOffset.y += lineHeight;
//        Debug.Log(vTowerOffset.y);

    }

    /*#
    // Generate particles with old colors

        // Shift all bricks down accordingly
        for(int y = 19;y > iLineStart;y--) {
            for(int x = 0;x < 10;x++) {
                int brick = GetBrick(x,y-1);
                string color = GetColor(x,y-1);

                SetColor(x,y,color);
                SetBrick(x,y,brick);
            }
        }
    */

    bool IsLineSolid(int y)
    {
        for(int x = 0;x < 10;x++) {
            if(GetBrick(x,y) == 0)
                return false;
        }
        return true;
    }

    void CreateParticles(int x, int y) {
        GameObject part = Instantiate(myParticlePrefab, new Vector3(10-x,20-y,1), Quaternion.identity);
        ParticleSystem sys = part.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ma = sys.main;

        ma.startColor = GetMaterial(GetColor(x,y)).color;
    }

    public Vector2 vTowerOffset;
}
