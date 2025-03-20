using UnityEngine;

public class ResourceManager
{
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
