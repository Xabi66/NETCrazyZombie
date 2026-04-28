using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    void Start()
    {
        Invoke("DestroyAfterTime", lifeTime);
    }

    private void DestroyAfterTime()
    {
        Destroy(gameObject);
    }
}
