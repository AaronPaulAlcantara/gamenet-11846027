using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayMovementController : MonoBehaviour
{
    public Joystick joystick;
    private RigidbodyFirstPersonController rigidbodyFirstPersonController;
    public FixedTouchField fixedTouchField;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyFirstPersonController = GetComponent<RigidbodyFirstPersonController>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigidbodyFirstPersonController.JoystickInput.x = joystick.Horizontal;
        rigidbodyFirstPersonController.JoystickInput.y = joystick.Vertical;
        rigidbodyFirstPersonController.mouseLook.LookInputAxis = fixedTouchField.TouchDist;

        animator.SetFloat("Horizontal", joystick.Horizontal);
        animator.SetFloat("Vertical", joystick.Vertical);

        if(Mathf.Abs(joystick.Horizontal)>0.9  || Mathf.Abs(joystick.Vertical) > 0.9)
        {
            animator.SetBool("IsRunning", true);
            rigidbodyFirstPersonController.movementSettings.ForwardSpeed = 10;
        }
        else
        {
            animator.SetBool("IsRunning", false);
            rigidbodyFirstPersonController.movementSettings.ForwardSpeed = 5;
        }
    }
}
