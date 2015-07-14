using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(1,1,1 * Time.deltaTime);
    }
}
