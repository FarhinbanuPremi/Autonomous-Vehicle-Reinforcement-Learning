using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pedestrian : MonoBehaviour
{

    public NavMeshAgent agent;
    public Animator animator;

    public GameObject PATH;
    public Transform[] PathPoints;

    public int index = 0;

    public float minDistance = 10;

    public float speed = 5.0f;

    // private float minDistance;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        PathPoints = new Transform[PATH.transform.childCount];
        for(int i = 0; i < PathPoints.Length; i++)
        {
            PathPoints[i] = PATH.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        roum();

    }

    void roum()
    {

        if (Vector3.Distance(this.transform.position, PathPoints[index].transform.position) < minDistance)
        {
            if (index >= 0 && index < PathPoints.Length)
            {
                index += 1;
            }
            /* else
             {
                 index = 0;
             }*/

           
        }
        if (index >= PathPoints.Length)
        {
            index = 0;
        }
        agent.SetDestination(PathPoints[index].transform.position);

        animator.SetFloat("verticle", !agent.isStopped ? 1 : 0);

        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }

    
}
