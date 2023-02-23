using LeanCloud.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Fraktalia.VoxelGen;
using System;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private VoxelSaveSystem voxelSaveSystem;
    [SerializeField] private LeanServer leanServer;
    [SerializeField] private GameObject VoxelBlock;
    [SerializeField] private TMP_InputField indexValueInputField;
    [SerializeField] private string indexValue;

    private bool isLoading = false;

    void Start()
    {
        indexValueInputField.onEndEdit.AddListener(SetIndexValue);
        indexValueInputField.text = indexValue;  
    }

    public void SetIndexValue(string content)
    {
        Debug.Log(string.Format("[GameController] Set index value to: {0}", content));

        indexValue = content;
    }

    public async void LoadVoxel()
    {
        if (isLoading) return;

        Debug.Log("[GameController] Start loading voxel");

        isLoading = true;

        if(ContainByteBuffer(indexValue))
        {
            Debug.Log("[GameController] Loading existing voxel data");
            LoadVoxel(indexValue);
        } else
        {
            LCObject result = await leanServer.QueryFirstAsync(indexValue);

            if (result == null)
            {
                Debug.Log("[GameController] Invalid index value");
            }
            else if (result.ClassName == "Failed")
            {
                Debug.Log("[GameController] Network error");
            }
            else
            {
                Debug.Log("[GameController] Get query result successfully");
                LoadVoxel(result["ID"] as string, result["Model"] as string);
            }
        }

        isLoading = false;
    }

    private bool ContainByteBuffer(string id)
    {
        return Fraktalia.VoxelGen.SaveSystem.Modules.SaveModule_ByteBuffer_V2.VoxelDictionary.ContainsKey(id);
    }

    private void LoadVoxel(string id, string base64)
    {
        Fraktalia.VoxelGen.SaveSystem.Modules.SaveModule_ByteBuffer_V2.VoxelDictionary.Add(id, Convert.FromBase64String(base64));
        voxelSaveSystem.ModuleByteBuffer_V2.Key = id;
        voxelSaveSystem.Load();
    }

    private void LoadVoxel(string id)
    {
        voxelSaveSystem.ModuleByteBuffer_V2.Key = id;
        voxelSaveSystem.Load();
    }
}
