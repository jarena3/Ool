using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public GameManager Manager;

    public ScreenShake ScreenShaker;

    public ToggleGroup Toggle;
    
    public GameObject RayPoint;

    public Transform TablePivot;

    public GameObject Cue;

    public Text TimeElapsedText;
    public Text TotalScoreText;


    void Start()
    {
        Manager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        var toggleON = Toggle.ActiveToggles().First().gameObject.name;


        switch (toggleON)
        {
            case ("Lift Button"):
                LiftMode();
                break;
            case ("Gun Button"):
                GunMode();
                break;
        }

        TimeElapsedText.text = string.Format("{0}:{1}:{2}", Manager.GameStopwatch.Elapsed.Minutes, Manager.GameStopwatch.Elapsed.Seconds, Manager.GameStopwatch.Elapsed.Milliseconds);
        TotalScoreText.text = Manager.Score.ToString();
    }

    private void GunMode()
    {
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo) && hitInfo.transform.tag != "rayblock")
            {
                RayPoint.SetActive(true);
                RayPoint.transform.position = hitInfo.point;
            }
            else
            {
                RayPoint.SetActive(false);
            }

            if (Input.GetMouseButtonUp(0) && RayPoint.activeSelf)
            {
                var cue = (GameObject)PrefabUtility.InstantiatePrefab(Cue);
                cue.transform.position = ray.origin;
                cue.transform.parent = TablePivot;
                var rb = cue.GetComponent<Rigidbody>();
                rb.AddRelativeTorque(Random.onUnitSphere * 1);
                rb.AddForce(ray.direction * 150, ForceMode.Impulse);
                ScreenShaker.Shake(0.4f, 0.2f);
            }
        }
    }

    void LiftMode()
    {
        if (Input.GetMouseButton(0))
        {
            var camtrans = Camera.main.transform;
            TablePivot.Rotate(-camtrans.forward, Input.GetAxis("Mouse X"));
            TablePivot.Rotate(camtrans.right, Input.GetAxis("Mouse Y"));
        }

        else
        {
            StartCoroutine(DropTable());
        }

    }

    public void ResetTable()
    {
        StartCoroutine(DropTable());
    }

    IEnumerator DropTable()
    {
        while (TablePivot.rotation != Quaternion.identity)
        {
            TablePivot.rotation = Quaternion.Lerp(TablePivot.rotation, Quaternion.identity, Time.deltaTime * 20);
            TablePivot.position = Vector3.Lerp(TablePivot.position, Vector3.zero, Time.deltaTime * 20);
            yield return new WaitForFixedUpdate();
        }
 //       ScreenShaker.Shake(1, 0.1f);
    }

}
