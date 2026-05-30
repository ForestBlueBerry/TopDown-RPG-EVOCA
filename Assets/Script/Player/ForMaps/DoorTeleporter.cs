using UnityEngine;

public class DoorTeleporter : MonoBehaviour
{
    private BoxCollider2D boxCollider ;
    public Transform teleportpos ;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.transform.position = teleportpos.position;
        }
    }

}
