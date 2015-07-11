using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool IsBogus;

    public void Init(bool bogosity, Texture texture)
    {
        var manager = GameObject.Find("Manager").GetComponent<GameManager>();
        IsBogus = bogosity;
        var r = GetComponent<Renderer>();
        r.material = manager.BallMaterial;
        r.material.SetTexture(0, texture);

    }

    public BallInfo BallInfo()
    {
        return new BallInfo {isBogus = IsBogus, positionX = transform.position.x, positionY = transform.position.y, positionZ = transform.position.z};
    }
}