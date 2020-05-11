using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Account balnace, Stock balance, Company Stock Price, Change, Owning

    [SerializeField] float accountBalanceValue = 10000f;
    [SerializeField] float stockBalanceValue = 0f;
    [SerializeField] float companyStockPriceValue = 500f;
    [SerializeField] float ChangeValue = 0f;
    [SerializeField] float owningValue = 0f;

    [SerializeField] Text accountBalanceText;
    [SerializeField] Text stockBalanceText;
    [SerializeField] Text companyStockPriceText;
    [SerializeField] Text changeText;
    [SerializeField] Text owningText;

    float AmountOfBuyStocksValue;
    float currentAccountBanlanceValue;
    float currentOwningValue = 1f;







    public void Awake()
    {
        accountBalanceText.text = accountBalanceValue.ToString();

        stockBalanceText.text = stockBalanceValue.ToString();

        companyStockPriceText.text = companyStockPriceValue.ToString();

        changeText.text = ChangeValue.ToString();

        owningText.text = owningValue.ToString();

        StartCoroutine("calCompanyStockValue");
        Debug.Log("StartCoroutine comStockValue");

    }

    public GameObject windoW_graph;


    IEnumerator calCompanyStockValue()
    {
        //companyStockPriceValue의 가격은 기본가격 x 등락률인데, 등락률은 v = Windows_graph.addNewValue()의 값을 반영하면 된다.  companyStockPriceValue + or - (v/companyStockPriceValue * 100)
        //이전 가격과 비교해서 주식이 상승하면 +, 하락하며 -로 계산해야
        Debug.Log("update1");

        while (true)
        {
            int addValue = (int)windoW_graph.GetComponent<Window_Graph>().addValue;

            companyStockPriceValue = (int)(companyStockPriceValue + (addValue / companyStockPriceValue * 100));
            companyStockPriceText.text = companyStockPriceValue.ToString();

            ChangeValue = addValue;
            changeText.text = ChangeValue.ToString();
            Debug.Log("companyStockPriceValue:" + companyStockPriceValue);

            yield return new WaitForSeconds(1.0f);
        }
       

    }




    public void buy()
    {

        Debug.Log("Buy");

        //주식을 구매하면, owning이 하나씩 카운트 ++ 된다.
        currentOwningValue = owningValue++;

        //companyStockPriceValue의 가격은 기본가격 x 등락률인데, 등락률은 v = Windows_graph.addNewValue()의 값을 반영하면 된다.  companyStockPriceValue + or - (v/companyStockPriceValue * 100)
        //이전 가격과 비교해서 주식이 상승하면 +, 하락하며 -로 계산해야


        //stockBalance
        stockBalanceValue = currentOwningValue; // stockBalanceValue의 값을 저장해서 다시 Sell()실행할 때 반영해야 
        
        //회사 주식 1주당 가격인  companyStockPriceValue X 구매수량 = owningAmountValue(보유한 주식금액)이 총 구매금액이며,
        AmountOfBuyStocksValue = companyStockPriceValue * owningValue;

        //현재 주식 보유량인 current accountBanlanceValue = accountBanlanceValue  - AmountOfBuyStocksValue(보유한 주식금액) 이 된다.
        currentAccountBanlanceValue = accountBalanceValue - AmountOfBuyStocksValue;

        accountBalanceValue = currentAccountBanlanceValue;
        accountBalanceText.text = accountBalanceValue.ToString();
        stockBalanceText.text = stockBalanceValue.ToString();

        //Debug.Log(accountBalanceValue);
    }




    public void sell()
    {
        Debug.Log("Sell");

        //주식을 ㅍ면, owning이 하나씩 카운트 -- 하면 된다.
        currentOwningValue = owningValue--;

        //회사 주식 1주당 가격인  companyStockPriceValue X 구매수량 = owningAmountValue(보유한 주식금액)이 총 구매금액이며,
        AmountOfBuyStocksValue = companyStockPriceValue * owningValue;

        //현재 주식 보유량인 current accountBanlanceValue = accountBanlanceValue  - AmountOfBuyStocksValue(보유한 주식금액) 이 된다.
        currentAccountBanlanceValue = accountBalanceValue - AmountOfBuyStocksValue;

        accountBalanceValue = currentAccountBanlanceValue;
        accountBalanceText.text = accountBalanceValue.ToString();
        stockBalanceText.text = stockBalanceValue.ToString();

        //Debug.Log(accountBalanceValue);

    }
}
