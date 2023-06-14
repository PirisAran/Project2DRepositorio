using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Transform[] backgrounds;
    public float parallaxScale = 0.5f;
    public float parallaxSpeed = 0.2f;

    private Transform cam;
    private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;
    }

    void Update()
    {
        float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float backgroundTargetPosX = backgrounds[i].position.x + parallax * (i * parallaxSpeed + 1);
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
