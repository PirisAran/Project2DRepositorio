using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Transform[] _backgrounds;
    public float[] _parallaxScales;
    public float _smoothing = 1f;

    [SerializeField]private Camera _cam;
    private Vector3 _previousCamPos;

    void Awake()
    {
    }

    void Start()
    {
        _previousCamPos = _cam.transform.position;

        _parallaxScales = new float[_backgrounds.Length];
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _parallaxScales[i] = _backgrounds[i].position.z * -1;
        }
    }

    void Update()
    {
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            float parallax = (_previousCamPos.x - _cam.transform.position.x) * _parallaxScales[i];

            float backgroundTargetPosX = _backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, _backgrounds[i].position.y, _backgrounds[i].position.z);

            _backgrounds[i].position = Vector3.Lerp(_backgrounds[i].position, backgroundTargetPos, _smoothing * Time.deltaTime);
        }

        _previousCamPos = _cam.transform.position;
    }
}
