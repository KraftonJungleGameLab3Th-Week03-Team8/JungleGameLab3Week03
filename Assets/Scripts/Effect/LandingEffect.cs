using UnityEngine;

public class LandingEffect : MonoBehaviour
{
    // 높이에 따라 생성되는 먼지의 개수달라지게 나중에 리팩토링 
    [SerializeField] static private int _numOfDust;
    [SerializeField] static private int _maxDustNum = 50;

    static public void MakeLandingEffect(float extraForce)
    {
        Transform playerTransform = Manager.Game.PlayerController.transform;
        GameObject dustPrefab;

        _numOfDust = (int)(extraForce) / 10;
        if(_numOfDust > _maxDustNum)
        {
            _numOfDust = _maxDustNum;
        }
        Debug.Log("먼지 개수 : " + _numOfDust);
        for (int i = 0; i < _numOfDust; i++)
        {
            dustPrefab = Manager.Resource.Instantiate("Dust");
            dustPrefab.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 0.4f, playerTransform.position.z);
        }
    }
}
