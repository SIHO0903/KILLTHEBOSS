using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAOE : MonoBehaviour
{
    public float damage;
    Color color;
    SpriteRenderer sprite;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        color = sprite.color;
        color.a = 0.5f;
        StartCoroutine(SetFalseTimer());
    }
    IEnumerator SetFalseTimer()
    {
        while (true)
        {
            color.a -= Time.deltaTime;
            sprite.color = color;
            if (color.a < 0)
            {
                gameObject.SetActive(false);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
