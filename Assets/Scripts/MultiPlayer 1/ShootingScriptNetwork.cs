using Assets.Scripts;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.IO;

using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    private GameObject shoot_effect;

    [SerializeField]
    private AudioClip shootSoundEffect;

    [SerializeField]
    private AudioClip expSoundEffect;

    [SerializeField]
    private AudioClip missSoundEffect;


    private TargetsManagerNetWork targetsManager;
    private StatsManagerNetwork statsManager;
    private GameObject currentTarget;


    private void Start()
    {
        targetsManager = GameObject
            .FindGameObjectWithTag("TargetsManager")
            .GetComponent<TargetsManagerNetWork>();
        statsManager = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<StatsManagerNetwork>();
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
            statsManager.IncreaseCorrectCharactersTyped();
        }
        else
        {
            playMissSound();
        }
        statsManager.IncreaseCharactersTyped();
        GameStats stats = statsManager.GetStats();
   
        UpdateStats(stats.accurecy);

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

    public void UpdateStats(string accurecy)
    {
        photonView.RPC(nameof(UpdateStatsRPC), photonView.Owner,accurecy);
    }


    [PunRPC]
    private void UpdateStatsRPC(string accurecy)
    {
        Hashtable hash = new Hashtable();
        hash.Add("accurecy", accurecy);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
      

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
