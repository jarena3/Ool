using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public ScreenShake ScreenShaker;

    public ToggleGroup Toggle;
    
    public GameObject CurrentIndicator;

    public List<GameObject> Indicators;
    public GameObject RayPoint;
    public GameObject Chalk;

    public Transform TablePivot;

    private bool IsTableLifting;

    private void Update()
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
    }

    private void BrickMode()
    {
        DisableIndicators();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

        if (Physics.Raycast(ray, out hitInfo))
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
            switch (CurrentIndicator.name)
            {
                case("Front"):
                    TablePivot.Rotate(Vector3.right, Input.GetAxis("Mouse Y"));
                    break;
                case("Back"):
                    TablePivot.Rotate(Vector3.left, Input.GetAxis("Mouse Y"));
                    break;
                case ("Right"):
                    TablePivot.Rotate(Vector3.forward, Input.GetAxis("Mouse Y"));
                    break;
                case ("Left"):
                    TablePivot.Rotate(Vector3.back, Input.GetAxis("Mouse Y"));
                    break;
            }
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
            TablePivot.rotation = Quaternion.Slerp(TablePivot.rotation, Quaternion.identity, Time.deltaTime * 20);
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
