using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField] private PlayerWeapon primaryWeapon;
    [SerializeField] private PlayerWeapon secondaryWeapon;
    [SerializeField] private PlayerWeapon grenadeWeapon;
    [SerializeField] private PlayerWeapon Weapon4;
    [SerializeField] private PlayerWeapon Weapon5;
    [SerializeField] private PlayerWeapon Weapon6;

    [SerializeField] private GameObject weaponHolder;
    private PlayerWeapon currentWeapon;

    private List<PlayerWeapon> weapons;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化加入链表
        weapons = new List<PlayerWeapon> { primaryWeapon, secondaryWeapon, grenadeWeapon, Weapon4, Weapon5, Weapon6 };

        EquipWeapon(primaryWeapon);  // 正手
    }

    public void EquipWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;

        while (weaponHolder.transform.childCount > 0)
        {
            DestroyImmediate(weaponHolder.transform.GetChild(0).gameObject);
        }

        // 加载图片
        GameObject weaponObject = Instantiate(currentWeapon.graphics, weaponHolder.transform.position, weaponHolder.transform.rotation);
        weaponObject.transform.SetParent(weaponHolder.transform);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void ToggleWeapon()
    {
        // 当前武器
        int currentIndex = weapons.IndexOf(currentWeapon);
        // 下一个武器
        int nextIndex = (currentIndex + 1) % weapons.Count;
        EquipWeapon(weapons[nextIndex]);
    }

    [ClientRpc]
    private void ToggleWeaponClientRpc()
    {
        ToggleWeapon();
    }

    [ServerRpc]
    private void ToggleWeaponServerRpc()
    {
        if (!IsHost)
        {
            ToggleWeapon();
        }
        ToggleWeaponServerRpc();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (!Input.GetKeyDown(KeyCode.Q))
            {
                return;
            }
            ToggleWeapon();
        }
    }
}
