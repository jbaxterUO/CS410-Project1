using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public float speed = 0.0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public float jumpAmount = 5;
    public float gravityScale = 2.75f;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool onGround = true;
    private bool hasJumped = false;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump())
            {
                Jump();
            }
        }

    }

    bool canJump()
    {
        if (onGround)
        {
            onGround = false;
            return true;
        }
        else if (!hasJumped && !onGround) 
        {
            hasJumped =true;
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {
        //Formula found online for giving a good approximate fixed jump height
        float jumpForce = Mathf.Sqrt(jumpAmount * -2 * (Physics2D.gravity.y));
        rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
    }

    void OnMove(InputValue movementValue)
    {
        //OnMove Function Body
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 12)
        {
            winTextObject.SetActive(true);
            countText.text = "";
            ResetPlayer();
        }
    }

    void FixedUpdate() 
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        
    }


    void ResetPlayer()
    {
        transform.position = new Vector3(0, 0.5f, 0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            hasJumped = false;

        }

        if (other.gameObject.CompareTag("Respawn"))
        {
            ResetPlayer();
        }
        
    }

}
