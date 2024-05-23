using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
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

    private float _average;
    private int _gridX;
    private int _gridY;

    private float _lastFrameGpuTime;
    private float _gpuTime;
    private float _currentFrameGpuTime;

    private float _minData;
    private float _maxData;

    private List<float> _fpsList;

    private void Start()
    {
        _dataPath = Application.dataPath + "/" + nameCvs + ".csv";
        _fpsList = new List<float>();
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
        float fps = Mathf.RoundToInt(1f / Time.deltaTime); //float
        _fpsList.Add(fps);

        if (_fpsList.Count >= dataSample)
        {
            DataAverage();
            _minData = DataMinimum();
            _maxData = DataMaximum();
            
            Debug.Log(_minData);
            Debug.Log(_maxData);
        }
        
        //gpu usage
        _currentFrameGpuTime = Time.realtimeSinceStartup;
        _gpuTime = (_currentFrameGpuTime - _lastFrameGpuTime) * 1000; // Convert to milliseconds
        // Update last frame GPU time
        _lastFrameGpuTime = _currentFrameGpuTime;
    }

    private void DataAverage()
    {
        float sum = 0;

        foreach (var a in _fpsList)
        {
            sum += a;
        }

        _average = sum / _fpsList.Count;
        Debug.Log(_average);

        _active = false;
    }

    private float DataMinimum()
    {
        return _fpsList.Min();
    }
    
    private float DataMaximum()
    {
        return _fpsList.Max();
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