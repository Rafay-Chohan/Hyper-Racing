using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineLapManager : MonoBehaviour
{
    public static SplineLapManager Instance { get; private set; }

    [Header("Spline Reference")]
    public SplineContainer splineContainer;

    [Header("Race Settings")]
    public int totalLaps;

    [Header("Racers")]
    public List<Racer> racers = new List<Racer>();

    private Spline splineList;

    

    [Serializable]
    public class Racer
    {
        public string name;
        public Transform transform;

        [HideInInspector] public int lap = 1;                   
        [HideInInspector] public float distanceAlongSpline = 0; 
        public float TotalProgress(float splineLength)
        {
            return lap * splineLength + distanceAlongSpline;
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (splineContainer == null)
            Debug.LogError("[SplineLapManager] splineContainer not assigned!");
    }

    void Start()
    {
        splineList = splineContainer.Spline;
        if (splineList == null)
            Debug.LogError("[SplineLapManager] splineContainer.Spline is null!");
    }

    void Update()
    {
        UpdateRacerPositions();
    }

    float GetSplineDistance(Vector3 worldPos)
    {
        var len = SplineUtility.CalculateLength(splineList, splineContainer.transform.localToWorldMatrix);
        SplineUtility.GetNearestPoint(splineList, worldPos, out float3 nearest, out float t);
        return t * len;
    }

    public void SetRacerLap(Transform racerTransform, int lap)
    {
        var r = racers.Find(x => x.transform == racerTransform);
        if (r == null)
        {
            Debug.LogWarning($"[SplineLapManager] SetRacerLap: no racer for {racerTransform.name}");
            return;
        }
        r.lap = lap;
   
    }
    public void UpdateRacerPositions()
    {
        float splineLength = SplineUtility.CalculateLength(splineList, splineContainer.transform.localToWorldMatrix);

        foreach (var r in racers)
        {
            if (r.transform == null) continue;
            r.distanceAlongSpline = GetSplineDistance(r.transform.position);

            // Debug.Log($"[Racer Update] {r.name} - Lap: {r.lap}, Dist: {r.distanceAlongSpline:F2}, TotalProgress: {r.TotalProgress(splineLength):F2}");
        }

        racers.Sort((a, b) =>
            b.TotalProgress(splineLength).CompareTo(a.TotalProgress(splineLength))
        );

        // Debug.Log("=== Racer Position Order ===");
        for (int i = 0; i < racers.Count; i++)
        {
            var r = racers[i];
            // Debug.Log($"Pos {i + 1}: {r.name} | Lap: {r.lap} | Dist: {r.distanceAlongSpline:F2} | Total: {r.TotalProgress(splineLength):F2}");
        }
    }


    public int GetRacerPosition(Transform racerTransform)
    {
        for (int i = 0; i < racers.Count; i++)
            if (racers[i].transform == racerTransform)
                return i + 1;
        return -1;
    }


    public void RegisterRacer(Transform racerTransform, string racerName)
    {
        racers.Add(new Racer { name = racerName, transform = racerTransform });
    }

    public void ShowRaceResults()
    {
        // Debug.Log("=== Final Results ===");
        // for (int i = 0; i < racers.Count; i++)
            // Debug.Log($"Pos {i + 1}: {racers[i].name} (Lap {racers[i].lap})");
    }
}
