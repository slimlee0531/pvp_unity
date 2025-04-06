using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private Behaviour[] componentsToDisable;
    private bool[] componentsEnabled;
    private bool colliderEnabled;

    private NetworkVariable<float> currentHealth = new NetworkVariable<float>();
    private NetworkVariable<bool> isDead = new NetworkVariable<bool>();
    

    public void SetUp()
    {
        componentsEnabled = new bool[componentsToDisable.Length];
        for(int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsEnabled[i] = componentsToDisable[i].enabled;
        }
        Collider collider = GetComponent<Collider>();
        colliderEnabled = collider.enabled;

        SetDefaults();
    }
    private void SetDefaults()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = componentsEnabled[i];
        }
        Collider collider = GetComponent<Collider>();
        collider.enabled = colliderEnabled;

        if (IsServer)
        {
            currentHealth.Value = maxHealth;
            isDead.Value = false;
        }
    }

    public void GotShot(float damage)
    {
        if (isDead.Value) return;// 死亡后返回，不然一直被触发
        currentHealth.Value -= damage;
        if(currentHealth.Value < 0 )
        {
            currentHealth.Value = 0;
            isDead.Value = true;

            if(!IsHost)
            {
                DieOnServer();
            }
            
            DieClientRpc();
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.Singleton.MatchingSettings.respawnTime);
        SetDefaults();
        if(IsLocalPlayer)
        {
            transform.position = new Vector3(0f, 20f, 0f);//从天而降
        }
        
    }

    private void DieOnServer()
    {
        Die();
    }

    [ClientRpc]
    private void DieClientRpc()
    {
        Die();
    }

    private void Die()
    {
        for(int i = 0;i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        StartCoroutine(Respawn());
    }

    public float GetHealth()
    {
        return currentHealth.Value;
    }
}
