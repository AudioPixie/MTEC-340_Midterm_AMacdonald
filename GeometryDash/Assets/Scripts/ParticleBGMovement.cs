using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBGMovement : MonoBehaviour
{
    public Transform playerTransform;

    private void Awake()
    {
        transform.position = new Vector3(0, -0.5f, 0);
    }

    void LateUpdate()
    {
        if (playerTransform.position.x >= -6 && (GameManager.Instance.State == "Level1" || GameManager.Instance.State == "Level2" || GameManager.Instance.State == "Level3"))
        {
            transform.position = new Vector3
            (playerTransform.position.x + 6, transform.position.y, transform.position.z);
        }

        if (GameManager.Instance.State == "Level1" || GameManager.Instance.State == "Level2" || GameManager.Instance.State == "Level3")
            GetComponent<ParticleSystem>().Play();
        else
            GetComponent<ParticleSystem>().Stop();
    }

    public void Reset()
    {
        transform.position = new Vector3(0, -0.5f, 0);
    }
}
