using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{

    private GameObject _ball;

    private Rigidbody2D _rigidbody;

    public float _velocity;

    public float rangeOfDefense;

    public Transform defensePos;

    private bool canShoot;

    public Transform checkGround, checkHead;

    public bool grounded, canHead;

    public LayerMask ground_layers, ball_layer;

    public float _jumpForce;

    public Animator anim;

    public GameObject[] cryObjs;

    public int CryHash { get; set; }
    public int CelebrateHash { get; set; }

    // Use this for initialization
    void Start()
    {
        CryHash = Animator.StringToHash("Cry");
        CelebrateHash = Animator.StringToHash("Celebrate");

        _ball = GameObject.FindGameObjectWithTag("Ball");
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.Instance.Scored && !GameController.Instance.EndMatch)
        {

            //Debug.Log ("RANGE + " + Mathf.Abs(_ball.transform.position.x - transform.position.x));
            if (Mathf.Abs(_ball.transform.position.x - transform.position.x) <= rangeOfDefense)
            {
                //StopAllCoroutines ();
                float _directionMove = (_ball.transform.position.x > transform.position.x) ? 1.0f : -1.0f;
                _rigidbody.velocity = new Vector2(_velocity * _directionMove, _rigidbody.velocity.y);
                //Attack();
            }
            else if (transform.position.x <= defensePos.position.x)
                _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            else
                TurnBack();

            if (canShoot)
                Shoot();

            if (canHead && grounded)
                Jump();

        }

    }

    void FixedUpdate()
    {

        grounded = Physics2D.OverlapCircle(checkGround.position, 0.2f, ground_layers);
        canHead = Physics2D.OverlapCircle(checkHead.position, 1.0f, ball_layer);
        //Debug.Log ("ground : " + grounded);
    }

    void TurnBack()
    {
        //StartCoroutine (MoveObject (transform, transform.position, defensePos.position, 2.0f));
        _rigidbody.velocity = new Vector2(-_velocity, _rigidbody.velocity.y);
    }

    void Attack()
    {
        StartCoroutine(MoveObject(transform, transform.position, new Vector3(_ball.transform.position.x, transform.position.y, 0.0f), 2.0f));
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
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

    void Shoot()
    {
        canShoot = false;
        _ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(750.0f, 300.0f));

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.ballKick.Play();
        }
    }

    void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);

        if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
        {
            SoundManager.Instance.jump.Play();
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
