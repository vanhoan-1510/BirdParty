using UnityEngine;

public class TriggerToRotareGroundRight : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 15f;

    private bool isRotated = false;
    private Quaternion targetQuaternion;

    private void Start()
    {
        targetQuaternion = Quaternion.Euler(transform.eulerAngles + Vector3.forward * -80f);
    }
    private void Update()
    {
        if (isRotated)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isRotated = true;
            AudioManager.Instance.PlaySFX("Eggy1");
        }
    }
}