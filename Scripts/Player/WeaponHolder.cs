using UnityEngine;


//class responsible for holding player weapons attached as child game objects and switching between them
public class WeaponHolder : MonoBehaviour
{
    //declare fields and script to communicate with
    public int selectedWeapon = 0;
    public GameController gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //set references and initial weapon
        gameManager = GameObject.Find("GameManager").GetComponent<GameController>();
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        //record previous weapon
        int previousSelectedWeapon = selectedWeapon;
        //switch weapon with scroll wheel, using the child count of weapon game objects under weapon holder game object, reseting at end of child count
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        //switch weapon but in opposite direction, resetting at end of child count
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }
        //switch weapon with assigned number keys, have to add with each new weapon, undecided which system to keep so both here for now
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        //change to new active weapon selected from old weapon
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
    //disabled old weapon child and sets active newly selected weapon child
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
        //updates damge dice UI and stats used accordingly
        gameManager.PlayerUpdateActiveDamageDice();
    }
}
