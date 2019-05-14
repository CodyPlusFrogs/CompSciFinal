using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class RingGeneration : EditorWindow
{


    [MenuItem("Level Generation/Instantiate Rings")]
    static void InstantiateRings()
    {
        //this is the parent that we will instantiate all of the level under
        GameObject instantiateParent = GameObject.FindGameObjectWithTag("Ring Parent");
        //this object contains a bunch of cubes that will show us our path
        GameObject ringPath = GameObject.FindGameObjectWithTag("Ring Path");
        //get the transform
        Transform ringPathTransform = ringPath.GetComponent<Transform>();


        //get the ring object from the resources folder
        GameObject ringGameObject = Resources.Load("prefabs/Ring") as GameObject;




        //amount of spacing between rings
        double spacing = 80;


        //first we are going to make sure the spacing on the path is equal throughout
        for (int i = 0; i < ringPathTransform.childCount-1; i++)
        {
            //the first block will be the start of the line
            Transform lineStartTransform = ringPathTransform.GetChild(i);

            //the second block will be the end of the line (look back to the 0th one on the end)
            Transform lineEndTransform = ringPathTransform.GetChild(i + 1);

            Transform nextLineEndTransform = ringPathTransform.GetChild(i + 2 < ringPathTransform.childCount? i + 2 : ringPathTransform.childCount-1);

            

            //we will only make into a spline half of the lines
            Vector3 midPoint1 = Vector3.Lerp(lineStartTransform.position, lineEndTransform.position, 0.5f);
            Vector3 midPoint2 = Vector3.Lerp(lineEndTransform.position, nextLineEndTransform.position, 0.5f);


            //the last position we instantiated from
            Vector3 lastInstantiate = new Vector3(-10000, 0, 0);
            for (float percent = 0; percent < 1; percent += 0.01f)
            {
                Vector3 firstLerp = Vector3.Lerp(midPoint1, lineEndTransform.position, percent);
                Vector3 secondLerp = Vector3.Lerp(lineEndTransform.position, midPoint2, percent);
                Vector3 splineLerp = Vector3.Lerp(firstLerp, secondLerp, percent);

                if (Vector3.Distance(splineLerp, lastInstantiate) > spacing)
                {
                    //compute the heading toward the end transform
                    Vector3 heading = secondLerp - firstLerp;

                    //instantiate the ring now
                    GameObject newRing = Instantiate(ringGameObject, splineLerp, Quaternion.LookRotation(heading,
                        Vector3.Lerp((lineStartTransform.up + lineEndTransform.up) / 2,
                        (lineEndTransform.up + nextLineEndTransform.up) / 2, percent)));
                    
                    //set the parent to the instantiateParent so we don't croud the scene
                    newRing.transform.parent = instantiateParent.transform;

                    lastInstantiate = splineLerp;
                }


            }
        }
    }

    [MenuItem("Level Generation/Clear Rings")]
    static void ClearRings()
    {
        //this is the parent that we will instantiate under
        GameObject instantiateParent = GameObject.FindGameObjectWithTag("Ring Parent");
        Transform parentTransform = instantiateParent.transform;
        while (parentTransform.childCount > 0)
        {
            DestroyImmediate(parentTransform.GetChild(0).gameObject);
        }
    }


}
