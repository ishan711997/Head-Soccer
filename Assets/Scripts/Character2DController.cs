using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    private float horizontalAxis = 0.0f;

    public float _velocity;

    private Rigidbody2D _rigidbody;

    public float _jumpForce;

    private bool canShoot;

    private GameObject _ball;

    public Transform checkGround;

    private bool grounded;

    public LayerMask ground_layers;

    public GameObject[] cryObjs;

    private Animator _animator;

    private int
        cryHash,
        celebrateHash;

    public Animator Anim { get { return _animator; } }

    public int CryHash { get { return cryHash; } }
    public int CelebrateHash { get { return celebrateHash; } }

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        canShoot = false;
        _ball = GameObject.FindGameObjectWithTag("Ball");
        Application.targetFrameRate = 60;
        grounded = false;

        cryHash = Animator.StringToHash("Cry");
        celebrateHash = Animator.StringToHash("Celebrate");
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(horizontalAxis * Time.deltaTime * _velocity, _rigidbody.velocity.y);

        grounded = Physics2D.OverlapCircle(checkGround.position, 0.2f, ground_layers);
        //Debug.Log ("ground : " + grounded);
    }

    public void Move(int value)
    {
        if (!GameController.Instance.Scored && !GameController.Instance.EndMatch)
        {
            horizontalAxis = value;
        }
    }

    public void StopMove()
    {
        horizontalAxis = 0.0f;
    }

    public void Jump()
    {
        if (!GameController.Instance.Scored && !GameController.Instance.EndMatch)
        {
            if (grounded)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            }
        }

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.jump.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {

            canShoot = true;
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {

            canShoot = false;
        }

    }

    public void Shoot()
    {
        if (!GameController.Instance.Scored && !GameController.Instance.EndMatch)
        {
            if (canShoot)
            {
                _animator.SetTrigger("Shoot");
                _ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500.0f, 300.0f));
            }
        }

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.ballKick.Play();
        }
    }

    void OnDisable()
    {
        StopCry();
    }

    public void StopCry()
    {
        var length = cryObjs.Length - 1;
        for (var i = length; i >= 0; i--)
        {
            cryObjs[i].SetActive(false);
        }
    }
}
