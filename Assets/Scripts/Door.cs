using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2 fromPos;
    public Vector2 toPos;

    public bool horizontal;
    public bool positive;

   
    

    public void SetPositions(Vector2 from, Vector2 to)
    {
        fromPos = from;
        toPos = to;
        if (to.y == from.y)
        {
            horizontal = true;
            if(to.x > from.x)
            {
                positive = true;
            }else positive = false;
        }
        else
        {
            horizontal = false;
            if(to.y > from.y)
            {
                positive = true;
            }else positive = false;
        }

    }

    public Vector2 getFromPos()
    {
        return fromPos;
    }

    public Vector2 getToPos()
    {
        return toPos;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if(horizontal)
            {
                if(positive)
                {
                    if (player.GetPlayerDirection() == Vector2.right)
                    {
                        CameraFollow.instance.MoveCamera(fromPos, toPos);
                    }
                }
                else
                {
                    if (player.GetPlayerDirection() == Vector2.left)
                    {
                        CameraFollow.instance.MoveCamera(fromPos, toPos);
                    }
                }

            }
            else
            {
                if (positive)
                {
                    if (player.GetPlayerDirection() == Vector2.up)
                    {
                        CameraFollow.instance.MoveCamera(fromPos, toPos);
                    }
                }
                else
                {
                    if (player.GetPlayerDirection() == Vector2.down)
                    {
                        CameraFollow.instance.MoveCamera(fromPos, toPos);
                    }
                }
            }
        }
    }


}
