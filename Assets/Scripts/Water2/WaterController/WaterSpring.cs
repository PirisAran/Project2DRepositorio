using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using TecnocampusProjectII;

public class WaterSpring : MonoBehaviour
{
    [SerializeField] Transform _player;
    public float velocity = 0;
    public float force = 0;
    // current height
    public float height = 0f;
    // normal height
    private float target_height = 0f;
    public Transform springTransform;
    [SerializeField]
    private static SpriteShapeController spriteShapeController = null;
    private int waveIndex = 0;
    private List<WaterSpring> springs = new();
    private float resistance = 40f;


    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }

    public void Init(SpriteShapeController ssc) { 

        var index = transform.GetSiblingIndex();
        waveIndex = index+1;
        
        spriteShapeController = ssc;
        velocity = 0;
        height = transform.localPosition.y;
        target_height = transform.localPosition.y;
    }
    // with dampening
    // adding the dampening to the force
    public void WaveSpringUpdate(float springStiffness, float dampening) { 
        height = transform.localPosition.y;
        // maximum extension
        var x = height - target_height;
        var loss = -dampening * velocity;

        force = - springStiffness * x + loss;
        velocity += force;
        var y = transform.localPosition.y;  
        transform.localPosition = new Vector3(transform.localPosition.x, y+velocity, transform.localPosition.z);
  
    }
    public void WavePointUpdate() { 
        if (spriteShapeController != null) {
            Spline waterSpline = spriteShapeController.spline;
            Vector3 wavePosition = waterSpline.GetPosition(waveIndex);
            waterSpline.SetPosition(waveIndex, new Vector3(wavePosition.x, transform.localPosition.y, wavePosition.z));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            return;
        }

        

        var speed = rb.velocity;
        velocity += speed.y / resistance;
    }

}