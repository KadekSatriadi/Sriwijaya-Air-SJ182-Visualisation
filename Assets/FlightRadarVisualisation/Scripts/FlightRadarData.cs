
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FlightRadarData : MonoBehaviour
{
    public TextAsset csvFile;
    private FligthtData[] fligthData;

    private float maxSpeed;
    private float maxAltitude;

    public FligthtData[] GetData()
    {
        return fligthData;
    }

    public void LoadData()
    {
        List<Dictionary<string, object>> d = CSVReader.Read(csvFile);
        fligthData = new FligthtData[d.Count];

        maxSpeed = 0f;
        maxAltitude = 0f;

        for(int i = 0; i < d.Count; i++)
        {
            Dictionary<string, object> item = d[i];
            int ts = (int) item["Timestamp"];
            DateTime utc = DateTime.Parse(item["UTC"].ToString());
            string cs = item["Callsign"].ToString();
            Vector2 latlong = StringToLatLong(item["Position"].ToString());
            float alt = float.Parse(item["Altitude"].ToString());
            float spd = float.Parse(item["Speed"].ToString());
            float dir = float.Parse(item["Direction"].ToString());

            if (maxSpeed < spd) maxSpeed = spd;
            if (maxAltitude < alt) maxAltitude = alt;

            fligthData[i] = new FligthtData
            {
                timeStamp = ts,
                UTC = utc,
                callSign = cs,
                latLong = latlong,
                altitude = alt,
                speed = spd,
                direction = dir
            };
        }
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetMaxAltitude()
    {
        return maxAltitude;
    }

    private Vector2 StringToLatLong(string s)
    {
        string[] d = s.Split(',');
        return new Vector2(float.Parse(d[0]), float.Parse(d[1]));
    }
}
