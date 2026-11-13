using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicles : MonoBehaviour
{

    //public NavMeshAgent agent;
    //public Animator animator;

    public GameObject PATH;
    public Transform[] vehicles;
    public float safeDistance = 10f;

    public int currentWP = 0;

    public float speed = 10f;

    public float rotSpeed = 1.0f;

    public float minDistance = 10;

    public string[] tags;

    [Header("Sensors")]

    public float sensorLength = 5f;
    public Vector3 frontsensorPosition = new Vector3(0f, 0.2f, 0.5f);
    public float frontsidesensorPosition = 0.2f;
    public float frontSensorAngle = 30f;

    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();

        vehicles = new Transform[PATH.transform.childCount];
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i] = PATH.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

       /* RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, safeDistance);

        if (hit.transform)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                if (hit.transform.tag == tags[i])
                {
                    speed = 10f;
                }
            }
        }
        else
        {
            Move();
        }
       */
       
    }

    void Move()
    {
        if (Vector3.Distance(this.transform.position, vehicles[currentWP].transform.position) < minDistance)
        {
            if (currentWP >= 0 && currentWP < vehicles.Length)
            {
                currentWP += 1;
            }
        }

        if (currentWP >= vehicles.Length)
        {
            currentWP = 0;
        }

        Quaternion lookatWP = Quaternion.LookRotation(vehicles[currentWP].transform.position - this.transform.position);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookatWP, rotSpeed * Time.deltaTime);

        this.transform.Translate(0, 0, speed * Time.deltaTime);

        //agent.SetDestination(vehicles[currentWP].position);
        //animator.SetFloat("vertical", !agent.isStopped ? 1 : 0);

    }


    private void FixedUpdate()
    {
        sensors();
    }

    private void sensors()
    {
        RaycastHit hit;
        Vector3 sensorstartPos = transform.position + frontsensorPosition;
        
         //front centre line
        if(Physics.Raycast(sensorstartPos,transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorstartPos, hit.point);

           /* if (hit.transform)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    if (hit.transform.tag == tags[i])
                    {
                        //speed = 0f;

                    }
                }
            }*/
        }
        else
        {
            Move();
        }

        

        //front right sensor
        sensorstartPos.x += frontsidesensorPosition;
        if (Physics.Raycast(sensorstartPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorstartPos, hit.point);
        }

        //front right angle sensor
       
        if (Physics.Raycast(sensorstartPos, Quaternion.AngleAxis(frontSensorAngle,transform.up) * transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorstartPos, hit.point);
        }

        //front left sensor
        sensorstartPos.x -= 2 * frontsidesensorPosition;
        if (Physics.Raycast(sensorstartPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorstartPos, hit.point);
        }

        //front left angle sensor

        if (Physics.Raycast(sensorstartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorstartPos, hit.point);
        }


       // Physics.Raycast(transform.position, transform.forward, out hit, safeDistance);

        
    }


}

