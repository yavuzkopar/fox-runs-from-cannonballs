using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance{
        get{
            return instance;
        }
    }
    private void Awake() {
        instance = this;
    }
    //yukardaki kisim Singleton denen bir yapi baslangic asamasi arkadaslar fazla endislenmesin bu kodun icindeki butun public degiskenleri static yapiyor gibi dusunebilirsiniz
    [SerializeField] Slider slider; //can barimiz
    public TextMeshProUGUI pointtext; // topladigimiz coinler
    public static float maxHealth = 100f;
    [Range(0,100)]public static float currentHealth = 100f;
    public TextMeshProUGUI endPointText; // buna playerController kodundan erisip degistiriyoruz

   // public static int points;
   
    private void Start() {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = currentHealth/maxHealth;
    }
}
