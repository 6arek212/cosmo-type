using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class TextTypeNetwork : MonoBehaviour
{
    [SerializeField]
    MeshText text;

    [SerializeField]
    string triggerTag;

    [SerializeField]
    GameObject explosion;

    [SerializeField]
    float explosionDestroyDelay = 0.75f;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private MoverNetwork shipMover;

    [SerializeField]
    private List<Word> words;

    [SerializeField]
    private AudioClip expSoundEffect;

    [SerializeField]
    private GameObject hit_effect;
    private Color[] colors = new Color[5];

    /*    [SerializeField] private explosion*/
    [SerializeField]
    private TargetsManagerNetWork targetsManager;

    [SerializeField]
    private PhotonView photonView;

    public bool isTaken { get; set; } = false;
    private int currentWordLength;
    private string fullText;
    private int health;
    ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;

    // Start is called before the first frame update
    void Start()
    {
        shipMover = GetComponent<MoverNetwork>();
        InitializeColors();
        targetsManager = GameObject
            .FindGameObjectWithTag("TargetsManager")
            .GetComponent<TargetsManagerNetWork>();
        UpdateTextTypeWords();
    }

    public void UpdateTextTypeWords()
    {
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
                new Word
                {
                    text = text.currentText,
                    isRTL = false,
                    lang = "en"
                }
            };
            SetWords(words);
        }
    }

    public void InitializeColors()
    {
        colors[0] = Color.red;
        colors[1] = Color.green;
        colors[2] = new Color(0.886f, 0.482f, 0.055f, 1.0f);
        colors[3] = Color.white;
        colors[4] = Color.gray;
    }

    public void SetWords(List<Word> words)
    {
        string json = JsonUtility.ToJson(new WordsData { Words = words });

        // Add an RPC call to synchronize the words across all players
        photonView.RPC("SetWordsRPC", RpcTarget.AllBuffered, json);
    }

    [Serializable]
    private class WordsData
    {
        public List<Word> Words;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != triggerTag)
            return;

        // if the bullet target is not this object then ignore
        TargetMover targetMover = other.GetComponent<TargetMover>();
        if (targetMover == null || targetMover.target != gameObject)
            return;

        OnHit(other.gameObject);
    }

    protected virtual void OnHit(GameObject other)
    {
        photonView.RPC("OnHitRPC", RpcTarget.All, other.GetComponent<PhotonView>().ViewID);
    }

    protected void ExplodeAndDestroy()
    {
        if (photonView.IsMine)
        {
            targetsManager.RemoveTarget(gameObject);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void RemoveFirstChar()
    {
        photonView.RPC("RemoveFirstCharRPC", RpcTarget.All);
    }

    public char FirstChar() => fullText.First();

    public void ChangeCurrentWordColor()
    {
        photonView.RPC("ChangeCurrentWordColorRPC", RpcTarget.All);
    }

    // this returns the current full text length
    public int FullTextLength
    {
        get { return fullText.Length; }
    }

    public int GetPlayerCharacter()
    {
        if (!playerProperties.ContainsKey("playerAvatar"))
            return 0;

        int avatarIndex = (int)playerProperties["playerAvatar"];
        return avatarIndex;
    }

    [PunRPC]
    public void RemoveFirstCharRPC()
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
            }
        }
    }

    [PunRPC]
    public void ChangeCurrentWordColorRPC()
    {
        int index = GetPlayerCharacter();
        Debug.Log(index);
        text.ChangeColor(colors[index]);
    }

    [PunRPC]
    protected virtual void OnHitRPC(int otherViewId)
    {
        PhotonView otherPhotonView = PhotonView.Find(otherViewId);
        if (otherPhotonView.Owner == PhotonNetwork.LocalPlayer)
        {
            PhotonNetwork.Destroy(otherPhotonView.gameObject);
        }
        health--;
        Instantiate(hit_effect, transform.position, Quaternion.identity);

        /*  shipMover.MoveUp(GetComponent<PhotonView>().ViewID);*/
        if (health == 0)
            ExplodeAndDestroy();
    }

    [PunRPC]
    private void SetWordsRPC(string json)
    {
        // Convert the JSON back to a list of words
        WordsData data = JsonUtility.FromJson<WordsData>(json);
        this.words = data.Words;
        text.UpdateText(this.words.First());
        fullText = string.Join("", this.words);
        currentWordLength = text.CountNoSpaces();
        health = fullText.Length;
    }
}
