using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KegExplode : MonoBehaviour
{
    public GameObject kegModel;
    public GameObject explosionRadius;
    public ParticleSystem explosionEffect;
    public AudioSource explosionSound;

    private void Start()
    {
        explosionRadius.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            StartCoroutine(KegExplosion());
            Debug.Log("Kerblam");
        }
    }

    IEnumerator KegExplosion()
    {
        kegModel.SetActive(false);
        explosionRadius.SetActive(true);
        explosionEffect.Play();
        explosionSound.Play();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
