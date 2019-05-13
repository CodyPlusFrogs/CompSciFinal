using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public List<GameObject> allBuildings = new List<GameObject>();
    public Texture2D levelTexture;
    // Start is called before the first frame update
    void Start()
    {
        /*
        for(int y = 0; y < levelTexture.height; y++)
        {
            for(int x = 0; x < levelTexture.width; x++)
            {
                float spacing = 15;
                if(levelTexture.GetPixel(x,y).r > 0.01)
                {
                    Instantiate(allBuildings[0], new Vector3(x * spacing, 0, y * spacing), Quaternion.identity);
                }
                if(levelTexture.GetPixel(x,y).g > 0.01)
                {
                    Instantiate(allBuildings[1], new Vector3(x  * spacing, 0, y * spacing), Quaternion.identity);
                }
                if (levelTexture.GetPixel(x, y).b > 0.01)
                {
                    Instantiate(allBuildings[2], new Vector3(x * spacing, 0, y * spacing), Quaternion.identity);
                }
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
