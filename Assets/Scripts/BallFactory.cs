using UnityEditor;
using UnityEngine;

public class BallFactory : MonoBehaviour
{
    public GameManager Manager;
    public Material BogusMaterial;
    public GameObject BallPrefab;
    public Transform TablePivot;
	
    public void Instantiate(BallInfo ballInfo, int ballnumber)
    {
        var ball = (GameObject) PrefabUtility.InstantiatePrefab(BallPrefab);
        var pos = new Vector3(ballInfo.positionX, ballInfo.positionY, ballInfo.positionZ);
        var b = ball.GetComponent<Ball>();
        ball.transform.position = pos;

        ball.transform.SetParent(TablePivot);

        ball.GetComponent<SphereCollider>().material = Manager.BallPhysicMaterial;
        if (ballInfo.isBogus)
        {
            b.IsBogus = true;
            ball.GetComponent<MeshRenderer>().material = BogusMaterial;
        }
        else
        {
            b.SetNumber(ballnumber);

            if (b.ballNumber == 1)
            {
                b.SetBallAsTarget();
            }

            ball.GetComponent<MeshRenderer>().material = Manager.BallMaterial;
        }
    }

}