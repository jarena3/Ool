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
            case ("Punch Button"):
                PunchMode();
                break;
            case ("Brick Button"):
                BrickMode();
                break;
        }

        TimeElapsedText.text = Manager.GameStopwatch.Elapsed.ToString();
        TotalScoreText.text = Manager.Score.ToString();
    }

    private void BrickMode()
    {
        DisableIndicators();
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

    private void PunchMode()
    {
        DisableIndicators();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo) && hitInfo.transform.gameObject.name == "Frame")
        {
            RayPoint.SetActive(true);
            RayPoint.transform.position = hitInfo.point;
        }
        else
        {
            RayPoint.SetActive(false);
        }

        if (Input.GetMouseButton(0) && RayPoint.activeSelf)
        {
            RayPoint.transform.localScale = Vector3.Lerp(RayPoint.transform.localScale, new Vector3(5, 5, 5),
                Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(0) && RayPoint.activeSelf)
        {
            var balls = GameObject.FindGameObjectsWithTag("Ball");
            var hitangle = (hitInfo.point - new Vector3(0, 0, 10)).normalized;
            foreach (var ball in balls)
            {
                ball.GetComponent<Rigidbody>().AddForce(-hitangle * RayPoint.transform.localScale.x * 250);
            }
            Chalk.GetComponent<Rigidbody>().AddForce(-hitInfo.normal * RayPoint.transform.localScale.x * 100);
            ScreenShaker.Shake(RayPoint.transform.localScale.x/40, 0.08f);
            RayPoint.transform.localScale = Vector3.one;
        }
    }

    void LiftMode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo) && !IsTableLifting)
        {
            var indicatorDistances = from element in Indicators
                                     orderby Vector3.Distance(hitInfo.point, element.transform.position)
                                     select element;

            DisableIndicators();

            var cInd = indicatorDistances.First();
            cInd.SetActive(true);
            CurrentIndicator = cInd;
        }
        if (Input.GetMouseButton(0) && CurrentIndicator != null)
        {
            IsTableLifting = true;
            var move = Input.GetAxis("Mouse Y")/2;
            switch (CurrentIndicator.name)
            {
                case("Front"):
                    TablePivot.Rotate(Vector3.right, move);
                    break;
                case("Back"):
                    TablePivot.Rotate(Vector3.left, move);
                    break;
                case ("Right"):
                    TablePivot.Rotate(Vector3.forward, move);
                    break;
                case ("Left"):
                    TablePivot.Rotate(Vector3.back, move);
                    break;
            }
            TablePivot.Translate(0,move/5,0);
        }

        if (Input.GetMouseButtonUp(0) && IsTableLifting)
        {
            StartCoroutine(DropTable());
            IsTableLifting = false;
        }
    }

    IEnumerator DropTable()
    {
        while (TablePivot.rotation != Quaternion.identity)
        {
            TablePivot.rotation = Quaternion.Slerp(TablePivot.rotation, Quaternion.identity, Time.deltaTime * 5);
            TablePivot.position = Vector3.Slerp(TablePivot.position, Vector3.zero, Time.deltaTime*20);
            yield return new WaitForFixedUpdate();
        }
    }

    void DisableIndicators()
    {
        foreach (var indicator in Indicators)
        {
            indicator.SetActive(false);
        }
    }
}
