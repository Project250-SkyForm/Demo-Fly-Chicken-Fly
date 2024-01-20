using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : AnimatedEntity
{   
    private float Gravity = -9.81f;
    private float Speed = 0;
    private bool falling = true;
    private float horizontal_speed=10.0f; //horizontal_speed_left = 10.0f, horizontal_speed_right = 10.0f;
    private AudioSource audioSource;
    private Platform current_platform;
    private SpriteRenderer spriteRenderer;
    private int lumb = 3;

    public List<Sprite> interruptAnimationCycle;

    void Start()
    {
        AnimationSetup();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationUpdate(); 

        // Going up
        if (Input.GetKey(KeyCode.W) && falling==false){
            if (current_platform != null){
                    current_platform.refresh();
            }
                
            Speed = 10;
            falling = true;
            current_platform = null;     // to avoid null bug
        }

        // Going left
        if(Input.GetKey(KeyCode.A)){
            // horizontal_speed_right = 10.0f;
            transform.position+= Vector3.left*Time.deltaTime*horizontal_speed;
            if (current_platform!=null){
                if (transform.position.x < current_platform.getLeft()){ //detect whether player go beyond the left edge of the platform
                    falling = true;
                }
            }
        }

        // Going down
        if(Input.GetKey(KeyCode.D)){
            // horizontal_speed_left = 10.0f;
            transform.position+= Vector3.right*Time.deltaTime*horizontal_speed;
            if (current_platform!=null){
                if (transform.position.x > current_platform.getRight()){ //detect whether player go beyond the right edge of the platform
                    falling = true;
                }
            }
        }


        if (falling){
            Speed += Gravity*Time.deltaTime;
            transform.position+=Vector3.up*Time.deltaTime*Speed;
        }
        
    }

    void OnTriggerEnter(Collider other){
        Platform platform = other.gameObject.GetComponent<Platform>();

        if(platform!=null){
            if (audioSource != null)
            {
                audioSource.Play();
            }
            Interrupt (interruptAnimationCycle);
            
            if (transform.position.y + CalculateEdges("down") >= platform.getTop()){
                Speed = 0;
                falling = false;
                current_platform = platform;    //record the information of the collider maybe
            }
            else{
                Speed = 0;   
                current_platform = platform;
                Vector3 newPosition = new Vector3(current_platform.transform.position.x, current_platform.getTop()-CalculateEdges("down"), transform.position.z);
                transform.position = newPosition;
                falling = false;
                lumb -= 1;
                Debug.Log("You get a lumb! You only have:"+ lumb );
            }
        }
    }

    public float CalculateEdges(string direction)     // some of the codes are from chatgpt,
    //return one of the edges of the sprite
    {
        // Ensure the object has a SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            // Get the left and right edges of the platform
            float leftEdge =  - spriteRenderer.bounds.size.x / 2f;
            float rightEdge =  spriteRenderer.bounds.size.x / 2f;
            float topEdge =  spriteRenderer.bounds.size.y / 2f;
            float downEdge = - spriteRenderer.bounds.size.y / 2f;
            if (direction=="left"){
                return leftEdge;
            }
            else if (direction == "right"){
                return rightEdge;
            }
            else if (direction == "top"){
                return topEdge;
            }
            else if (direction == "down"){
                //Debug.Log("Down"+ downEdge);
                return downEdge;
            }
            else{   // should not happen, just in case debug warning
                return 0;
            }
        }
        else{   // should not happen, just in case debug warning
                return 0;
            }
    }
}
