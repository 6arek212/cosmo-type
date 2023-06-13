using UnityEngine;

public class DestoryOnTrigger : MonoBehaviour
{
    [SerializeField] string targetTag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (targetTag != other.gameObject.tag)
            return;

        Destroy(gameObject);
    }
}
