using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyControl : MonoBehaviour
{
    GameObject[] roamingArea;
    bool aradakiMesafeyiBirKereAl = true;
    Vector3 distancebtwgizmos;
    int gizmoscount = 0;
    bool forwardOrBack = true;
    GameObject character;
    RaycastHit2D ray;
    public LayerMask layermask;
    int speed = 10;
    public Sprite frontOfEnemy;
    public Sprite backOfEnemy;
    SpriteRenderer spr;
    public GameObject bullet;
    float firetime = 0;
    void Start()
    {
        roamingArea = new GameObject[transform.childCount];
        spr = GetComponent<SpriteRenderer>();
        character = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < roamingArea.Length; i++)
        {
            roamingArea[i] = transform.GetChild(0).gameObject;
            roamingArea[i].transform.SetParent(transform.parent);
        }
        
        
    }

    void FixedUpdate()
    {
        seeMe();

        if (ray.collider.tag == "Player")
        {
            speed = 12;
            spr.sprite = frontOfEnemy;
            fire();

        }
        else
        {
            speed = 7;
            spr.sprite = backOfEnemy;
        }

        goToPoint();
    }

    void goToPoint()
    {
        if (aradakiMesafeyiBirKereAl)
        {
            distancebtwgizmos = (roamingArea[gizmoscount].transform.position - transform.position).normalized;
            aradakiMesafeyiBirKereAl = false;
        }
        float distance = Vector3.Distance(transform.position, roamingArea[gizmoscount].transform.position);
        transform.position += distancebtwgizmos * Time.deltaTime * speed;
        if (distance < 0.5f) 
        {

            if (gizmoscount == roamingArea.Length - 1)
            {
                forwardOrBack = false;
            }
            if (gizmoscount == 0)
            {
                forwardOrBack = true;
            }
            if (forwardOrBack == true)
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

    void seeMe()
    {
        Vector3 rayDirection = character.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position, rayDirection, 1000, layermask);
        Debug.DrawLine(transform.position, ray.point, Color.magenta);
    }

    void fire()
    {
        firetime += Time.deltaTime;
        if(firetime > 1)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            firetime = 0;
        }
    }

    public Vector2 getDirection()
    {
        return (character.transform.position - transform.position).normalized;
    }










#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyControl))]
[System.Serializable]
class EnemyControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyControl script = (EnemyControl)target;
        if (GUILayout.Button("Produce"))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform;
            newObject.name = script.transform.childCount.ToString();
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layermask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("frontOfEnemy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("backOfEnemy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bullet"));
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}

#endif
