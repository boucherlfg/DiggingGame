using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (!GUILayout.Button("Generate World")) return;
        var wg = (WorldGenerator)target;
        wg.Generate();
    }
}
#endif

[Serializable]
public struct Resource
{
    public int epicenterQty;
    public int quantity;
    public int minDepth;
    public int maxDepth;
    public GameObject prefab;
}
public class WorldGenerator : MonoBehaviour
{
    public int width = 1000;
    public int height = 10000;
    [SerializeField] private Vector2 playerStartPosition = new Vector2(0, 4);
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject shop;
    [SerializeField] private Vector2 shopStartPosition = new Vector2(0, 0.5f);
    [SerializeField] private GameObject dirt;
    [SerializeField] private Resource[] resources;
    [SerializeField] private GameObject bedrock;
    [SerializeField] private Resource empty;
    [SerializeField] private GameObject[] trees;
    [SerializeField] private int treeAmount = 10;
    private readonly List<GameObject> _treeInstances = new();
    private Transform _parent;

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        RegenerateTrees();
    }

    void RegenerateTrees()
    {
        _treeInstances.RemoveAll(tree => tree.transform.childCount <= 2);
        while (_treeInstances.Count < treeAmount)
        {
            GenerateOneTree();
        } 
    }
    private int[] _map;
    public void Generate()
    {
        _parent = new GameObject().transform;
        _parent.parent = transform;
        _map = new int[width * height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                _map[i * height + j] = 0;
            }
        }

        for (var i = 0; i < resources.Length; i++)
        {
            GenerateResource(i);
        }

        GenerateEmpty();

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var index = i * height + j;
                var value = _map[i * height + j];
                if (value < 0) continue;
                var prefab = value == 0 ? dirt : resources[value - 1].prefab;
                Instantiate(prefab, new Vector3(i - width/2, -j), Quaternion.identity, _parent);
            }
        }
        GenerateBedrock();
        GenerateTrees();
        Instantiate(player, playerStartPosition, Quaternion.identity);
        Instantiate(shop, shopStartPosition, Quaternion.identity);
    }

    private void GenerateBedrock()
    {
        const int floor = 30;
        for (var i = -width/2 - 1; i < width/2; i++)
        {
            Instantiate(bedrock, new Vector3(i, -height), Quaternion.identity, _parent);
        }

        for (var j = -height; j < 0; j++)
        {
            Instantiate(bedrock, new Vector3(-width/2 - 1, j), Quaternion.identity, _parent);
            Instantiate(bedrock, new Vector3(width/2, j), Quaternion.identity, _parent);
        }

        var radius = width / 2 + 1;
        var alreadyCovered = new List<Vector2Int>();
        for (int i = 0; i < 2000; i++)
        {
            var pos = new Vector2Int((int)(Mathf.Cos(Mathf.PI * (i / 2000f))*radius), (int)(Mathf.Sin(Mathf.PI * (i / 2000f))*radius));
            if (alreadyCovered.Contains(pos))
            {
                continue;
            }
            alreadyCovered.Add(pos);
            Instantiate(bedrock, (Vector2)pos, Quaternion.identity, _parent);
        }
    }

    private void GenerateTrees()
    {
        for (int i = 0; i < treeAmount; i++)
        {
            GenerateOneTree();
        }
    }

    private void GenerateOneTree()
    {
        var spawn = new Vector2(Random.Range(-width/2, width/2), 0);
        var tree = trees.Random();
        _treeInstances.Add(Instantiate(tree, spawn, Quaternion.identity, _parent));
    }
    private void GenerateEmpty()
    {
        var indices = new List<int>();
        var min = empty.minDepth;
        var max = empty.maxDepth > 0 ? empty.maxDepth : height;
        for (var _ = 0; _ < empty.epicenterQty; _++)
        {
            var random = new Vector2Int(Random.Range(0, width), Random.Range(min, max));
            _map[random.x * height + random.y] = -1;
            indices.Add(random.x * height + random.y);
        }
        
        var count = indices.Count;
        while(count < empty.quantity)
        {
            if (indices.Count <= 0)
            {
                Debug.LogWarning("no indices left");
                return;
            }
            
            var random = indices.Random();
            var neighbours = random.GetNeighbours(width, height);
            
            if (neighbours.All(x => _map[x] == -1))
            {
                indices.Remove(random);
                continue;
            }

            var neighbour = neighbours.First(x => _map[x] != -1);
            _map[neighbour] = -1;
            indices.Add(neighbour);
            count++;
        }
    }
    private void GenerateResource(int resourceIndex)
    {
        var resource = resources[resourceIndex];
        var min = resource.minDepth;
        var max = resource.maxDepth > 0 ? resource.maxDepth : height;
        var indices = new List<int>();
        for (var _ = 0; _ < resource.epicenterQty; _++)
        {
            var random = new Vector2Int(Random.Range(0, width), Random.Range(min, max));
            _map[random.x * height + random.y] = resourceIndex + 1;
            indices.Add(random.x * height + random.y);
        }
        
        var count = indices.Count;
        while(count < resource.quantity)
        {
            if (indices.Count <= 0)
            {
                Debug.LogWarning("no indices left");
                return;
            }
            
            var random = indices.Random();
            var neighbours = random.GetNeighbours(width, height);
            
            if (neighbours.All(x => _map[x] == resourceIndex + 1))
            {
                indices.Remove(random);
                continue;
            }

            var neighbour = neighbours.First(x => _map[x] != resourceIndex + 1);
            _map[neighbour] = resourceIndex + 1;
            indices.Add(neighbour);
            count++;
        }
    }

}
