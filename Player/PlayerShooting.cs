using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    private WeaponManager _weaponManager;
    private PlayerWeapon currentWeapon;
    
    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
       // cam = GetComponent<Camera>();
       _weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = _weaponManager.GetCurrentWeapon();
        if(currentWeapon.shootRate <= 0)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.shootRate); // 下一次触发再0s后，频率

            }
            else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
        
    }
    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            if(hit.collider.tag == PLAYER_TAG)
            {
                ShootEnemyServerRpc(hit.collider.name, currentWeapon.damage);
            }
            else
            {
                ShootPicServerRpc(hit.collider.name);
            }
        }
    }

    [ServerRpc]
    private void ShootPicServerRpc(string PicName)
    {
        GameManager.UpdateInfo("Dear " + transform.name + ", this's " + PicName);
    }
    [ServerRpc]
    private void ShootEnemyServerRpc(string enemyName, float damage)
    {
        GameManager.UpdateInfo(transform.name + " hit " + enemyName);
        Player player = GameManager.Singleton.GetPlayer(enemyName);
        player.GotShot(damage);
    }
}
