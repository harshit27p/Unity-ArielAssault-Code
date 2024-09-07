using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down based upon player input")][SerializeField] float controlSpeed = 10f;
    [Tooltip("How fast player moves horizontally")][SerializeField] float xRange = 10f;
    [Tooltip("How fast player moves vertically")][SerializeField] float yRange = 7f;

    [Header("Laser gun array")]
    [Tooltip("Add all player laser here")]
    [SerializeField] GameObject[] lasers;


    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = 2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Player input based tuning")]
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = -20f;

    float xThrow;
    float yThrow;

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xoffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xoffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yoffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yoffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        //if pusing fire button
        //then print "shooting"
        //else don't print "shooting"
        if (Input.GetButton("Fire1"))
        {
            //Debug.Log("I'm shooting");
            //ActiveLasers();
            SetLasersActive(true);

        }
        else
        {
            //Debug.Log("I'm not shooting"); 
            //DeactiveLaser(); 
            SetLasersActive(false);
        }

        void SetLasersActive(bool isActive)
        {
            foreach (GameObject laser in lasers)
            {
                //laser.SetActive(true);
                var emissionModule = laser.GetComponent<ParticleSystem>().emission;
                emissionModule.enabled = isActive;
            }
        }

        /*
            void DeavtiveLasers()
            {
                foreach (GameObject laser in lasers)
                {
                    //laser.SetActive(false);
                    var emissionModule = laser.GetComponent<ParticleSystem>().emission;
                    emissionModule.enabled = false;
                }
            }
        */
    }
}
