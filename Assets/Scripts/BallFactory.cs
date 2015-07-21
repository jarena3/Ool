using UnityEditor;
using UnityEngine;

public class BallFactory : MonoBehaviour
{
    public GameManager Manager;
    public Material BogusMaterial;
    public GameObject BallPrefab;
    public Transform TablePivot;

    public Texture[] BallNumberTextures;
	
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
            ball.GetComponent<MeshRenderer>().material = Manager.BallMaterial;

            var textureNumber = ballnumber;
            while (textureNumber > 15) textureNumber -= 15;

            var renderer = ball.GetComponent<Renderer>();
            var mat = renderer.material;
            mat.EnableKeyword("_DETAIL_MULX2");
            mat.SetTexture("_DetailAlbedoMap", BallNumberTextures[textureNumber - 1]);

            renderer.material = mat;

            if (b.ballNumber == 1)
            {
                b.SetBallAsTarget();
            }

        }
    }

}