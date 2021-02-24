using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FoosballAgent : Agent
{
    public GameObject ball;
    public Transform goal;

    Rigidbody rBody;

    //TODO: Tune these
    float linForceMultiplier = 10;
    float angForceMultiplier = 10;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    //Use OnEpisodeBegin for setup if needed

    public override void CollectObservations(VectorSensor sensor)
    {
        //Ball Observations
        sensor.AddObservation(ball.transform.localPosition);
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.x);
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.y);
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.z);

        //Paddle Observations
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.localRotation);
        sensor.AddObservation(rBody.velocity.z); //Make sure z is what we want
        sensor.AddObservation(rBody.angularVelocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Actions, size = 2
        Vector3 linMove = Vector3.up * actions.ContinuousActions[0];
        rBody.AddForce(linMove * linForceMultiplier);

        Vector3 angMove = Vector3.up * actions.ContinuousActions[1];
        rBody.AddTorque(angMove * angForceMultiplier);

        //Rewards
        float distanceToGoal = Vector3.Distance(ball.transform.localPosition, goal.localPosition);
        //Reached Target. TODO: Change this to work with the trigger  instead:
        if (distanceToGoal <  1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        //TODO: neg reward for owngoal? Reset episode if ball falls off?
    }
}
