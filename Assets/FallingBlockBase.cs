using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlockBase : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RoomCamManager.GetCameraManager().ShakeOnce();
        collision.rigidbody.bodyType = RigidbodyType2D.Static;
    }
}
