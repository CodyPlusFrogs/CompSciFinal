using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class StaticGeneration : EditorWindow
{

    //the step size of the instantiation
    public static float gridSize = 0;

    //all the possible buildings to instantiate
    private static GameObject[] allBuildings;

    //the wall to be instantiated
    private static GameObject wall;

    //the level image map
    private static Texture2D levelImageMap;

    //how far up the image we are
    private static int totalYProgress;

    /**
     * This method adds another pull down to the unity editor and will instantiate a level from a path
     */
    [MenuItem("Level Generation/Instantiate Along Path")]
    static void InstantiateAlongPath()
    {
        
        //get all the available buildings in the folder
        allBuildings = getBuildings();

        //load the image level map
        loadLevelMap();

        
        //now each cityBlock will have a disabled cube called Size telling us it's size
        Transform cityBlockSizeChild = allBuildings[0].GetComponent<Transform>().Find("Size");
        //the x scale of the block will represent the size of the city block
        gridSize = cityBlockSizeChild.localScale.x;


        //this is the parent that we will instantiate all of the level under
        GameObject instantiateParent = GameObject.FindGameObjectWithTag("Level Parent");
        //this object contains a bunch of cubes that will show us our path
        GameObject levelPath = Resources.Load("prefabs/LevelPath") as GameObject;

        //get the transform
        Transform levelPathTransform = levelPath.GetComponent<Transform>();


        //load the wall
        wall = Resources.Load("prefabs/Wall") as GameObject;


        //tracks where we are on the level map image and incremenets as we go up in height, instantiating buildings accordingly
        totalYProgress = 0;
        //first we are going to make sure the spacing on the path is equal throughout
        for(int i = 0; i < levelPathTransform.childCount; i++)
        {
            //the first block will be the start of the line
            Transform lineStartTransform = levelPathTransform.GetChild(i);

            //the second block will be the end of the line (look back to the 0th one on the end)
            Transform lineEndTransform = levelPathTransform.GetChild(i+1 < levelPathTransform.childCount ? i + 1 : (i+1)- levelPathTransform.childCount);

            Transform nextLineEndTransform  = levelPathTransform.GetChild(i + 2 < levelPathTransform.childCount ? i + 2 : (i + 2) - levelPathTransform.childCount);
            
            




            //we will only make into a spline half of the lines
            Vector3 midPoint1 = Vector3.Lerp(lineStartTransform.position, lineEndTransform.position, 0.5f);
            Vector3 midPoint2 = Vector3.Lerp(lineEndTransform.position, nextLineEndTransform.position, 0.5f);


            //the last position we instantiated from
            Vector3 lastInstantiate = new Vector3(-10000, 0, 0);
            for(float percent = 0; percent < 1; percent += 0.01f)
            {
                Vector3 firstLerp = Vector3.Lerp(midPoint1, lineEndTransform.position, percent);
                Vector3 secondLerp = Vector3.Lerp(lineEndTransform.position, midPoint2, percent);
                Vector3 splineLerp = Vector3.Lerp(firstLerp, secondLerp, percent);

                if(Vector3.Distance(splineLerp,lastInstantiate) > gridSize)
                {
                    //compute the heading toward the end transform
                    Vector3 heading = secondLerp - firstLerp;
                    
                    InstantiateRow(splineLerp, Quaternion.LookRotation(heading, 
                        Vector3.Lerp((lineStartTransform.up + lineEndTransform.up) / 2,
                        (lineEndTransform.up + nextLineEndTransform.up) / 2, percent)),
                        instantiateParent);

                    //move 1 up in the image we are instanting from
                    totalYProgress++;

                    lastInstantiate = splineLerp;
                }


            }
        }
    }

    /**
     * Loads the level png map to be wrapped around the path
     */
    private static void loadLevelMap()
    {
        levelImageMap = Resources.Load<Texture2D>("Images/level1");
    }

    /**
     * Instantiates a single row
     */
    private static void InstantiateRow(Vector3 location, Quaternion rotation, GameObject instantiateParent)
    {
        
        //go through this row of the image
        for(int x = 0; x < levelImageMap.width; x++)
        {
            //how many tile
            int blocksFromCenter = x-levelImageMap.width/2;
            //multiply that by the unit size of the grid to get the distance from the center it should be
            float distanceFromCenter = blocksFromCenter * gridSize;


            //get the pixel color
            Color thisPixel = levelImageMap.GetPixel(x, totalYProgress);

            //continue if this is black
            if (thisPixel.r+thisPixel.g+thisPixel.b == 0){continue;}

            //randomly choose a building to instantiate for now
            int buildingToInstantiate = 0;

            //get the pixel color and choose the instantiate type
            if (thisPixel.r > 0.3f){buildingToInstantiate = 0;}
            if (thisPixel.g > 0.3f) { buildingToInstantiate = 2; }
            if (thisPixel.b > 0.3f) { buildingToInstantiate = 6; }


            

           
            //instantiate the building now
            GameObject newCityBlock = Instantiate(allBuildings[buildingToInstantiate], location, rotation);
            //now move it to spread them out
            newCityBlock.transform.position += newCityBlock.transform.right * distanceFromCenter;
            //set the parent to the instantiateParent so we don't croud the scene
            newCityBlock.transform.parent = instantiateParent.transform;

            assignMaterialToBuilding(newCityBlock);


        }
        
        //instantiate the wall now
        GameObject newWall = Instantiate(wall, location, rotation);
        //now move it to spread them out
        //newWall.transform.position += newWall.transform.right * 5
        newWall.transform.Rotate(new Vector3(0, 90, 0));
        //set the parent to the instantiateParent so we don't croud the scene
        newWall.transform.parent = instantiateParent.transform;
        

    }

    //makes the buildings rainbow
    private static void assignMaterialToBuilding(GameObject building)
    {
     
        /*

        //Set Texture on the material
        //myNewMaterial.SetTexture("_MainTex", myTexture);
        try
        {
            //Find the Standard Shader
            Material myNewMaterial = building.GetComponent<MeshRenderer>().sharedMaterials[1];
            //myNewMaterial.SetFloat("_Metallic", 1);
            //myNewMaterial.SetFloat("_Glossiness", 1);
            myNewMaterial.SetColor("_EmissionColor", new Color(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1)));

        }
        catch(Exception e)
        {
            Material myNewMaterial = building.transform.Find("Body").GetComponent<MeshRenderer>().materials[1];
            myNewMaterial.SetColor("_EmissionColor", new Color(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1)));


        }
        //building.transform.GetComponent<Renderer>().shareMaterial
        */
    }

    /**
     * Returns a new arraylist with all the buildings in the buildings folder as gameobjcets
     */
    private static GameObject[] getBuildings()
    {
        //this is what we will be instantiating
        UnityEngine.Object[] cityBlocks = Resources.LoadAll("prefabs/LevelGeneration/");

        GameObject[] buildings = new GameObject[cityBlocks.Length];

        //now convert them to game objects
        for(int i = 0; i < cityBlocks.Length; i++)
        {
            buildings[i] = (cityBlocks[i]) as GameObject;
        }
        return buildings;
    }



    /**
     * Destroys all the instantiated elements
     */
    [MenuItem("Level Generation/Reset Level")]
    static void Reset()
    {
        //this is the parent that we will instantiate under
        GameObject instantiateParent = GameObject.FindGameObjectWithTag("Level Parent");
        Transform parentTransform = instantiateParent.transform;
        while(parentTransform.childCount > 0)
        {
            DestroyImmediate(parentTransform.GetChild(0).gameObject);
        }
    }
}