using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public InputSystem_Actions inputActions;
    public float flapStrength;
    public LogicScript Logic;
    public bool isBirdAlive = true;
    public Animator wingsAnimator;
    public AudioSource flapAudio;
    public AudioSource deathAudio;
    private bool hasDied = false;
    public ScreenFlash screenFlash;

    private Collider2D birdCollider;

    void Start()
    {
        Logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        birdCollider = GetComponent<Collider2D>();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        // Smooth rotation based on falling speed
        float fallSpeed = myRigidBody.linearVelocity.y;
        float tiltAngle = Mathf.Lerp(0f, -90f, -fallSpeed / 10f);
        tiltAngle = Mathf.Clamp(tiltAngle, -90f, 30f);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, tiltAngle);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2f);

        if (inputActions.Player.Jump.WasPressedThisFrame() && isBirdAlive)
        {
            myRigidBody.linearVelocity = Vector2.up * flapStrength;
            transform.rotation = Quaternion.Euler(0, 0, 50f);
            flapAudio.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDied) return;

        hasDied = true;
        wingsAnimator.enabled = false;
        deathAudio.Play();

        if (birdCollider != null)
            birdCollider.enabled = false;

        StartCoroutine(FlashThenGameOver());
    }

    private IEnumerator FlashThenGameOver()
    {
        yield return StartCoroutine(screenFlash.FlashAndFreeze());
        Logic.gameOver();
        isBirdAlive = false;
    }
}
