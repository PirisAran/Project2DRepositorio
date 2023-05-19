using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePrefabBehaviour : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterTime(_lifeTime));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
