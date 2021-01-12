using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI text;


    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.transform != null && hit.transform.name == "data_point")
        {
            FlightTrajectoryVisualisation vis = hit.transform.parent.GetComponent<FlightTrajectoryVisualisation>();
            FligthtData data = vis.GetDataFromTransform(hit.transform);
            string t = "Date: " + data.UTC + "\n";
            t += "Altitude: " + data.altitude + " ft \n";
            t += "Speed: " + data.speed + " kts \n";

            Vector3 offset = new Vector3(100, 100, 0);
            text.rectTransform.position =  Camera.main.WorldToScreenPoint(hit.transform.position) + offset;
            text.text = t; 
        }
        else
        {
            text.text = "";
        }
    }

}
