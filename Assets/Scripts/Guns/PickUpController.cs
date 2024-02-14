using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GunSystem gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;

    public float pickUpRange, pickUpTime;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            slotFull = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
        }
    }


    private void Update()
    {
        //Check if player is in range and "E" is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();


        // Drop if equipped and G is pressed.
        if (equipped && Input.GetKeyDown(KeyCode.G)) Drop();


    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //make weapon a child of the camera and move it to default position.
        transform.SetParent(gunContainer);
        transform.localPosition = new Vector3(0.358999878f, -0.36500001f, 0.555999994f);
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 278.258636f, 0f));
        transform.localScale = new Vector3(1.85775125f, 1.85775137f, 1.85775125f);

        // Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;
        
        // Enable Gun Script.
        gunScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;


        //Add force
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        gunScript.enabled = false;
    }
}
