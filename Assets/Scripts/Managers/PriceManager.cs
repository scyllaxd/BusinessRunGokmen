using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceManager : Singleton<PriceManager>
{
    public enum PriceTypes
    {
        Skateboard,
        Bicycle,
        Motorcycle
    }

    
    public bool isSkate;
    public bool isBike;
    public bool isMotorcycle;


    private PlayerManager _playerManager;

    public int price;
    
    public Image availableImage;
    public Image notAvailableImage;

    private void Awake()
    {
        _playerManager = PlayerManager.Instance;
    }

    private void Update()
    {
        PriceAvailable();
        PreventSameVehicle();
    }


    void PriceAvailable()
    {
        if(price <= CollectManager.Instance.CollectedCoin)
        {
            transform.gameObject.GetComponent<BoxCollider>().enabled = true;
            availableImage.gameObject.SetActive(true);
            notAvailableImage.gameObject.SetActive(false);
            

        }
        else
        {
            transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            notAvailableImage.gameObject.SetActive(true);
            availableImage.gameObject.SetActive(false);
            
        }
    }

    void PreventSameVehicle()
    {
        if(isSkate && _playerManager.isSkating)
        {
            transform.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else if(isBike && _playerManager.isBiking)
        {
            transform.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else if(isMotorcycle && _playerManager.isMotorcycle)
        {
            transform.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        
    }
}
