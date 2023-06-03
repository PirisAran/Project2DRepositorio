using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;


public class Destroyer : MonoBehaviour, IRestartLevelElement
{
    private Vector3 _oPosition;
    private Quaternion _oRotation;

    private void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    private void Awake()
    {
        _oPosition = transform.position;
        _oRotation = transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyableObject obj = collision.gameObject.GetComponent<DestroyableObject>();

        if (obj != null)
        {
            CallDestroyObject(obj);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyableObject obj = collision.gameObject.GetComponent<DestroyableObject>();

        if (obj != null)
        {
            CallDestroyObject(obj);
        }
    }
    private void CallDestroyObject(DestroyableObject obj)
    {
        obj.StartDestroyObject();
    }

    public void RestartLevel()
    {
        transform.position = _oPosition;
        transform.rotation = _oRotation;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

}
