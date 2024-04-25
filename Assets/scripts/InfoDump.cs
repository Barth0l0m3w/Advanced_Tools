using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.Profiling;

public class InfoDump : MonoBehaviour
{
    [SerializeField] private BreakUnity _breakUnity;

    private string filename = "";
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
        
        lastFrameGpuTime = Time.realtimeSinceStartup;
        filename = Application.dataPath + "/test.csv";

        fpsList = new List<int>();
        active = true;
        
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Test Number, ShadersTotal, FPS, GPU Usage");itk
        tw.Close();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A)))
        {
            Debug.Log("write to file activated");
            WriteToFile();
            //fileWritten = true;
        }

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
        gridX = Mathf.FloorToInt(_breakUnity.size.x);
        gridX = Mathf.FloorToInt(_breakUnity.size.y);
        
        StringBuilder builder = new();
        int sum = 0;

        foreach (var a in fpsList)
        {
            sum += a;
        }
        
        Debug.Log(gpuTime);
        
        average = sum / dataSample;
        active = false;
    }

    private void WriteToFile()
    {
        int shadersTotal = gridX * gridX;
        
        TextWriter tw = new StreamWriter(filename, true);

        tw.WriteLine(testNumber + ", " + shadersTotal
                     + ", " + average + ", " + gpuTime);
        tw.Close();
        
        testNumber += 1;
        fpsList.Clear(); //reset
        active = true;
    }
}