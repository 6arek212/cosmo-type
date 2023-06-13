using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Word
{
    public bool isRTL;
    public string lang;
    public string text;

    public override string ToString()
    {
        return text.Replace(" ", "");
    }
}

public class TextType : MonoBehaviour
{
    [SerializeField] MeshText text;

    [SerializeField] string triggerTag;

    [SerializeField] GameObject explosion;

    [SerializeField] float explosionDestroyDelay = 0.75f;

    [SerializeField] TargetsManager targetsManager;

    [SerializeField] private List<Word> words;
    [SerializeField] private AudioClip expSoundEffect;
    [SerializeField] private GameObject hit_effect;


    private Mover shipMover;
    private int currentWordLength;
    private string fullText;
    private int health;

    // Start is called before the first frame update
    void Start()

    {

        shipMover = GetComponent<Mover>();

        targetsManager = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetsManager>();

        if (words.Count > 0)
        {
            // use words list object 
            SetWords(words);
        }
        else
        {
            // use the current word on the text object
            words = new List<Word>
            {
                new Word { text = text.currentText, isRTL = false , lang ="en" }
            };
            SetWords(words);
        }
    }

    public void SetWords(List<Word> words)
    {
        this.words = words;
        text.UpdateText(this.words.First());
        fullText = string.Join("", this.words);
        currentWordLength = text.CountNoSpaces();
        health = fullText.Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != triggerTag) return;

        // if the bullet target is not this object then ignore
        TargetMover targetMover = other.GetComponent<TargetMover>();
        if (targetMover == null || targetMover.target != gameObject) return;

        OnHit(other.gameObject);
    }



    protected virtual void OnHit(GameObject other)
    {
        // Destroy Bullet 
        Destroy(other);
        health--;
        Instantiate(hit_effect, transform.position, Quaternion.identity);
        shipMover.MoveUp();
        if (health == 0)
            ExplodeAndDestroy();
    }


    protected void ExplodeAndDestroy()
    {
        targetsManager.RemoveTarget(gameObject);
        // destroy this object
        Destroy(gameObject);

        // make an explosion
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(expSoundEffect, transform.position);
        Destroy(exp, explosionDestroyDelay);
    }


    public void RemoveFirstChar()
    {
        fullText = fullText.Remove(0, 1).Trim();
        text.RemoveFirstChar();
        currentWordLength--;

        if (currentWordLength == 0)
        {
            words.RemoveAt(0);

            if (words.Count > 0)
            {
                currentWordLength = words.First().text.Length;
                text.UpdateText(words.First());
                KeyboardLanguageChanger.ChangeKeyboardLanguage(words.First().lang);
            }
        }
    }

    public char FirstChar() => fullText.First();

    public void ChangeCurrentWordColor()
    {
        text.ChangeColor(Color.yellow);
        //text.ChangeFirstCharColor();
    }


    // this returns the current full text length
    public int FullTextLength { get { return fullText.Length; } }
}