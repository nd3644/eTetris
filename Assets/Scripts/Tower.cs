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
        iGrid[9 - x, 19 - y] = i;
    }
    public void SetColor(int x, int y, string s)
    {
        sColors[9 - x, 19 - y] = s;

        // Manually update the material because it isn't updated every frame, only when
        // the active state changes
        myObjects[9 - x, 19 - y].GetComponent<Renderer>().material = GetMaterial(s);
    }

    public string GetColor(int x, int y)
    {
//        Debug.Log("x: " + x);
        return sColors[9 - x, 19 - y];
    }

    public int GetBrick(int x, int y)
    {
        if (x < 0 || x > 9 || y < 0 || y > 19)
        {
            return 1;
        }
        return iGrid[9 - x, 19 - y];
    }
    // Start is called before the first frame update
    void Start()
    {
        iGrid = new int[10, 20];
        sColors = new string[10, 20];
        myObjects = new GameObject[10, 20];
        for (int x = 0; x < iGridWidth; x++)
        {
            for (int y = 0; y < iGridHeight; y++)
            {
                iGrid[x, y] = 0;//Random.Range(0,2);
                myObjects[x, y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                myObjects[x, y].transform.parent = transform;
                myObjects[x, y].transform.position = new Vector3(x + 0.6f, y + 0.65f, 0);
            }
        }
    }

    public Material GetMaterial(string s)
    {
        switch (s)
        {
            case "blue":
                return blueMaterial;
            case "teal":
                return tealMaterial;
            case "green":
                return greenMaterial;
            case "purple":
                return purpleMaterial;
            case "orange":
                return orangeMaterial;
            case "yellow":
                return yellowMaterial;
            case "red":
                return redMaterial;
            default:
                return redMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < iGridWidth; x++)
        {
            for (int y = 0; y < iGridHeight; y++)
            {
                if (iGrid[x, y] == 0)
                {
                    if (myObjects[x, y].activeSelf == true)
                    {
                        myObjects[x, y].SetActive(false);
                    }
                }
                else
                {
                    if (myObjects[x, y].activeSelf == false)
                    {
                        myObjects[x, y].SetActive(true);
                        myObjects[x, y].GetComponent<Renderer>().material = GetMaterial(sColors[x, y]);
                    }
                }
            }
        }

        int loop_guard = 0;
        while (IsAnyLinesSolid())
        {
            ClearSolidLineAndShiftBricks();
            if (loop_guard++ > 20)
            { // This should only happen in the case of a bug
                break;
            }
        }

        vTowerOffset = Vector2.Lerp(vTowerOffset, new Vector2(0, 0), Time.deltaTime * 10);
        transform.position = vTowerOffset;
    }

    bool IsAnyLinesSolid()
    {
        for (int y = 0; y < 20; y++)
        {
            if (IsLineSolid(y))
            {
                return true;
            }
        }
        return false;
    }

    // Function assumes there are solid lines present in the playfield
    void ClearSolidLineAndShiftBricks()
    {

        int target_line = 0;
        for (int y = 0; y < 20; y++)
        {
            if (IsLineSolid(y))
            {
                target_line = y;
            }
        }

        for (int x = 0; x < 10; x++)
        {
            CreateParticles(x, target_line);
            for(int y = target_line;y > 0;y--) {
                SetColor(x, y, GetColor(x, y - 1));
                SetBrick(x, y, GetBrick(x, y - 1));
            }
        }
    }

    bool IsLineSolid(int y)
    {
        for (int x = 0; x < 10; x++)
        {
            if (GetBrick(x, y) == 0)
                return false;
        }
        return true;
    }

    void CreateParticles(int x, int y)
    {
        if (x < 0 || x >= 10 || y <= 0 || y >= 20)
        {
            return;
        }
        GameObject part = Instantiate(myParticlePrefab, new Vector3(9 - x, 19 - y, 1), Quaternion.identity);
        ParticleSystem sys = part.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule ma = sys.main;

        ma.startColor = GetMaterial(GetColor(x, y)).color;
    }

    public void ClearTower()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                iGrid[x, y] = 0;
            }
        }
    }

    public Vector2 vTowerOffset;
}
