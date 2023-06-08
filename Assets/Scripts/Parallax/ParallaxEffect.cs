using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float parallaxMultiplayer;

    float _startPosition;
    Transform _cameraTransform;

    void Start()
    {
        _startPosition = transform.position.x;
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        ParallaxEfect();
    }

    private void ParallaxEfect()
    {
        float parallaxOffset = (_cameraTransform.position.x - _startPosition) * parallaxMultiplayer;
        Vector2 newPosition = new Vector2(_startPosition + parallaxOffset, transform.position.y);
        transform.position = newPosition;
    }
}
