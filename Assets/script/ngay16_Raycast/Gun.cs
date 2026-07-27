using System;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
  [SerializeField]  private float damage = 20f;
    [SerializeField] private float range = 100f;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private int maxAmmo = 30;
     private int currentAmmo;
    private bool _isReloading = false;
    [SerializeField] private float reloadTime = 2f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip emptyClip;
    [SerializeField] private AudioClip reloadClip;

    public event Action<int, int> OnAmmoChanged; // event để thông báo khi số đạn thay đổi

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo); // thông báo cho các listener biết số đạn đã thay đổi
    }

    // Update is called once per frame
    void Update()
    {
        if (_isReloading)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R) )
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo <= 0)
            {
                AudioManager.Instance.PlayerSFX(emptyClip); // phát âm thanh bắn đạn hết
                return;
            }
            Shoot();
            
           
        }
       
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        Debug.Log("Reloading...");
        AudioManager.Instance.PlayerSFX(reloadClip); // phát âm thanh nạp đạn
        yield return new WaitForSeconds(reloadTime); // tạm dừng 2s để nạp đạn 

        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo); // thông báo cho các listener biết số đạn đã thay đổi
        _isReloading = false;
        Debug.Log("Nạp XOng");
    }

    private void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play(); // phát hiệu ứng bắn đạn
        }
        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo); // thông báo cho các listener biết số đạn đã thay đổi
        AudioManager.Instance.PlayerSFX(shootClip); // phát âm thanh bắn đạn
        if (Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward, out RaycastHit hit, range))
        {
            Vector3 spawnPos =
       hit.point + hit.normal * 0.01f; // tạo ra viên đạn ở vị trí trúng đạn và dịch chuyển ra ngoài 1 chút để tránh bị chồng lên nhau
            Debug.Log("trung :" + hit.transform.name);
            GameObject impact = Instantiate(bulletPrefab, spawnPos, Quaternion.LookRotation(hit.normal)); Destroy(impact, 5f);
            // kiểm tra xem có phải enemy không , nếu phải thi sẽ gây damage
            Health health = hit.collider.GetComponent<Health>(); // object chứa collider này có script Health không
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
