using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource rocketThrust;
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;


	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        rocketThrust = GetComponent<AudioSource>();

        rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
	}
	
	// Update is called once per frame
	void Update () {                        //todo stop sound on death
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);    //parametarized this time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);   // kill the player
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(0);  //todo allow for more than 2 levels
    }

    private void Thrust()
    {

        if (Input.GetKey(KeyCode.Space))    //can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!rocketThrust.isPlaying)
            {
                rocketThrust.Play();
            }
        }
        else
        {
            rocketThrust.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;        // take manual control of rotation


        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;       // Control from physics
    }
}
