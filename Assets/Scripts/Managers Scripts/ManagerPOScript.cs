using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class ManagerPOScript : MonoBehaviour
{
    public string poolNameOfCoins;
    public string poolNameOfSkeletons;
    List<GameObject> _listCoins = new List<GameObject>();
    List<GameObject> _listSkeletons = new List<GameObject>();

    public bool CreateEvilcoin(Vector3 position, Quaternion rot)
    {
        GameObject go = EasyObjectPool.instance.GetObjectFromPool(poolNameOfCoins, position, rot);

        if (go)
        {
            _listCoins.Add(go);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CreateZombie(Vector3 position, Quaternion rot)
    {
        GameObject go = EasyObjectPool.instance.GetObjectFromPool(poolNameOfSkeletons, position, rot);

        if (go)
        {
            _listSkeletons.Add(go);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _listCoins.Count; i++)
        {
            if (!_listCoins[i].activeSelf && !_listCoins[i].GetComponent<PoolObject>().isPooled)
            {
                EasyObjectPool.instance.ReturnObjectToPool(_listCoins[i]);
                _listCoins.Remove(_listCoins[i]);
            }
        }
        for (int i = 0; i < _listSkeletons.Count; i++)
        {
            if (!_listSkeletons[i].activeSelf && !_listSkeletons[i].GetComponent<PoolObject>().isPooled)
            {
                EasyObjectPool.instance.ReturnObjectToPool(_listSkeletons[i]);
                _listSkeletons.Remove(_listSkeletons[i]);
            }
        }
    }
}
