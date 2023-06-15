using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    static CursorController _cursorController;
    GameObject _particlesPrefab;
    static GameObject _particlesInstantiated;

    public static CursorController GetCursorController()
    {
        return _cursorController;
    }

    public static void InitCursor()
    {
        GameObject l_GameObject = new GameObject("Cursor");
        l_GameObject.transform.position = Vector3.zero;
        CursorController.DontDestroyOnLoad(l_GameObject);
        l_GameObject.AddComponent<CursorController>();

        if (_cursorController != null)
        {
            Destroy(_cursorController.gameObject);
        }
        _cursorController = l_GameObject.AddComponent<CursorController>();
    }

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Start()
    {
        if (_particlesInstantiated != null)
        {
            Destroy(_particlesInstantiated);
        }
        _particlesPrefab = Resources.Load<GameObject>("CursorParticles");
        _particlesInstantiated = Instantiate(_particlesPrefab);
        CursorController.DontDestroyOnLoad(_particlesInstantiated);
    }


    private void FixedUpdate()
    {
        Vector2 cursorPos = new Vector2();
        cursorPos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        cursorPos.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        _particlesInstantiated.transform.position = cursorPos;
    }
}
