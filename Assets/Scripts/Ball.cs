using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public GameObject _effect;

    public Rigidbody2D rigid2D;

    // Use this for initialization
    void Start()
    {
        rigid2D.AddForce(transform.up * 400.0f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log ("OK" + col.gameObject.name);
        if (col.gameObject.tag == "Head")
        {
            //Debug.Log ("OK");
            if (!GameController.Instance.Scored)
            {
                Instantiate(_effect, transform.position, transform.rotation);

                if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
                {
                    SoundManager.Instance.ballHit.Play();
                }
            }
        }

        if (col.gameObject.tag == "UpCol")
        {
            if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
            {
                SoundManager.Instance.crossBarHit.Play();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!GameController.Instance.Scored)
        {
            if (col.tag == "RightNet")
            {
                GameController.Instance.ScoredAgainst(false);

                if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
                {
                    SoundManager.Instance.goal.Play();
                }
            }
            if (col.tag == "LeftNet")
            {
                GameController.Instance.ScoredAgainst(true);

                if (PlayerPrefs.GetInt(GameConstants.SOUND, 1) == 1)
                {
                    SoundManager.Instance.goal.Play();
                }
            }
        }
    }
}
