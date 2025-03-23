using UnityEngine;

public class LandingEffect : MonoBehaviour
{
    // 높이에 따라 생성되는 먼지의 개수달라지게 나중에 리팩토링 
    [SerializeField] static private int _numOfDust = 8;

    static public void MakeLandingEffect()
    {
        Transform playerTransform = Manager.Game.PlayerController.PlayerTransform;
        GameObject dustPrefab;
        for (int i = 0; i < _numOfDust; i++)
        {
            // 플레이어 위치보다 y-1 위치에 생성
            dustPrefab = Manager.Resource.Instantiate("Dust");
            dustPrefab.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 0.4f, playerTransform.position.z);
        }
    }
}
