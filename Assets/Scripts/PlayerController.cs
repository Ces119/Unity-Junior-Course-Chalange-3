using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem explosionEffect;
    public ParticleSystem dirtyParticleSystem;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpSpeed = 10.0f;
    public float gravityModifier;
    public bool gameOver = false;

    private bool isOnGround = true;
    private Rigidbody rb;
    private Animator playerAnim;
    private AudioSource playerAudioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && gameOver != true)
        {
            rb.AddForce(Vector3.up * jumpSpeed ,ForceMode.Impulse);
            playerAnim.SetTrigger("Jump_trig");
            isOnGround = false; 
            dirtyParticleSystem.Stop();
            playerAudioSource.PlayOneShot(jumpSound,1);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            dirtyParticleSystem.Play();
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionEffect.Play();
            dirtyParticleSystem.Stop();
            playerAudioSource.PlayOneShot(crashSound,1);
            Debug.Log("Game Over");
        }

    }
}
