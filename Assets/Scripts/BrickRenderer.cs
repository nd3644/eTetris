using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BrickRenderer : MonoBehaviour
{

    public GameObject[,] myBricks;

    // Start is called before the first frame update
    void Start()
    {
        myBricks = new GameObject[4, 4];
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                myBricks[x, y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            }
        }
    }

    // calls SetActive() on each cube based on the contents of igrid
    public void Populate(int[,] igrid, float alpha)
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (igrid[x, y] == 1)
                    myBricks[x, y].SetActive(true);
                else
                    myBricks[x, y].SetActive(false);
 
                myBricks[x, y].GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f, 0.25f);
            }
        }
    }

    public void SetMaterial(Material mat) {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                myBricks[x, y].GetComponent<Renderer>().material = mat;
            }
        }
    }

    public Vector3 vPosition;
    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                myBricks[x, y].transform.position = new Vector3(10 - (vPosition.x + x) - 0.9f, 20 - (vPosition.y + y) - 0.35f, vPosition.z - 0.8f);
            }
        }
    }
}
