using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    public float Reward = 0f;
    
    public float Panalty = 0f;

    float moveSpeed;

    [Header("Sensors")]

    public float sensorLength = 5f;
    public Vector3 frontsensorPosition = new Vector3(0f, 0.2f, 0.5f);
    public float frontsidesensorPosition = 0.2f;
    public float frontSensorAngle = 30f;


    public override void OnEpisodeBegin()
    {
        transform.position = Vector3.zero;
    }

    // agent observe the environment
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.ContinuousActions[0]);


        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        

        moveSpeed = 5f;
        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
        {
            AddReward(+1f);
            Reward++;
            floorMeshRenderer.material = winMaterial;
            //EndEpisode();
        }
       
        /*if (other.TryGetComponent<Sidewalk_10>(out Sidewalk_10 sidewalk))
        {
            AddReward(-1f);
            Panalty = Reward++;
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
            Reward++;

        else if (collision.gameObject.CompareTag("Sidewalk_10") || collision.gameObject.CompareTag("car") || collision.gameObject.CompareTag("Pedestrian") || collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1.0f);
            Panalty++;

            floorMeshRenderer.material = loseMaterial;

            EndEpisode();
        }

        if(collision.gameObject.CompareTag("Traffic Light"))
        {
            moveSpeed = 0f;
        }
       
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
        if (Physics.Raycast(sensorstartPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorstartPos, hit.point);
        }
        else
        {
           // moveCar();

            
        }



        //front right sensor
        sensorstartPos.x += frontsidesensorPosition;
        if (Physics.Raycast(sensorstartPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorstartPos, hit.point);
        }

        //front right angle sensor

        if (Physics.Raycast(sensorstartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
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
