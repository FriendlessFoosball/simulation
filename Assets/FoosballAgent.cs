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

    public bool player1;
    public GameObject offense;
    public GameObject goalie;

    //TODO: Tune these
    float linMoveMultiplier = 0.25f;
    float angMoveMultiplier = 5f;

    float ballStartMultiplier = 20f;

    float maxLinMove = 2f;

    float frozenTime = 0f;
    float maxFrozenTime = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    //Reset ball to middle with random force
    public override void OnEpisodeBegin()
    {
        frozenTime = 0f;
        ball.transform.localPosition = Vector3.zero;
        ball.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * ballStartMultiplier);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Ball Observations
        sensor.AddObservation(ball.transform.localPosition);
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.x);
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.y);
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.z);
        //Possibly add observe goal position?

        //Paddle Observations
        sensor.AddObservation(offense.transform.localPosition);
        sensor.AddObservation(offense.transform.localRotation);
        sensor.AddObservation(goalie.transform.localPosition);
        sensor.AddObservation(goalie.transform.localRotation);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Actions, size = 4
        Vector3 offenseLinMove = Vector3.up * actions.ContinuousActions[0] * linMoveMultiplier;
        if (player1 && Mathf.Abs(offense.transform.localPosition.z + offenseLinMove.y) < maxLinMove)
        {
            offense.transform.Translate(offenseLinMove);
        }
        else if (!player1 && Mathf.Abs(offense.transform.localPosition.z - offenseLinMove.y) < maxLinMove)
        {
            offense.transform.Translate(offenseLinMove);
        }
        Vector3 offenseAngMove = Vector3.up * actions.ContinuousActions[1] * angMoveMultiplier;
        offense.transform.Rotate(offenseAngMove);

        Vector3 goalieLinMove = Vector3.up * actions.ContinuousActions[2] * linMoveMultiplier;
        if (player1 && Mathf.Abs(goalie.transform.localPosition.z + goalieLinMove.y) < maxLinMove)
        {
            goalie.transform.Translate(goalieLinMove);
        }
        else if (!player1 && Mathf.Abs(goalie.transform.localPosition.z - goalieLinMove.y) < maxLinMove)
        {
            goalie.transform.Translate(goalieLinMove);
        }
        Vector3 goalieAngMove = Vector3.up * actions.ContinuousActions[3] * angMoveMultiplier;
        goalie.transform.Rotate(goalieAngMove);

        //Rewards
        float distanceToGoal = Vector3.Distance(ball.transform.localPosition, goal.localPosition);
        //Reached Target. TODO: Change this to work with the trigger  instead:
        if (distanceToGoal <  1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        //TODO: neg reward for owngoal? Reset episode if ball falls off?
        //Reset if ball stops moving for 10 secs or falls off plane
        if (ball.GetComponent<Rigidbody>().IsSleeping() || ball.transform.position.y < -5)
        {
            if (frozenTime != 0f && Time.time - frozenTime > maxFrozenTime)
            {
                EndEpisode();
            }
            else if (frozenTime == 0f)
            {
                frozenTime = Time.time;
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");

        continuousActionsOut[2] = Input.GetAxis("Vertical");
        continuousActionsOut[3] = Input.GetAxis("Horizontal");
    }
}
