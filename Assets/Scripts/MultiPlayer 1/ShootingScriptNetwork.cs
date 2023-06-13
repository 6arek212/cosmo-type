using Assets.Scripts;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.IO;

using Photon.Pun;

// This Script represents the enemy shooting.
public class ShootingScriptNetwork : MonoBehaviour
{
    [SerializeField]
    PhotonView photonView;

    [SerializeField]
    private GameObject BulletObject;

    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private TargetsManagerNetWork targetsManager;

    [SerializeField]
    private GameObject shoot_effect;

    [SerializeField]
    private AudioClip shootSoundEffect;

    [SerializeField]
    private AudioClip expSoundEffect;

    [SerializeField]
    private AudioClip missSoundEffect;

    /*    [SerializeField] private PlayerController playerController;*/

    private GameObject currentTarget;

    private void Start()
    {
        targetsManager = GameObject
            .FindGameObjectWithTag("TargetsManager")
            .GetComponent<TargetsManagerNetWork>();
    }

    void Update()
    {
        //if not the player of the machine we using.
        if (!photonView.IsMine)
            return;

        string pressedKey = Input.inputString; //get the pressed key
        if (string.IsNullOrEmpty(pressedKey) || targetsManager.Count == 0)
            return;
        // Find a new target if we dont have one already
        if (currentTarget == null && LockOnTarget(pressedKey) == null)
            return;
        // Check the current taget text length, if its empty then find another target
        TextTypeNetwork textType = currentTarget.GetComponent<TextTypeNetwork>();
        if (textType.FullTextLength == 0 && LockOnTarget(pressedKey) == null)
            return;

        RotateWeapon();

        // Check if the key pressed char is equal to the words first char
        textType = currentTarget.GetComponent<TextTypeNetwork>();
        if (textType.FullTextLength > 0 && textType.FirstChar() == pressedKey.First())
        {
            textType.RemoveFirstChar();
            textType.ChangeCurrentWordColor();
            Shoot();
        }
        else
        {
            playMissSound();
        }

        // if we finished the current word then change to defualt keyboard
        if (textType.FullTextLength == 0)
            KeyboardLanguageChanger.ChangeKeyboardLanguage();
    }

    GameObject LockOnTarget(string pressedKey)
    {
        return currentTarget = targetsManager.FindTarget(gameObject, pressedKey.First());
    }

    // Rotate the Weapon at the target direction.
    private void RotateWeapon()
    {
        Vector3 turretPosition = transform.position;
        Vector2 direction = currentTarget.transform.position - turretPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
        transform.rotation = targetRotation;
    }

    //spawn bullet objects and set a target.
    private void Shoot()
    {
        photonView.RPC("ShootRPC", RpcTarget.All);
    }

    [PunRPC]
    private void ShootRPC()
    {
        if (!photonView.IsMine)
            return;
        StartShootEffect();
        Vector3 positionOfSpawnedObject = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z
        );

        GameObject newBullet = PhotonNetwork.Instantiate(
            Path.Combine("PhotonPrefabs", BulletObject.name),
            positionOfSpawnedObject,
            Quaternion.identity
        );

        // set the target for the bullet
        TargetMover newObjectMover = newBullet.GetComponent<TargetMover>();
        if (newObjectMover)
            newObjectMover.setTarget(currentTarget);
        playShootSound();
    }

    private void StartShootEffect()
    {
        GameObject obj = (GameObject)Instantiate(
            shoot_effect,
            transform.position - new Vector3(0, 0, 5),
            Quaternion.identity
        ); //Spawn muzzle flash
        obj.transform.parent = transform;
    }

    private void playShootSound()
    {
        AudioSource.PlayClipAtPoint(shootSoundEffect, transform.position);
    }

    private void playMissSound()
    {
        AudioSource.PlayClipAtPoint(missSoundEffect, transform.position);
    }
}
