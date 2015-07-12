using UnityEditor;
using UnityEngine;

public class BallFactory : MonoBehaviour
{
    public GameManager Manager;
    public Material BogusMaterial;
    public GameObject BallPrefab;
	
    public void Instantiate(BallInfo ballInfo)
    {
        var ball = (GameObject) PrefabUtility.InstantiatePrefab(BallPrefab);
        var pos = new Vector3(ballInfo.positionX, ballInfo.positionY, ballInfo.positionZ);
        ball.transform.position = pos;
        ball.GetComponent<SphereCollider>().material = Manager.BallPhysicMaterial;
        if (ballInfo.isBogus)
        {
            ball.GetComponent<Ball>().IsBogus = true;
            ball.GetComponent<MeshRenderer>().material = BogusMaterial;
        }
        else
        {
            ball.GetComponent<MeshRenderer>().material = Manager.BallMaterial;
        }
    }

}