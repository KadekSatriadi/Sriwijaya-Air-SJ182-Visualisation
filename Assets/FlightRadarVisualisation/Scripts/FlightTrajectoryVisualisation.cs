using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class FlightTrajectoryVisualisation : MonoBehaviour
{
    public struct DataTransform
    {
        public Transform t;
        public FligthtData d;
    }
    public FlightRadarData database;
    public AbstractMap map;
    public Color maxSpeedColor;
    public Color minSpeedColor;

    public float trajectoryRadius = 0.0001f;
    
    public List<DataTransform> samplePoints = new List<DataTransform>();
    // Start is called before the first frame update
    void Start()
    {
        map.OnInitialized += delegate
        {
            transform.SetParent(map.transform);
            database.LoadData();
            //create trajectory
            CreateTrajectory(database.GetData());
        };
    }


    private void CreateTrajectory(FligthtData[] pathData) 
    {
        Material mat = new Material(Shader.Find("VertexColor"));
        GameObject g = new GameObject();
        g.AddComponent<MeshFilter>();
        g.AddComponent<MeshRenderer>();
        g.transform.SetParent(transform);
        TubeRenderer line =  g.AddComponent<TubeRenderer>();
        line.material = mat;
        Vector3[] points = new Vector3[pathData.Length];


        Color[] colors = new Color[pathData.Length];
        for (int i = 0; i < pathData.Length; i++)
        {
            Vector3 pos = map.GeoToWorldPosition(Vec2To2d(pathData[i].latLong));
            pos = pos + new Vector3(0, pathData[i].altitude * map.WorldRelativeScale, 0);
            pos = map.transform.InverseTransformPoint(pos);
            points[i] = pos;

            colors[i] = Color.Lerp(minSpeedColor, maxSpeedColor, (pathData[i].speed/database.GetMaxSpeed()));

            //create sample point
            GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sp.name = "data_point";
            sp.transform.localScale *= 0.01f;
            sp.transform.SetParent(transform);
            sp.transform.position = pos;
            sp.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Color"));
            sp.GetComponent<MeshRenderer>().material.color = colors[i];

            samplePoints.Add(new DataTransform {
                t = sp.transform,
                d = pathData[i]
            });
        }

        line.SetPoints(points,trajectoryRadius, colors);
    }

    private Vector2d Vec2To2d(Vector2 v)
    {
        return new Vector2d(v.x, v.y);
    }

    public FligthtData GetDataFromTransform(Transform t)
    {
        foreach(DataTransform dt in samplePoints)
        {
            if(t == dt.t)
            {
                return dt.d;
            }
        }

        return new FligthtData { };
    }
}
