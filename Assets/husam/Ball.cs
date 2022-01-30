using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject vfx; // patlama effecti
    private void OnCollisionEnter(Collision other) {
    
        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.currentHealth -= 20;
            Debug.Log("Hit");
        }
        GameObject patlama = Instantiate(vfx,transform.position,Quaternion.identity);
        Destroy(patlama,0.5f);
        Destroy(gameObject);
        // bu sekilde kullanınca bazen toplar havada birbirleriyle carpisip patliyorlar hosuma gittigi icin if icine yazmadim.
    }
}
