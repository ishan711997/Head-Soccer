using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private bool resetPos;

    public float speedMove;

    // Behaviour messages
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Move
        transform.position -= new Vector3(speedMove * Time.deltaTime, 0.0f, 0.0f);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (!GeometryUtility.TestPlanesAABB(planes, spriteRenderer.bounds))
        {
            spriteRenderer.enabled = false;

            if (!resetPos)
            {
                resetPos = true;
                transform.position = new Vector3(10.5f, transform.position.y, 0.0f);
            }
        }
        else
        {
            spriteRenderer.enabled = true;

            if (resetPos)
            {
                resetPos = false;
            }
        }
    }
}
