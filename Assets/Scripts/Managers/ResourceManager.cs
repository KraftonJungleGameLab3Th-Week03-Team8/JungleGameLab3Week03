using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    // 캐싱 (추후에 구현)
    Dictionary<string, Object> _resourceCache = new Dictionary<string, Object>();

    public void Init()
    {
        Debug.Log("리소스 매니저 Init 호출");

        // TODO
        /*
         Resources 폴더 전부 순회하면서 캐싱
         */
    }

    /// <summary>
    ///  Resources 폴더의 특정 경로에서 파일 가져오기
    /// </summary> 
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// 오브젝트 소환
    /// </summary>
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(path == null) 
        {
            return null;
        }
        return Object.Instantiate(prefab, parent);
    }

    public void Destory(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}