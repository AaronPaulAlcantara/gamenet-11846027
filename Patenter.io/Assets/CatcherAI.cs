using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CatcherAI : MonoBehaviourPunCallbacks
{
    public Vector2 direction;
    public float speed;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                if(direction == Vector2.right && transform.position.x<11)
                {
                    transform.Translate(direction.x * speed *Time.deltaTime , 0, 0);
                }
                else if(direction == Vector2.right && transform.position.x >= 11)
                {
                    direction = Vector2.left;
                }
                else if (direction == Vector2.left && transform.position.x > -11)
                {
                    transform.Translate(direction.x * speed * Time.deltaTime, 0, 0);
                }
                else if (direction == Vector2.left && transform.position.x <= -11)
                {
                    direction = Vector2.right;
                }
    }
}
