using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 10f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = 0.2f;

    // State
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // Cached component references
    private AudioClip clip;
    Rigidbody2D myRigidBody2D;
    Animator animator;

    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchBallOnMouseClick();
        }
        else
        {
            var dir = myRigidBody2D.velocity;
            var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
            myRigidBody2D.MoveRotation(angle);
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void LaunchBallOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            animator = GetComponent<Animator>();
            animator.enabled = true;
            myRigidBody2D.velocity = new Vector2(xPush, yPush);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2(Random.Range(0f, randomFactor), Random.Range(0f, randomFactor));
        if (hasStarted)
        {
            myRigidBody2D.velocity += velocityTweak;
            if (!collision.gameObject.CompareTag("Block"))
            {
                clip = ballSounds[0];
            }
            else
            {
                clip = ballSounds[1];
            } 
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}
