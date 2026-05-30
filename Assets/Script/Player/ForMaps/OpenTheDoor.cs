using System.Collections;
using UnityEngine;

public class OpenTheDoor : MonoBehaviour
{
    public PlayerController player;
    private float distanceToPlayer;
    public GameObject closeDoor;
    public GameObject openDoorAnim;
    public Transform pointDoor;
    public Animator animator;
    public GameObject closeDoorAnimation;
    private bool dooropen = false;
    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = (player.transform.position - pointDoor.position).sqrMagnitude;
        if (distanceToPlayer < 4f) {
            closeDoor.SetActive(false);
            openDoorAnim.SetActive(true);
            dooropen = true;
            
        }
        if (distanceToPlayer > 6f && dooropen)
        {
            StartCoroutine(TimeToClosestDoor());
        }
    }
    private IEnumerator TimeToClosestDoor()
    {
        openDoorAnim.SetActive(false);
        dooropen = false;
        closeDoorAnimation.SetActive(true);
        yield return new WaitForSeconds(1f);
        closeDoorAnimation.SetActive(false);
        closeDoor.SetActive(true);
    }
}
