using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    public float speed = 10;
    public float rotationSpeed = 200;
    public float currentSpeed = 0;
    private float nextJump = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && Time.time>nextJump)
        {
            Debug.Log("Jump");
            nextJump = Time.time+1.5f;
            animator.SetTrigger("Jump");
            GetComponent<Rigidbody>().AddForce(Vector3.up*5f,ForceMode.Impulse);
        }

        transform.Translate(0, 0, translation);
        currentSpeed = translation;
        animator.SetFloat("Speed", currentSpeed*201);
        transform.Rotate(0, rotation, 0);

    }

    
}
