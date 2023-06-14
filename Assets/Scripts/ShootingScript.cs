using Assets.Scripts;
using System.Linq;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

// This Script represents the enemy shooting.
public class ShootingScript : MonoBehaviour
{
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

    private GameObject currentTarget;

    private TargetsManager targetsManager;

    private StatsManager statsManager;

    private LangKey currentLangKey = LangKey.ENGLISH;



    void Start()
    {
        targetsManager = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetsManager>();
        statsManager = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<StatsManager>();
    }


    void Update()
    {
        string pressedKey = Input.inputString; //get the pressed key
        if (string.IsNullOrEmpty(pressedKey) || targetsManager.Count == 0)
            return;

        pressedKey = pressedKey.First().Translate(currentLangKey).ToString();


        // Find a new target if we dont have one already
        if (currentTarget == null && LockOnTarget(pressedKey) == null)
            return;

        // Check the current taget text length, if its empty then find another target
        TextType textType = currentTarget.GetComponent<TextType>();
        if (textType.FullTextLength == 0 && LockOnTarget(pressedKey) == null)
            return;

        RotateWeapon();

        // Check if the key pressed char is equal to the words first char
        textType = currentTarget.GetComponent<TextType>();
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

        // if we finished the current word then change to defualt keyboard
        if (textType.FullTextLength == 0)
            currentLangKey = LangKey.ENGLISH;
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
    private GameObject Shoot()
    {
        StartShootEffect();

        Vector3 positionOfSpawnedObject = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z
        );

        // create new bullet
        GameObject newBullet = Instantiate(
            BulletObject,
            positionOfSpawnedObject,
            Quaternion.identity
        );

        // set the target for the bullet
        TargetMover newObjectMover = newBullet.GetComponent<TargetMover>();
        if (newObjectMover)
            newObjectMover.setTarget(currentTarget);

        // play sound
        playShootSound();

        return newBullet;
    }

    private void playShootSound()
    {
        AudioSource.PlayClipAtPoint(shootSoundEffect, transform.position);
    }

    private void playMissSound()
    {
        AudioSource.PlayClipAtPoint(missSoundEffect, transform.position);
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

    // sets the current keyboard language
    public void SetLanguage(LangKey langKey) => currentLangKey = langKey;
    public void SetLanguage(string langKey)
    {
        if (langKey == "he")
        {
            currentLangKey = LangKey.HEBREW;
            return;
        }
        currentLangKey = LangKey.ENGLISH;
    }

}
