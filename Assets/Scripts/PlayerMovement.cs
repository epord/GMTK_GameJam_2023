using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Sprite idleUp;
    public Sprite idleUpRight;
    public Sprite idleRight;
    public Sprite idleDownRight;
    public Sprite idleDown;
    public Sprite idleDownLeft;
    public Sprite idleLeft;
    public Sprite idleUpLeft;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetInteger("direction", (int)MovementDirection.UP_RIGHT);
    }

    // Update is called once per frame
    void Update()
    {
        bool moving = true;

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetInteger("direction", (int)MovementDirection.UP_LEFT);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("direction", (int)MovementDirection.UP_RIGHT);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("direction", (int)MovementDirection.UP);
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetInteger("direction", (int)MovementDirection.DOWN_LEFT);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("direction", (int)MovementDirection.DOWN_RIGHT);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("direction", (int)MovementDirection.DOWN);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetInteger("direction", (int)MovementDirection.LEFT);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetInteger("direction", (int)MovementDirection.RIGHT);
        }
        else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            moving = false;
        }

        animator.SetBool("moving", moving);

        Debug.Log(animator.GetInteger("direction"));
        Debug.Log(animator.GetBool("moving"));
    }
}
public enum MovementDirection
{
    UP,
    UP_RIGHT,
    RIGHT,
    DOWN_RIGHT,
    DOWN,
    DOWN_LEFT,
    LEFT,
    UP_LEFT
}

