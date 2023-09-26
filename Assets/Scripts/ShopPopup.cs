using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ShopPopup : MonoBehaviour
{
    [SerializeField] private Button freeButton;
    [SerializeField] private Color colorFreeButton;
    private UnityAction<bool> x;

    void OnEnable()
    {
        freeButton.GetComponent<Image>().color = Color.red;
    }

    public void ChangeColorFreeButton()
    {
        freeButton.GetComponent<Image>().color = colorFreeButton;
    }
   
   
    public void BuyRotateItem(int price)
    {
        if (DataManager.Instance.Gold > price)
        {
            DataManager.Instance.Gold -= price;
            DataManager.Instance.SaveGold();
            DataManager.Instance.RotateQuantity++;
            DataManager.Instance.SaveRotateQuantity();
        }
        else
        {
            Debug.Log("Bạn không đủ vàng");
        }
    }
    public void BuySwapItem(int price)
    {
        if (DataManager.Instance.Gold > price)
        {
            DataManager.Instance.Gold -= price;
            DataManager.Instance.SaveGold();
            DataManager.Instance.SwapQuantity++;
            DataManager.Instance.SaveSwapQuantity();
        }
        else
        {
            Debug.Log("Bạn không đủ vàng");
        }
    }
    public void ActivePopup(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public void OnClosePopup()
    {
        UIController.Instance.ShowPopup(PopupType.Shop, false);
    }

    public void ShowAds(string name)
    {
        Debug.Log("show");
        ManagerAds.Ins.ShowRewardedVideo((x) =>
            {
            if (x)
            {
                    switch (name)
                    {
                        case "swap":
                            Debug.Log("swap");
                            DataManager.Instance.SwapQuantity++;
                            DataManager.Instance.SaveSwapQuantity();
                            break;
                        case "rotate":
                            Debug.Log("rotate");
                            DataManager.Instance.RotateQuantity++;
                            DataManager.Instance.SaveRotateQuantity();
                            break;
                        case "gold":
                            Debug.Log("gold");
                            DataManager.Instance.Gold += 100;
                            DataManager.Instance.SaveGold();
                            break;
                    }
            }
        });
       
    }
 
}
