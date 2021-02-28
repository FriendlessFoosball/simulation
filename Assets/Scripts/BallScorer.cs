using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScorer : MonoBehaviour
{
    public GameObject humanAgent;
    public GameObject humanOffense;
    public GameObject humanGoalie;
    public GameObject robotAgent;
    public GameObject robotOffense;
    public GameObject robotGoalie;
    
    FoosballAgent m_humanAgent;
    FoosballAgent m_robotAgent;

    PaddleInitializer m_humanOffense;
    PaddleInitializer m_humanGoalie;
    PaddleInitializer m_robotOffense;
    PaddleInitializer m_robotGoalie;

    Rigidbody rb;

    const float fieldWidth = 12.835f;
    const float fieldLength = 18.47f;
    const float ballStartMultiplier = 1000f; //20f;

    // Start is called before the first frame update
    void Start()
    {
        m_humanAgent = humanAgent.GetComponent<FoosballAgent>();
        m_robotAgent = robotAgent.GetComponent<FoosballAgent>();

        m_humanOffense = humanOffense.GetComponent<PaddleInitializer>();
        m_humanGoalie = humanGoalie.GetComponent<PaddleInitializer>();
        m_robotOffense = robotOffense.GetComponent<PaddleInitializer>();
        m_robotGoalie = robotGoalie.GetComponent<PaddleInitializer>();
        
        rb = GetComponent<Rigidbody>();

        ResetBall();
    }

    void Reset()
    {
        m_humanAgent.EndEpisode();
        m_robotAgent.EndEpisode();

        m_humanOffense.Reset();
        m_humanGoalie.Reset();
        m_robotOffense.Reset();
        m_robotGoalie.Reset();

        ResetBall();
    }

    public void ResetBall()
    {
        transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(Random.onUnitSphere * ballStartMultiplier);
    }

    void HumanWin()
    {
        m_humanAgent.SetReward(1f);
        m_robotAgent.SetReward(-1f);
        Reset();
    }

    void RobotWin()
    {
        m_humanAgent.SetReward(-1f);
        m_robotAgent.SetReward(1f);
        Reset();
    }

    void Failure()
    {
        m_humanAgent.SetReward(-1f);
        m_robotAgent.SetReward(-1f);
        Reset();
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Robot"))
            RobotWin();
        else if (collision.gameObject.CompareTag("Human"))
            HumanWin();
    }

    void FixedUpdate()
    {
        if (transform.localPosition.y < -5 || Mathf.Abs(transform.localPosition.x) > (fieldLength + 2.75) || Mathf.Abs(transform.localPosition.z) > (fieldWidth + 2.75))
            Failure();
    }
}
