using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class sawControl : MonoBehaviour
{
    GameObject[] roamingArea;
    bool aradakiMesafeyiBirKereAl = true;
    Vector3 distancebtwgizmos;
    int gizmoscount = 0;
    bool forwardOrBack = true;
    void Start()
    {
        roamingArea = new GameObject[transform.childCount];
        for(int i=0; i<roamingArea.Length; i++)
        {
            roamingArea[i] = transform.GetChild(0).gameObject;
            roamingArea[i].transform.SetParent(transform.parent);
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, 5);
        goToPoint();
    }

    void goToPoint()
    {
        if(aradakiMesafeyiBirKereAl)
        {
            distancebtwgizmos = (roamingArea[gizmoscount].transform.position - transform.position).normalized;
            aradakiMesafeyiBirKereAl = false;
        }
        float distance = Vector3.Distance(transform.position, roamingArea[gizmoscount].transform.position);
        transform.position += distancebtwgizmos * Time.deltaTime * 10;
        if(distance < 0.5f) //3 -> 0, 1, 2
        {
            
            if(gizmoscount == roamingArea.Length-1)
            {
                forwardOrBack = false;
            }
            if(gizmoscount == 0)
            {
                forwardOrBack = true;
            }
            if(forwardOrBack == true)
            {
                gizmoscount++;
                aradakiMesafeyiBirKereAl = true;
            }
            else
            {
                gizmoscount--;
                aradakiMesafeyiBirKereAl = true;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        
        for(int i=0; i<transform.childCount-1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position);
        }
    }
#endif
}







#if UNITY_EDITOR
[CustomEditor(typeof(sawControl))]
[System.Serializable]
class sawEditor: Editor
{
    public override void OnInspectorGUI()
    {
        sawControl script = (sawControl)target;
        if(GUILayout.Button("Produce"))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform;
            newObject.name = script.transform.childCount.ToString();
        }
    }
}

#endif
