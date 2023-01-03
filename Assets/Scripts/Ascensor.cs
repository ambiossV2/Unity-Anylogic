using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascensor : MonoBehaviour
{
    public GameObject person;
    public bool NoMoveXZ;

    public void Setup(GameObject agent)
    {
        person = agent;
        transform.SetParent(person.transform);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.SetParent(null);
    }

    void Update()
    {
        if (person == null) return;

        Vector3 a_pos = person.transform.position;

        if (!NoMoveXZ) if (a_pos.y + 3 >= 0) NoMoveXZ = true;

            transform.position = new Vector3(!NoMoveXZ ? a_pos.x : transform.position.x, a_pos.y, !NoMoveXZ ? a_pos.z : transform.position.z);
    }
}
