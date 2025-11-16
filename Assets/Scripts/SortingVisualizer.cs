using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class SortingVisualizer : MonoBehaviour
{
    public enum SortingAlgorithm
    {
        BogoSort,
        GnomeSort,
        BubbleSort,
        CocktailSort,
        InsertionSort,
        SelectionSort,
        ShellSort,
        BitonicSort,
        MergeSort,
        HeapSort,
        QuickSort,
        IntroSort,
        AdaptiveMergeSort,
        RadixMSD,
        RadixLSD
    }

    public enum DataType { Int, String }

    [Serializable]
    public class SortingConfig
    {
        public SortingAlgorithm algorithm = SortingAlgorithm.BubbleSort;
        public DataType dataType = DataType.Int;
        public int dataSize = 24;
        public int bogoMaxIterations = 1000000;
    }

    [Header("Visualization")]
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform _offset;

    [Header("Sorting Settings")]
    [SerializeField] private SortingConfig _sortingConfig = new();
    [SerializeField] private int _maxNumValue = 100;
    [SerializeField] private int _minNumValue = 0;

    private List<GameObject> _visualElements = new();
    private System.Collections.IList _data;
    private Stopwatch _stopwatch = new();
    private IVisualizer _visualizer;

    private void Start()
    {
        if (_offset == null)
            _offset = transform;

        InitVisualization();
    }

    private void InitVisualization()
    {
        switch (_sortingConfig.dataType)
        {
            case DataType.Int:
                _visualizer = new NumericVisualizer(_cubePrefab, _offset, _visualElements, _minNumValue, _maxNumValue, _sortingConfig.dataSize);
                break;
            case DataType.String:
                _visualizer = new StringVisualizer(_sortingConfig.dataSize);
                break;
            default:
                break;
        }

        _visualizer?.Randomize();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            SortData();

        if (Input.GetKeyUp(KeyCode.R))
            RandomizeValues();
    }

    private void RandomizeValues()
    {
        _visualizer.Randomize();
    }

    private void SortData()
    {
        _stopwatch.Start();

        SortingManager.Sort(_sortingConfig, _visualizer.Data);

        _stopwatch.Stop();

        UnityEngine.Debug.Log($"Sorting completed in {_stopwatch.Elapsed.TotalMilliseconds:F3} ms using {_sortingConfig.algorithm}");

        _stopwatch.Reset();

        _visualizer.UpdateVisualData();
    }
}

internal class StringVisualizer : IVisualizer
{
    private int _dataSize;
    private int _stringSizeMax;
    private int _stringSizeMin;
    private List<string> _data;
    public IList Data => _data;

    public StringVisualizer(int dataSize, int stringSizeMin = 3, int stringSizeMax = 8)
    {
        _dataSize = dataSize;
        _stringSizeMin = stringSizeMin;
        _stringSizeMax = stringSizeMax;
    }

    public void Randomize()
    {
        RandomizeData();
        UpdateVisualData();
    }

    public void RandomizeData()
    {
        _data = new List<string>();

        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        for (int i = 0; i < _dataSize; i++)
        {
            int length = UnityEngine.Random.Range(_stringSizeMax, _stringSizeMin);
            string randomString = "";

            for (int j = 0; j < length; j++)
                randomString += chars[UnityEngine.Random.Range(0, chars.Length)];

            _data.Add(randomString);
        }
    }

    public void UpdateVisualData()
    {
        UnityEngine.Debug.Log($"String Data: [{string.Join(", ", _data)}]");
    }
}

public interface IVisualizer
{
    IList Data { get; }
    void Randomize();
    void RandomizeData();
    void UpdateVisualData();
}

internal class NumericVisualizer : IVisualizer
{
    private GameObject _cubePrefab;
    private Transform _offset;
    private readonly List<GameObject> _visualElements;
    private int _minNumValue;
    private int _maxNumValue;
    private int _dataSize;
    private List<int> _data;
    public IList Data => _data;

    public NumericVisualizer(GameObject cubePrefab, Transform offset, List<GameObject> visualElements, int minNumValue, int maxNumValue, int dataSize)
    {
        _cubePrefab = cubePrefab;
        _offset = offset;
        this._visualElements = visualElements;
        _minNumValue = minNumValue;
        _maxNumValue = maxNumValue;
        _dataSize = dataSize;

        InitVisualElements();
        Randomize();
    }

    private void InitVisualElements()
    {
        for (int i = 0; i < _dataSize; i++)
        {
            GameObject instance = GameObject.Instantiate(_cubePrefab, _offset);
            var newPos = instance.transform.position;
            newPos.y = 0;
            newPos.x += i;
            instance.transform.position = newPos;
            _visualElements.Add(instance);
        }
    }

    public void RandomizeData()
    {
        _data = new List<int>();

        for (int i = 0; i < _dataSize; i++)
        {
            _data.Add(UnityEngine.Random.Range(_minNumValue, _maxNumValue + 1));
        }
    }

    public void Randomize()
    {
        RandomizeData();
        UpdateVisualData();
    }

    public void UpdateVisualData()
    {
        for (int i = 0; i < _data.Count; i++)
        {
            var sizeY = Mathf.Max(_data[i] / 10f, 0.1f);

            _visualElements[i].transform.localScale = new(1f, sizeY, 1f);

            var newPos = _visualElements[i].transform.position;
            newPos.y = 0;
            newPos.y += sizeY / 2;
            _visualElements[i].transform.position = newPos;

            var text = _visualElements[i].GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = _data[i].ToString();
        }
    }
}