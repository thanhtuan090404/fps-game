using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float damage = 20f;
    private float range = 100f;
    [SerializeField] public Camera playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range, Color.red, 1f);

        }
    }

    private void Shoot()
    {
        if(Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward, out RaycastHit hit, range))
        {
            Debug.Log("trung :" + hit.transform.name);
            // kiểm tra xem có phải enemy không , nếu phải thi sẽ gây damage
            Target target = hit.transform.GetComponent<Target>();  // lấy component Target của đối tượng bị trúng đạn
            if (target != null) // nếu có component Target thì gây damage
            {
                target.TakeDamage(damage);
            }
        }
    }
}
