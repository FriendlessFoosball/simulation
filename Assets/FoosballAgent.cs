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
    float linMoveMultiplier = 0.25f;
    float angMoveMultiplier = 5f;

    float ballStartMultiplier = 15f;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    //Use OnEpisodeBegin for setup if needed
    public override void OnEpisodeBegin()
    {
        ball.transform.localPosition = Vector3.zero;
        ball.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * 15);
        //ball.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * ballStartMultiplier);
    }

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
        sensor.AddObservation(rBody.angularVelocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Actions, size = 2
        Vector3 linMove = Vector3.up * actions.ContinuousActions[0] * linMoveMultiplier;
        transform.Translate(linMove);

        Vector3 angMove = Vector3.up * actions.ContinuousActions[1] * angMoveMultiplier;
        transform.Rotate(angMove);

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

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}
