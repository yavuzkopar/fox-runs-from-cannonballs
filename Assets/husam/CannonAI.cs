using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAI : MonoBehaviour
{
    [SerializeField] GameObject mermiPrefab; // firlatacagimiz gulle prefabi
    [SerializeField] GameObject mermiSpawnPos; // namlunun ucundaki bos game object
    GameObject target; 
    [SerializeField] GameObject namlu; // yukari asagi hareket ederek dogru aciyi bulmaya calisan namlu
    [SerializeField] float speed = 15;
    [SerializeField] float attackSpeed;
    float turnSpeed = 2;
    bool canShoot = true;
    [SerializeField] bool isLow = true;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void CanShootAgain()
    {
        canShoot = true;
    }

    void Fire()
    {
        if (canShoot)
        {
            GameObject shell = Instantiate(mermiPrefab, mermiSpawnPos.transform.position, mermiSpawnPos.transform.rotation);
            shell.GetComponent<Rigidbody>().velocity = speed * namlu.transform.forward; // transform.forward objenin on yonunu gösterir aci hesaplamalarinda sikca kullanilir
            // yukarda speed kadar gucu bir kere uyguluyoruz ve canShoot = false yapiyoruz atis yaptiktan sonra kuvvet uygulamaya devaam etmiyoruz yani serbest dususe geciyoruz 
            canShoot = false;
            Invoke("CanShootAgain", attackSpeed); // attackSpeed suresi kadar sonra CanShootAgain fonksiyonunu cagiriyoruz. Invoke bu tarz gecikmeli fonksiyon calistirmak icin kullanilir
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.transform.position - this.transform.position).normalized;
       // direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        //yukardaki kisim genel olarak bir yone belli bir hizda ve yumusaklikta donmemizi saglar
        float? angle = RotateTurret();

        if(angle != null && Vector3.Angle(direction, this.transform.forward) < 10) // burdaki 10 degerini kuculterek daha isabetli atis yapabilirsiniz
        {
            Fire();
        }     
    }

    float? RotateTurret() // float? ifadesi bu degerin belirli durumlarda bir sonuc veremeyebilecegi demektir.
    {
        float? angle = CalculateAngle(isLow);

        if (angle != null)
        {
            namlu.transform.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f);
        }
        return angle;
    }

    float? CalculateAngle(bool low) // burda iki farkli sonuc elde edebilecegimiz icin bir bool kullaniyoruz. 3. sonuc, bir sonucun olmamasi 
    {
        //asagidaki kisim icin lütfen paylastigim egitime goz atin matematik ve fizik formullerini benden daha iyi anlatiyor.
        Vector3 targetDir = target.transform.position - namlu.transform.position;
        float y = targetDir.y;
        float x = targetDir.magnitude;
        float gravity = 9.81f;
        float sSqr = speed * speed;
        float underTheSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr); // butun float? ların sebebi bu formul. bu formul - degerde cikarsa oyle bir aci yok demektir

        if (underTheSqrRoot >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;

            if (low)
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);

        }
        else
            return null;
    }
}
