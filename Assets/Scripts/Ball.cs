using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{

    public float DragCoefficient;

    public bool IsBogus;
    public bool BallIsTarget;
    
    public int ballNumber;

    private Rigidbody rb;
    private GameManager Manager;

    public GameObject canvasPrefab;
    public GameObject canvas;

    public GameworldUIManager gwui;

    public AudioSource aSource;

    public BallInfo BallInfo()
    {
        return new BallInfo { isBogus = IsBogus, positionX = transform.position.x, positionY = transform.position.y, positionZ = transform.position.z };
    }

    public void Awake()
    {
        if (!IsBogus)
        {
            canvas = Instantiate(canvasPrefab);
        }
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        aSource = GetComponent<AudioSource>();
        gwui = FindObjectOfType<GameworldUIManager>();
        Manager = FindObjectOfType<GameManager>();
        SetRigidbodyParameters(Manager.BallMaterialOption);
        canvas.GetComponentInChildren<Text>().text = ballNumber.ToString();
        canvas.SetActive(false);

    }

    private void SetRigidbodyParameters(LevelOptions.MaterialOptions ballMaterialOption)
    {
        switch (ballMaterialOption)
        {
            case LevelOptions.MaterialOptions.Lead:
                rb.mass = 40;
                rb.angularDrag = 25;
                rb.drag = 10;
                aSource.clip = Manager.BallClip[0];
                break;
            case LevelOptions.MaterialOptions.Sandpaper:
                rb.mass = 1;
                rb.angularDrag = 15f;
                rb.drag = 5;
                aSource.clip = Manager.BallClip[1];
                break;
            case LevelOptions.MaterialOptions.Wood:
                rb.mass = 2;
                rb.angularDrag = 10;
                rb.drag = 3;
                aSource.clip = Manager.BallClip[2];
                break;
            case LevelOptions.MaterialOptions.Normal:
                rb.mass = 3;
                rb.angularDrag = 1.5f;
                rb.drag = 0.5f;
                aSource.clip = Manager.BallClip[3];
                break;
            case LevelOptions.MaterialOptions.Metal:
                rb.mass = 3;
                rb.angularDrag = 0.1f;
                rb.drag = 0.1f;
                aSource.clip = Manager.BallClip[0];
                break;
            case LevelOptions.MaterialOptions.Ice:
                rb.mass = 1;
                rb.angularDrag = 0.001f;
                rb.drag = 0.001f;
                aSource.clip = Manager.BallClip[4];
                break;
        }
    }
    /*
    void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity *= -DragCoefficient*rb.mass*Time.deltaTime;
        }
    }
    */
    void OnCollisionEnter(Collision col)
    {
        switch (col.gameObject.tag)
        {
            case "Ball":
                PlaySound();
                break;
            case "Rail":
                Debug.Log("hit!");
                RaycastHit hitinfo;
                Physics.Raycast(transform.position, rb.velocity, out hitinfo);
                rb.velocity = Vector3.Reflect(rb.velocity, hitinfo.normal) * 2;
                break;
            case "Pocket":
                if (IsBogus)
                {
                    Foul("Bogus ball pocketed!", -1000);
                }
                if (ballNumber == Manager.activeBallNumber &&!IsBogus)
                {
                    Sunk(col.gameObject.name);
                }
                else if (!IsBogus)
                {
                    Foul("Out of turn!", -500);
                }
                break;
            default:
                if (col.gameObject.tag == "OOB" && !IsBogus)
                {
                    Foul("Out of bounds!", -500);
                }
                break;
        }
    }

    void PlaySound()
    {
        if (Manager.PlaySound)
        {
            aSource.Play();  
        }
    }


    private void Sunk(string pocketName)
    {
        gwui.PlaySound(Manager.GoodSinkClip);
        Manager.Score += 100 * Manager.ScoreMultiplier;
        gwui.SunkBall(string.Format("{0} Ball, {1} : {2} points", ballNumber, pocketName, 100*Manager.ScoreMultiplier));
        Destroy(gameObject);
    }

    private void Foul(string reason, int penalty)
    {
        gwui.PlaySound(Manager.BadSinkClip);
        Manager.Score += penalty;
        Manager.CurrentFaults ++;
        gwui.FoulBall(string.Format("Foul: {0} : {1} point penalty", reason, penalty));
        Destroy(gameObject);
    }

    public void SetNumber(int number)
    {
        ballNumber = number;
    }

    public void SetBallAsTarget()
    {
        BallIsTarget = true;
        canvas.SetActive(true);
    }

    void Update()
    {
        canvas.transform.position = transform.position + Vector3.up;
        canvas.transform.LookAt(Camera.main.transform);
        canvas.SetActive(BallIsTarget);
    }

    void OnDestroy()
    {
        if (BallIsTarget)
        {
            Manager.ActivateNextBall();
        }
        Destroy(canvas);
    }

}