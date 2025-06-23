using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
public class MeshCombiner : MonoBehaviour
{
    [Header("Object"), SerializeField] private GameObject[] _gameObjects;
    [Header("CenterTransform"), SerializeField] private Transform _center;
    [Header("ParentObjectName"), SerializeField] private string _name;
    [Header("SavePath"), SerializeField] private string _savePath;
    [Header("MakeStatic"), SerializeField] private bool _isStatic;
    [Header("DestoryObject"), SerializeField] private bool _destroy;

    public GameObject[] GameObjects => _gameObjects;
    private List<CombineInstance> _combineInstances = new List<CombineInstance>();

    public void MeshCombine()
    {
        var meshFilters = GetMeshFilters();
        MakeMeshGroup(meshFilters);

        if (_destroy)
        {
            for (int i = 0; i < _gameObjects.Length; i++)
                if (_gameObjects[i] != null)
                    Undo.DestroyObjectImmediate(_gameObjects[i]);
        }
    }

    public void ResetMeshCombiner()
    {
        _gameObjects = null;
        _name = null;
        _center = null;
    }

    private List<MeshFilter> GetMeshFilters()
    {
        var meshFilterList = new List<MeshFilter>();
        foreach (var gameObject in _gameObjects)
            meshFilterList.AddRange(gameObject.GetComponentsInChildren<MeshFilter>());

        return meshFilterList;
    }

    private void MakeMeshGroup(List<MeshFilter> meshFilters)
    {
        var meshGroup = new Dictionary<Material, List<MeshFilter>>();
        foreach(var meshFilter in meshFilters)
        {
            if(meshFilter.TryGetComponent(out MeshRenderer renderer))
            {
                var material = renderer.sharedMaterial;
                if(!meshGroup.ContainsKey(material))
                    meshGroup[material] = new List<MeshFilter>();

                meshGroup[material].Add(meshFilter);
            }
        }

        var parentObject = new GameObject(_name);
        foreach(var item in meshGroup)
        {
            var key = item.Key;
            var value = item.Value;
            CreateMesh(value, key, parentObject);
        }
    }

    private void CreateMesh(List<MeshFilter> meshFilters, Material material, GameObject parentObject)
    {
        int totalVertexCount = 0;
        foreach(var meshFilter in meshFilters)
        {
            bool isMeshFilter = meshFilter == null || meshFilter.sharedMesh == null;
            if (isMeshFilter)
            {
                Debug.Log("<CreateMesh> : meshFilter == null " +
                    "|| meshFilter.sharedMesh == null");
                continue;
            }

            Matrix4x4 matrix = _center.worldToLocalMatrix;
            _combineInstances.Add(new CombineInstance
            {
                mesh = meshFilter.sharedMesh,
                transform = matrix * meshFilter.transform.localToWorldMatrix
            });

            totalVertexCount += meshFilter.sharedMesh.vertexCount;
        }

        var childObject = new GameObject(material.name);
        childObject.transform.SetParent(parentObject.transform);
        if(_isStatic)
            childObject.isStatic = true;

        var newMesh = new Mesh();
        newMesh.indexFormat = totalVertexCount > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;

        try
        {
            newMesh.CombineMeshes(_combineInstances.ToArray(), true, true);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }

        var newMeshFilter = childObject.AddComponent<MeshFilter>();
        newMeshFilter.sharedMesh = newMesh;

        var newMeshRenderer = childObject.AddComponent<MeshRenderer>();
        newMeshRenderer.sharedMaterial = material;

        SaveMesh(newMesh, $"{_name}_CombineMesh");
        _combineInstances.Clear();
    }

    private void SaveMesh(Mesh mesh, string meshName)
    {
        string path = $"{_savePath}/{meshName}.asset";
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();

        Debug.Log("저장되었습니다.");
    }
}
#endif