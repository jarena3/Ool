using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    public bool IsBogus;

    private Rigidbody rb;
    private GameManager Manager;

    public BallInfo BallInfo()
    {
        return new BallInfo { isBogus = IsBogus, positionX = transform.position.x, positionY = transform.position.y, positionZ = transform.position.z };
    }

    public void Init(bool bogosity, Texture texture)
    {
        var manager = GameObject.Find("Manager").GetComponent<GameManager>();
        IsBogus = bogosity;
        var r = GetComponent<Renderer>();
        r.material = manager.BallMaterial;
        r.material.SetTexture(0, texture);
        SetRigidbodyParameters(manager.BallMaterialOption);
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        Manager = FindObjectOfType<GameManager>();
        SetRigidbodyParameters(Manager.BallMaterialOption);
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
            Sunk();
        }
        else if (col.gameObject.name == "OOBDetector")
        {
            Foul();
        }
    }

    private void Sunk()
    {
        Manager.Score += 100*Manager.ScoreMultiplier;
        Manager.CheckLevelOver();
        Destroy(gameObject);
    }

    private void Foul()
    {
        Manager.Score -= 1000;
        Manager.CheckLevelOver();
        Destroy(gameObject);
    }
}