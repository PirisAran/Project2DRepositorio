using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathConditionTrap : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [SerializeField]
    Collider2D TerrainCollider;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform == Player.transform)
        {
            Debug.Log("killed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.gameObject.GetComponent<BoxCollider2D>())
        {
            Destroy(gameObject);
        }
    }
}
