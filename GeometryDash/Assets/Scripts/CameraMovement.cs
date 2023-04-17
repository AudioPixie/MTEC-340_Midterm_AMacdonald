using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;
    public float xOffset;
    public float yOffset;
    private Quaternion rQ;

    public float rotationTime = 1.5f;

    private void Awake()
    {
        transform.position = new Vector3(0, 0, 0);
        rQ = transform.rotation;
    }

    void LateUpdate()
    {
        if (playerTransform.position.x >= -6 && (GameManager.Instance.State == "Level1" || GameManager.Instance.State == "Level2" || GameManager.Instance.State == "Level3"))
        {
            transform.position = new Vector3
            (playerTransform.position.x + xOffset,
            playerTransform.position.y + yOffset,
            transform.position.z);
        }
    }

    public IEnumerator RotateCamera()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 180, 0) * startRotation;
        float t = 0.0f;
        while (t < rotationTime)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / rotationTime);
            yield return null;
        }
        transform.rotation = endRotation;
    }

    public void Reset()
    {
        transform.position= new Vector3(0, 0, -10);
        transform.rotation = rQ;
    }
}
