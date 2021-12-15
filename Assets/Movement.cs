using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;

    public Transform cam;

    public float jumpPower;

    private float gravity;
    public float gravityPower;
    public float gravityMax;

    private float speed;
    public float speedAccel;
    public float speedDeccel;
    public float maxSpeed;

    public float turnSmooth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            speed = Mathf.Clamp(speed + Time.deltaTime * speedAccel, 0f, maxSpeed);

            float lookAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAngle, ref turnSmooth, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = (Quaternion.Euler(0f, lookAngle, 0f) * Vector3.forward).normalized;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Clamp(speed - Time.deltaTime * speedDeccel, 0f, maxSpeed);
        }


        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                gravity += jumpPower;
            }
            else
            {
                gravity = 0f;
            }
        }
        else
        {
            gravity = Mathf.Max(gravity - Time.deltaTime * gravityPower, -gravityMax);
        }

        controller.Move(Vector3.up * gravity * Time.deltaTime);

    }
}
