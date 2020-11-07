using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int damage = 0;
    public bool isStriking { get; set; }

    void Awake(){
        this.isStriking = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        //set damage for individual hitboxes. This might vary depending on the weapon
        //such as a sword that hits harder at the very tip
        for(int childIndex = 0; childIndex < this.gameObject.transform.childCount; ++childIndex){
            var childTransform = this.gameObject.transform.GetChild(childIndex);
            childTransform.gameObject.GetComponent<WeaponHitbox>().damage = damage;
        }    
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void DoWeaponStrike(){
        this.isStriking = true;
        StartCoroutine(Strike());
    }

    IEnumerator Strike(){
        //do the strike handling I guess?
        yield return new WaitForSeconds(1);
        this.isStriking = false;
        this.gameObject.SetActive(false);
    }
}
