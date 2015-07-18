using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public bool IsBogus;
    public bool BallIsTarget;
    
    public int ballNumber;

    private Rigidbody rb;
    private GameManager Manager;

    public GameObject canvasPrefab;
    public GameObject canvas;

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
                rb.mass = 20;
                rb.angularDrag = 40;
                break;
            case LevelOptions.MaterialOptions.Sandpaper:
                rb.mass = 1;
                rb.angularDrag = 15;
                break;
            case LevelOptions.MaterialOptions.Wood:
                rb.mass = 2;
                rb.angularDrag = 5;
                break;
            case LevelOptions.MaterialOptions.Normal:
                rb.mass = 3;
                rb.angularDrag = 2;
                break;
            case LevelOptions.MaterialOptions.Metal:
                rb.mass = 3;
                rb.angularDrag = 1;
                break;
            case LevelOptions.MaterialOptions.Ice:
                rb.mass = 1;
                rb.angularDrag = 0.01f;
                break;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "PocketDetector")
        {
            if (ballNumber == Manager.activeBallNumber)
            {
                Sunk();
            }
            else
            {
                Foul();
            }
        }
        else if (col.gameObject.name == "OOBDetector")
        {
            Foul();
        }
    }

    private void Sunk()
    {
        Manager.Score += 100*Manager.ScoreMultiplier;
        Destroy(gameObject);
    }

    private void Foul()
    {
        Manager.Score -= 1000;
        Destroy(gameObject);
    }

    public void SetNumber(int number)
    {
        ballNumber = number;
        //TODO: set texture
    }

    public void SetBallAsTarget()
    {
        BallIsTarget = true;
        canvas.SetActive(true);
    }

    void Update()
    {
        canvas.transform.position = transform.position + Vector3.up;
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