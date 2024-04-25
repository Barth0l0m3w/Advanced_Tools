using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NaughtyAttributes;
using UnityEngine.Serialization;

public class InfoDump : MonoBehaviour
{
    [SerializeField] private BreakUnity breakUnity;
    [SerializeField] private int dataSample = 50;
    public string nameCvs;
    public int testNumber;

    private string _dataPath;

    private bool _active;
    private bool _fileWritten;

    private int _average;
    private int _gridX;
    private int _gridY;

    private float _lastFrameGpuTime;
    private float _gpuTime;
    private float _currentFrameGpuTime;

    private List<int> _fpsList;

    private void Start()
    {
        _dataPath = Application.dataPath + "/" + nameCvs + ".csv";
        _fpsList = new List<int>();
        _active = true;

        //creating the file and filling in the headlines
        TextWriter tw = new StreamWriter(_dataPath, false);
        tw.WriteLine("Test Number, ShadersTotal, FPS, GPU Usage");
        tw.Close();
    }

    private void Update()
    {
        if (!_active)
        {
            return;
        }

        //calculating the frames per second
        int fps = Mathf.RoundToInt(1f / Time.deltaTime);
        _fpsList.Add(fps);

        if (_fpsList.Count >= dataSample)
        {
            DataAverage();
        }
        
        //gpu usage
        _currentFrameGpuTime = Time.realtimeSinceStartup;
        _gpuTime = (_currentFrameGpuTime - _lastFrameGpuTime) * 1000; // Convert to milliseconds
        // Update last frame GPU time
        _lastFrameGpuTime = _currentFrameGpuTime;
    }

    private void DataAverage()
    {
        int sum = 0;

        foreach (var a in _fpsList)
        {
            sum += a;
        }

        _average = sum / _fpsList.Count;
        Debug.Log(_average);

        _active = false;
    }

    [Button]
    private void WriteToFile()
    {
        Debug.Log("write to file");

        _gridX = Mathf.FloorToInt(breakUnity.size.x);
        _gridX = Mathf.FloorToInt(breakUnity.size.y);

        int shadersTotal = _gridX * _gridX;

        TextWriter tw = new StreamWriter(_dataPath, true);

        tw.WriteLine(testNumber + ", " + shadersTotal
                     + ", " + _average + ", " + _gpuTime);
        tw.Close();

        testNumber += 1;
        _fpsList.Clear(); //reset
        _active = true;

        breakUnity.size = new Vector2(breakUnity.size.x + 3, breakUnity.size.y + 3); //update grid size
    }
}