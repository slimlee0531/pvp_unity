using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;
    //[SerializeField]
    private Camera sceneCamera;

    // Start is called before the first frame update
    public override void OnNetworkSpawn() // �������ɹ����ӻ�ִ��һ��
    {
        base.OnNetworkSpawn();

        if(!IsLocalPlayer)
        {
            DisableComponents();
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
        SetPlayerName(); // ʵ�����ֲ�������ɾ
        RegisterPlayer();
    }
    private void SetPlayerName()
    {
        transform.name = "Trump" + GetComponent<NetworkBehaviour>().NetworkObjectId;
    }

    private void RegisterPlayer()
    {
        string name = "Donald " + GetComponent<NetworkObject>().NetworkObjectId.ToString();
        Player player = GetComponent<Player>();
        player.SetUp();
        GameManager.Singleton.RegisterPlayer(name, player);
    }

    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        OnDisable();
        Player player = GetComponent<Player>();
        GameManager.Singleton.UnRegisterPlayer(name, player);
    }

    private void OnDisable()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }


}
