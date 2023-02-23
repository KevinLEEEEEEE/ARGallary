using UnityEngine;
using LeanCloud;
using LeanCloud.Storage;
using System;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;

public class LeanServer : MonoBehaviour
{
    [SerializeField] private string APP_ID;
    [SerializeField] private string APP_KEY;
    [SerializeField] private string Server;
    [SerializeField] private string className;
    [SerializeField] private string indexParameter;
    [SerializeField] private bool debugMode;

    void Start()
    {
        LCApplication.Initialize(APP_ID, APP_KEY, Server);

        if (debugMode) EnableDebugger();
    }

    public void EnableDebugger()
    {
        LCLogger.LogDelegate += (level, info) =>
        {
            switch (level)
            {
                case LCLogLevel.Debug:
                    Debug.Log($"[DEBUG] {DateTime.Now} {info}\n");
                    break;
                case LCLogLevel.Warn:
                    Debug.LogWarning($"[WARNING] {DateTime.Now} {info}\n");
                    break;
                case LCLogLevel.Error:
                    Debug.LogError($"[ERROR] {DateTime.Now} {info}\n");
                    break;
                default:
                    Debug.Log(info);
                    break;
            }
        };
    }

    public async UniTask<LCObject> QueryFirstAsync(string indexValue)
    {
        try
        {
            Debug.Log("[LeanServer] Start querying voxel data, index value: " + indexValue);

            LCQuery<LCObject> query = new(className);
            query.WhereEqualTo(indexParameter, indexValue);

            return await query.First();
        }
        catch (LCException e)
        {
            Debug.LogError(e);

            return new LCObject("Failed");
        }

    }
}
