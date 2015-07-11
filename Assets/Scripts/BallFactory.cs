using UnityEditor;
using UnityEngine;

public class BallFactory : MonoBehaviour
{
    public Material[] BallMaterials;
    public PhysicMaterial[] BallPhysicMaterials;
    public Texture BogusTexture;
    public GameObject BallPrefab;
	
    public void Instantiate(BallInfo ballInfo)
    {
        Instantiate(BallPrefab, new Vector3(ballInfo.positionX, ballInfo.positionY, ballInfo.positionZ),
            Quaternion.identity);
    }

}