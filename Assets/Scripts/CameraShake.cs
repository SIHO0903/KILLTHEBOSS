using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    static Quaternion originRot;
    static Vector3 offset = Vector3.one/2f;
    static float force = 300f;
    static float shakeTimer;

    private void Start()
    {
        originRot = transform.rotation;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            StartCoroutine(ShakeCoroutine(3f));
        else if (Input.GetKeyDown(KeyCode.U))
        {
            StopAllCoroutines();
            StartCoroutine(ResetCameraRot());
        }
    }
    public static IEnumerator ShakeCoroutine(float timer)
    {
        Vector3 originEuler = Camera.main.transform.eulerAngles;
        shakeTimer = 0;
        while (shakeTimer < timer)
        {
            shakeTimer += Time.deltaTime;
            float rotX = Random.Range(-offset.x, offset.x);
            float rotY = Random.Range(-offset.y, offset.y);
            float rotZ = Random.Range(-offset.z, offset.z);

            Vector3 randomRot = originEuler + new Vector3(rotX, rotY, rotZ);
            Quaternion rot = Quaternion.Euler(randomRot);

            while (Quaternion.Angle(Camera.main.transform.rotation, rot) > 0.1f)
            {
                Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, rot, force * Time.deltaTime);
                yield return null;
            }
            yield return ResetCameraRot();
        }
    }
    static IEnumerator ResetCameraRot()
    {
        while (Quaternion.Angle(Camera.main.transform.rotation, originRot) > 0f)
        {
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation,originRot, force * Time.deltaTime);
            yield return null;
        }
    }
}
