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
    
    public GameObject CurrentIndicator;

    public List<GameObject> Indicators;
    public GameObject RayPoint;
    public GameObject Chalk;

    public Transform TablePivot;

    private bool IsTableLifting;

    public GameObject Brick;
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
            case ("Brick Button"):
                BrickMode();
                break;
        }

//        TimeElapsedText.text = string.Format("{0}:{1}:{2}", Manager.GameStopwatch.Elapsed.Minutes, Manager.GameStopwatch.Elapsed.Seconds, Manager.GameStopwatch.Elapsed.Milliseconds);
        TotalScoreText.text = Manager.Score.ToString();
    }

    private void BrickMode()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
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
            var brick = (GameObject)PrefabUtility.InstantiatePrefab(Brick);
            brick.transform.position = RayPoint.transform.position + new Vector3(0, 10, 0);
            brick.transform.parent = TablePivot;
            brick.GetComponent<Rigidbody>().AddRelativeTorque(Random.onUnitSphere * 10);
        }
    }

    private void GunMode()
    {
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
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
            IsTableLifting = true;
            TablePivot.Rotate(Vector3.back, Input.GetAxis("Mouse X"));
            TablePivot.Rotate(Vector3.right, Input.GetAxis("Mouse Y"));
        }

        else if (Input.GetMouseButtonUp(0) && IsTableLifting)
        {
            StartCoroutine(DropTable());
            IsTableLifting = false;
        }

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
