using UnityEngine;
using UnityEngine.Experimental.AI;
using Photon.Pun;
public class MoverNetwork : MonoBehaviour
{
    [SerializeField]
    public Transform playerTransform;

    [SerializeField] float currentSpeed;

    [SerializeField]
    public float initialSpeed = 0.3f;

    [SerializeField]
    private float accelerationRate = 0.009f;

    [SerializeField]
    private float maxSpeed = 2f;

    [SerializeField]
    private float hitMovementDistance = 0.3f;

    [SerializeField]
    private PhotonView photonView;
    private Vector3 currentPosition;



    private Rigidbody2D rb;

    private Vector3 currentTarget;

    public float Dist() => Vector3.Distance(transform.position, playerTransform.position);

    public float DistFromCenter() => Mathf.Abs(Camera.main.transform.position.y - transform.position.y);

    private void OnDisable()
    {
        rb.velocity = new Vector3();
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPosition = transform.position;
        currentSpeed = initialSpeed;
        currentTarget = playerTransform.position;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;



        // Calculate direction from the ship to the player
        Vector3 direction = currentTarget - transform.position;

        // Normalize the direction vector
        direction.Normalize();

        // Move the ship in the direction with the current speed
        rb.velocity = direction * currentSpeed;

        // Gradually increase the speed up to the max speed
        currentSpeed += accelerationRate * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, initialSpeed, maxSpeed);
    }


    public void UpdateEnemyPos()
    {
        photonView.RPC(nameof(UpdateEnemyPosRPC), RpcTarget.All);
    }

    [PunRPC]
    private void UpdateEnemyPosRPC()
    {
        currentPosition = transform.position;
    }

    public void MoveUp(int viewId)
    {

        PhotonView otherPhotonView = PhotonView.Find(viewId);
        Vector3 direction = playerTransform.position - otherPhotonView.gameObject.transform.position;
        direction.Normalize();
        transform.position -= direction * hitMovementDistance;
    }




    public void SetSpeed(float speed) => currentSpeed = initialSpeed = speed;
}