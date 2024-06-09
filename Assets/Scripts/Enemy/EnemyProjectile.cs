using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyProjectile : MonoBehaviour
{
    float timer;
    float rotateSpeed = 600f;
    public float damage;
    public bool isRock;

    Vector2 dirPos;
    private void OnEnable()
    {
        timer = 0f;
        transform.Rotate(Vector3.zero);
        StartCoroutine(ActiveTrueTime());
    }
    private void Update()
    {
        if(isRock)
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        
    }
    public Vector2 projectileLookAt(Vector3 target)
    {
        dirPos = (target - transform.position).normalized;
        float angle = Mathf.Atan2(dirPos.y, dirPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        return dirPos;
    }
    IEnumerator ActiveTrueTime()
    {
        while (true)
        {
            timer += Time.deltaTime;

            if (timer > 4.5f)
            {
                gameObject.SetActive(false);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
