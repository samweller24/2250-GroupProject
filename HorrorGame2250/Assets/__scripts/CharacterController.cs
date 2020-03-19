﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public Text alertText;
    public Text storyText;
    public GameObject Player;
    public GameObject hand;
    public GameObject leftHand;
    public GameObject spotLight;
    private Inventory inventory;
    public Image img;
    private Vector3 FlashlightAngle = new Vector3(124.943f, 3.28299f, -176.69f);
    public UI_Inventory uiInventory;
    private bool status = false;
    private bool fDown = false;
    private bool pageOneVisted = false;

    private bool isGrounded;
    public bool imgStatus;
    public AudioSource playerHit;
    Scene m_Scene;
    string sceneName;



    public float health = 10f;

    //Start is called before the first frame update

    public bool imgStatus;
    private readonly string selectedCharacter = "SelectedCharacter";


    public float speed;
    //Start is called before the first frame update
    void Start()
    {
        int getCharacter;
        getCharacter = PlayerPrefs.GetInt(selectedCharacter);

        switch(getCharacter)
        {
            case 1:
                speed = 23.0f;
                break;
            case 2:
                speed = 17.0f;
                break;
            case 3:
                speed = 10.0f;
                break;
            default:
                speed = 17.0f;
                break;
        }


        Cursor.lockState = CursorLockMode.Locked;
        img.enabled = false;
        ResetText();
        img.enabled = false;
        playerHit = GetComponent<AudioSource>();

         
         if(SceneManager.GetActiveScene().name == "SamLevel"){
             CheckLight();
         }
    }

    private void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }

    // Update is called once per frame
    void Update()
    {
        float translation;
        float strafe;
        CheckLight();

        // float translation = Input.GetAxis("Vertical");
        // float strafe = Input.GetAxis("Horizontal");
        // Vector3 move = transform.right * strafe + transform.forward * translation;

        // transform.Translate(move);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            translation = Input.GetAxis("Vertical") * speed * 2f;
            strafe = Input.GetAxis("Horizontal") * speed * 1.5f;
        }
        else
        {
            translation = Input.GetAxis("Vertical") * speed * 1.0f;
            strafe = Input.GetAxis("Horizontal") * speed * 1.0f;
        }

        //float translation = Input.GetAxis("Vertical")*speed;
        //float strafe = Input.GetAxis("Horizontal")*speed;
        translation *= Time.deltaTime;
        strafe *= Time.deltaTime;



        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
        {

        }

        transform.Translate(strafe, 0, translation);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown("f"))
        {
            Debug.Log("F press");
            fDown = true;
        }

    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PickUp")
        {
            Debug.Log("Pickup Detected");
            ItemFoundCalled();
            Update();
            if (fDown)
            {
                Debug.Log("Call from inside");
                SetObject(col.gameObject);
                ResetText();
                Debug.Log("Object being set");
            }
        }
        if (col.gameObject.tag == "StoryPage")
        {
            Debug.Log("Pickup Detected");
            StoryPageFoundCalled();
            Update();
            if (fDown)
            {
                Debug.Log("Call from inside");
                ViewStoryPage(col.gameObject);
                ResetText();
                Debug.Log("Object being set");
            }
        }
    }
    public void ItemFoundCalled()
    {
        alertText.text = "~Item Found - Press F to Pick Up~";
    }

    public void StoryPageFoundCalled()
    {
        alertText.text = "~Story Page Found - Press F to View ~";
    }

    public void ResetText()
    {
        alertText.text = "";

    }

    public void CheckLight()
    {
        spotLight.SetActive(status);
    }

    public void SetObject(GameObject goItem)
    {
        ResetText();
        if (goItem.name == "Flashlight")
        {
            goItem.transform.parent = hand.transform;
            goItem.transform.eulerAngles = FlashlightAngle;
            goItem.transform.position = hand.transform.position;

            status = true;
            CheckLight();
            inventory.AddItem(new PickUpItem { itemType = PickUpItem.ItemType.Flashlight, amount = 1 });
            uiInventory.SetInventory(inventory);
        }
        if (goItem.name == "Gun")
        {
            goItem.transform.parent = leftHand.transform;
            goItem.transform.eulerAngles = FlashlightAngle;
            goItem.transform.position = leftHand.transform.position;
        }

    }

    public void ViewStoryPage(GameObject goItem)
    {
        if (goItem.name == "StoryPage1")
        {
            imgStatus = true;
            img.enabled = imgStatus;
            if (!pageOneVisted)
            {
                inventory.AddItem(new PickUpItem { itemType = PickUpItem.ItemType.Map, amount = 1 });
                uiInventory.SetInventory(inventory);
            }
         }

         public void ExitStoryPage(){
                imgStatus = false;
                img.enabled = imgStatus;
         }

        public void TakeDamage(float amount){
            health -= amount;
            //playerHit.Play();
            if(health <= 0){
            Die();
            }
        }
        
        void Die(){
             Destroy(gameObject);
             alertText.text = "You Died";
             SceneManager.LoadScene("SamLevel");
         }

         void  OnTriggerExit(Collider col){
               ResetText();
            ExitStoryPage();
         }



}
