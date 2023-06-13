using UnityEngine;
using Photon.Pun;
// This Script enable object to move toward a targetpoint

public class TargetMover : MonoBehaviour
{
    [Tooltip("Move an object towards a point")]
    [SerializeField] public GameObject target {  get; private set; }

    [SerializeField] private float speed = 10f;
    [SerializeField] private PhotonView photonView;

    void Update()
    {
        if (target == null)
        {
            return;
        }

        // update position
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);

        // update rotation

        // Calculate the direction to the target
        Vector3 direction = target.transform.position - transform.position;
        direction.z = 0f; // Set the Z-coordinate to zero

        // Rotate the object to face the target
        if (direction != Vector3.zero)
        {
            transform.up = direction.normalized;
        }
    }



    public void setTarget(GameObject target) => this.target = target;
}
