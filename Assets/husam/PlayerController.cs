using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] DynamicJoystick joystick; // kullandiginiz joystick packteki prefablardan biri, ben bunu tercih ettim siz baska birini de tercih edebilirsiiz
    Vector3 inputVector; // joystick hareket ettirirken bize dondurecegi degeri tuttugumuz ve hareketimizin yonunu belirleyen vektor
    Animator animator;
    public static bool isDead = false;
    [SerializeField]GameObject vfx; // coinlere carptigimizda olusacak particle effect
    int points = 0;
    [SerializeField] GameObject endpanel; //oldugumuzde ortaya cikacak Restart, Main Menu butonlarının oldugu panel
    private void Start() 
    {
    animator = GetComponent<Animator>();    
    Time.timeScale=1f; // basta 1'e esitliyoruz cunku sonrasında 0'a esitledigimizde ve oyunu tekrar baslattigimizda oyle kalmasin.
    }

    // Update is called once per frame
    void Update()
    {
        if(UIManager.currentHealth <= 5)
        {
            animator.SetTrigger("isDead");
            // burada Invoke kullanmamin sebebi Death fonksiyonu icinde oyunu dondurdugumuz icin animasyon oynama firsati bulamiyordu
            Invoke("Death",2f);
            return;
        }
        inputVector = new Vector3(joystick.Horizontal,0,joystick.Vertical); // joystick.Horizontal,joystick.Vertical assetle birlikte gelen kodlar. Joystickin yataydaki hareketini x eksenine, dikeydeki hareketini z eksenine esitliyoruz oyun 3d oldugu icin
        transform.position += inputVector*speed * Time.deltaTime;
        Quaternion lookRotation = Quaternion.LookRotation(inputVector.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,rotationSpeed*Time.deltaTime);
        animator.SetFloat("Speed",inputVector.magnitude);//burda animatordeki blendTree'nin degerine erisiyoruz (0-1 arası)
        
    }
    void Death()
    {
        
        Time.timeScale = 0f; // oyun tekrar basladıgında Time.timeSlace = 1'e esitlememizin sebebi
        endpanel.SetActive(true);
        UIManager.Instance.endPointText.text = "Well done, your score is \n" + points.ToString();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Collectible"))
        {
            points ++;
            UIManager.Instance.pointtext.text= points.ToString();
            GameObject puan = Instantiate(vfx,transform.position,Quaternion.identity); // daha sonra yok edecegimiz icin bu sekilde ayri bir gameobject olarak tutuyoruz effecti
            Destroy(puan,0.5f);
            Vector3 spawnPos = new Vector3(Random.Range(-20,20),1,Random.Range(-20,20)); // coinleri yok etmek yerine haritamiz icindeki rastgele bir nokta hesaplayip oraya goturuyoruz
            other.transform.position = spawnPos;
        }
    }
}
