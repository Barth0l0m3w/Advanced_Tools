using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Threading;
using NaughtyAttributes;
using UnityEngine.Profiling;
using Object = System.Object;

public class InfoDump : MonoBehaviour
{
    [SerializeField] private BreakUnity _breakUnity;
    public string name;
    private string filename;
    private bool active;
    private bool fileWritten;
    private int average;

    private int gridX;
    private int gridY;

    private float lastFrameGpuTime;
    private float gpuTime;
    private float currentFrameGpuTime;

    [SerializeField] private int dataSample = 100;

    private List<int> fpsList;

    public int testNumber;


    private void Start()
    {
        filename = Application.dataPath + "/" + name + ".csv";

        fpsList = new List<int>();
        active = true;

        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Test Number, ShadersTotal, FPS, GPU Usage");
        tw.Close();
    }

    private void Update()
    {
        if (!active)
        {
            return;
        }

        if (fpsList.Count >= dataSample)
        {
            DataAverage();
        }

        int fps = Mathf.RoundToInt(1f / Time.deltaTime);

        //gpu usage
        currentFrameGpuTime = Time.realtimeSinceStartup;
        gpuTime = (currentFrameGpuTime - lastFrameGpuTime) * 1000; // Convert to milliseconds
        // Update last frame GPU time
        lastFrameGpuTime = currentFrameGpuTime;

        fpsList.Add(fps);
    }

    private void DataAverage()
    {
        int sum = 0;

        foreach (var a in fpsList)
        {
            sum += a;
        }

        average = sum / dataSample;
        Debug.Log(average);

        active = false;
    }

    [Button]
    private void WriteToFile()
    {
        Debug.Log("write to file");

        gridX = Mathf.FloorToInt(_breakUnity.size.x);
        gridX = Mathf.FloorToInt(_breakUnity.size.y);

        int shadersTotal = gridX * gridX;

        TextWriter tw = new StreamWriter(filename, true);

        tw.WriteLine(testNumber + ", " + shadersTotal
                     + ", " + average + ", " + gpuTime);
        tw.Close();

        testNumber += 1;
        fpsList.Clear(); //reset
        active = true;

        _breakUnity.size = new Vector2(_breakUnity.size.x + 3, _breakUnity.size.y + 3); //update grid size
    }
}