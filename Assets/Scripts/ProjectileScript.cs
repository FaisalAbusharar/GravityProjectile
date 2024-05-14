using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D RigidBody; // RigidBody of Projectile
    [SerializeField] float ForceAdded = 1500; // Force Applied on RigidBody
    [SerializeField] float Gravity = 9.8f; // Gravity Applied on RigidBody
    [SerializeField] TMP_Text SpeedText; // Display the Speed
    [SerializeField] TMP_Text GravityDisplay; // Display current Gravity
    [SerializeField] Button IncreaseGravityButton; // Button to increase Gravity
    [SerializeField] Button DecreaseGravityButton; // Button to Decrease Gravity
    [SerializeField] TMP_Text ForceDisplay; // Display current force
    [SerializeField] Button IncreaseForceButton; // Button to increase force
    [SerializeField] Button DecreaseForceButton; // Button to Decrease force
    [SerializeField] TrailRenderer ProjectileLine; // The Projectile Trail
    [SerializeField] GameObject Cloner; // Clone Gameobject/Script


    private bool firstLoad = true;
    private int speed; 


    void Start()
    {



        if (Gravity == 1)
        {
            Gravity = 10f; //Earth's actual gravity is 9.8, but we round since it looks better.
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.S)) { ThrowFunction();  } // Calls the Throwing Function / Start Function

        if (Input.GetKeyDown(KeyCode.R)) { ResetFunction();  } // Calls the Reset Function

        if (Input.GetKeyDown(KeyCode.UpArrow)) { IncreaseGravity();  };
        if (Input.GetKeyDown(KeyCode.DownArrow)) { DecreaseGravity(); };



        IEnumerator CalcSpeed()
        {
            bool isPlaying = true;

            while (isPlaying)
            {
                Vector3 prevPos = transform.position;  // Grab the position of the transform (Projectile)

                yield return new WaitForFixedUpdate();

                speed = Mathf.RoundToInt(Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime); // Yoinked this from google.
            }
        }


        if (firstLoad == true) // Start the Speed Calc Stuff
        {
            firstLoad = false;
            StartCoroutine(CalcSpeed());
        }



        // ------------ UPDATE UI ------------

        SpeedText.text = "SPEED: " + speed + " Px/F"; // Speed update
        GravityDisplay.text = "Current Gravity: " + Gravity + "N";  // Gravity Update
        ForceDisplay.text = "Current Force: "  + ForceAdded + "N"; // Force Update


      


    }


    private void Awake()
    {
        // adding a delegate with no parameters
        IncreaseGravityButton.onClick.AddListener(IncreaseGravity);
        DecreaseGravityButton.onClick.AddListener(DecreaseGravity);
        IncreaseForceButton.onClick.AddListener(IncreaseForce);
        DecreaseForceButton.onClick.AddListener(DecreaseForce);


    }

    private void IncreaseGravity() {Gravity = Gravity + 1;}
    private void DecreaseGravity() { Gravity = Gravity - 1; }
    private void DecreaseForce() { ForceAdded = ForceAdded - 50; }
    private void IncreaseForce() { ForceAdded = ForceAdded + 50; }







    void ThrowFunction()
    {
        RigidBody.drag = 0;
        ProjectileLine.emitting = true;
        RigidBody.gravityScale = Gravity / 9.8f; //Set the gravity according to the given Value and divide by earth's actual gravity (for realism)
        RigidBody.AddForce(transform.up * ForceAdded); // Give ball force upwards
        RigidBody.AddForce(transform.right * ForceAdded); // Move right with same force as up to make a diagonal
        
    }

    void ResetFunction()
    {
        RigidBody.gravityScale = 0; // Reset Gravity too
        RigidBody.velocity = new Vector2(0, 0); // Reset Speed incase there is still some left somehow
        RigidBody.rotation = 0; // Reset Rotation so ball doesn't get pushed in the wrong direction
        RigidBody.angularVelocity = 0; // Set this velocity to 0 so it doesnt cause rotation
        Cloner.GetComponent<CloneProjectile>().CloneBall(); // Clone the ball from the cloner script.
     

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        RigidBody.drag = 0.5f; // Add firction with the floor so it doesnt go to infinity lmao
        ProjectileLine.emitting = false;
    }

}
