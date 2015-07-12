using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool IsBogus;

    private Rigidbody rb;

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
//        var manager = FindObjectOfType<GameManager>();
//        SetRigidbodyParameters(manager.BallMaterialOption);
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

    public BallInfo BallInfo()
    {
        return new BallInfo {isBogus = IsBogus, positionX = transform.position.x, positionY = transform.position.y, positionZ = transform.position.z};
    }
}