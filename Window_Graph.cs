using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System.Linq;
using System;

public class Window_Graph : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    public float destroyTime = 0.5f;



    public void Awake()
    {

        //Debug.Log("Start Coroutine 01");
        //코루틴을 사용해서 데이터를 반복생성, 그래프그리기,

        //삭제 해보자
        


        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        CreateCircle(new Vector2(200, 200));

        List<int> valueList; //valueList라는 이름으로 정수형 리스트를 선언하고
        valueList = new List<int>(); //리스트로 공간을 만들고

        int[] RandList = GetRandomInt(20, -30, 30); //주식값 생성함수 활용하여 20개를 생성한 후 

        valueList = RandList.OfType<int>().ToList(); //정수형으로 리스트를 만들엇 valueList에 넣는다.

        //그래프 그리기
        ShowGraph(valueList);

        // 1초 딜레이하/

        StartCoroutine("GenerateStockValue", valueList); // 새로운 데이터 생성하고 그래프 그리기

        //CreateCircle(new Vector2(200, 200));



        // 리스트에 데이터를 반복적, 랜덤, 이전값을 참조해서 30%의 변화폭으로 생성,
        // 변화폭을 0~+-30%로 고정하고, 시키고 동시에 처음의 리스트값을 디스트로이한다. 리스트의 데이터양은 50개로만 한정한다.
        // 추가로 뉴스를 제공해서 생성되는 리스트값을 + - 방향으로 가중치를 줘서 변화폭에 영향을 준다. 
        // 가중치를 주는 시간(기간)은 뉴스에서 설정한 값을 참조한다. 
        // 값을 랜덤값으로 생성시키면 일정한 높낮이의 패턴이 형성되기때문에, 일정한 방향성을 만들어내는 AI를 넣어보자.
        // 만약 상승이면 0~5회이상이면 하강으로 선택하는데 최대한 폭이 불규칙속의 규칙적인 파동을 만들어보자. 중복숫자를 제거하면 됨. ㅎ


        //List<int> valueList = new List<int>() { 5, 43, 65, 34, 54, 12, 52, 60, 76, 54, 23, 76, 23 };


        /*
           List<int> valueList; //valueList라는 이름으로 정수형 리스트를 선언하고
           valueList = new List<int>(); //리스트로 공간을 만들고
           int PreListValue = Random.Range(0, 100);  // 초기값은 랜덤으로 설정, 0~100 사이에서 랜덤으로 시작!


           for (int i = 0; i < 50; i++)
           {
               Debug.Log("add list");
               int minusMovement = Random.Range(0, -30);
               int plusMovement = Random.Range(0, 30);
               int x = PreListValue - minusMovement; // preListValue는 이전값을 참조하겠다는 의미
               int y = PreListValue + plusMovement;

               int newListValue = Random.Range(x, y); //  수를 랜덤으로 생성한다. 단, 기존의 생성된 리스트값을 참조
               valueList.Add(newListValue); // 추가한다. 
           }
        */

        /*
        List<int> valueList; //valueList라는 이름으로 정수형 리스트를 선언하고
        valueList = new List<int>(); //리스트로 공간을 만들고

        int[] RandList = GetRandomInt(20, -30, 30); //주식값 생성함수 활용하여 20개를 생성한 후 

        valueList = RandList.OfType<int>().ToList(); //정수형으로 리스트를 만들엇 valueList에 넣는다.
        */


        //그래프 그리기
        //ShowGraph(valueList);

        //리스트 생성값 확인하기
        /*
        foreach (int value in valueList)
        {
            Debug.Log(value);
        }
        */

        //이제 값을 리스트의 마지막부분에 추가하고, 동시에 맨 첫번째 리스트값은 삭제한다.
        //마지막에 추가한 값을 가지로 주식가격에 반영한다. 이건 계산필요.

        //AddNewValue(valueList); // 새로운 값이 추가되어 반영됨. !!!!FixedUpdata 코드에서 사용하였음
        //DeleteFirstListValue(valueList);//  !!!!FixedUpdata 코드에서 사용하였음
        //Run(valueList); // 데이터 생성, 그래프 출력, 삭제 반복실행

    }

   IEnumerator GenerateStockValue(List<int> valueList)
    {


        //Debug.Log("Start Coroutine 02");


        while (true)
        {
            // 데이터 생성, 그래프 출력, 삭제 반복실행
            Run(valueList);

            yield return new WaitForSeconds(1.0f);
        }

    }


    //이건 이미 생성된 20개의 값을 가져오고, 그 값에 새로운 생성값을, 맨 뒤에 추가하는 메소드임
    public void AddNewValue(List<int> valueList)
    {
        List<int> preValueList = new List<int>(); //prevalueList라는 이름으로 정수형 리스트를 선언하고
        //앞에서 생성된 리스트값은 valueList이며 이것을 preValueList에 담는다.
        //preValueList = valueList; // 다른 메소드에 생성된 값 가져오기. 어떻게 가져올까나???
        List<int> postValueList = new List<int>(); //postvalueList라는 이름으로 정수형 리스트를 선언하고

        int[] AddNewList = GetRandomInt(1, -30, 30); //추가할 리스트값을 하나 생성하고


        /*새로 생성한 값을 가지로 주식투자결과를 연산할 것
        매수금액 x 등락비율(%) = 평가금액
        예수금 - 투자금액 = 잔액
        판매금액
        잔액 = 판매금액 +- 매수금ㅇ
        */

        postValueList = AddNewList.OfType<int>().ToList();
        //추가되는 주식값을 추출, 메소드(이전값을 참조해서 랜덤으로 값 추출 - GetRandomInt 메소드 재활용)를 만들어서 추가할 것
        //Debug.Log("added new value");
        valueList.AddRange(postValueList); // 생성한 값을 preValueList 리스트의 마지막 값으로 추가한다.

       //////// Invoke("Run", 3.0f); // 반복적으로 함수

    }


    //첫번째 리스트의 값을 지운다.
    public void DeleteFirstListValue(List<int> valueList)
    {
        valueList.RemoveAt(0);//첫번째 값을 삭제
        //Debug.Log("deleted the first value");
    }




    //규칙적인 시간 간격으로 실행되는 함수로  awake 함수가 실행된 후 이것이 계속 실행되어 값을 생성, 그리고 그래프를 다시 그린다.
    public void Run(List<int> valueList)
    {

        //Debug.Log("Run");

        // --- 그래프 지우기

        //그래프를 지우자. 그런데 이러면 다 지워지는뎅,수정 전 리스트값을 지우고
        //DeleteGraph(valueList);
       // ForceUpdateRectTransforms();
        //Destroy(gameObject.GetComponent<Renderer>()); //???????????????????어떻게 그래프를 지울 수 있을까??
        //Debug.Log("delete graphContainer");
        //그래프를 다시 그라묜 아무것도 안그려질거임
        //ShowGraph(valueList);

        //----- 그래프 지우기

        //그리고 다시 그리면됨새로운 값으로...즉, 지우기전 리스트값을 기억해


        AddNewValue(valueList); // 새로운 값이 추가되어 반영됨
        DeleteFirstListValue(valueList); // 첫 데이터 삭제
        //두번째 그래프 다시 그림
        ShowGraph(valueList);
        


    }



    //???????????   그래프 삭제가 문제.... !!!!!!!  이것만 하면 됨 ㅎㅎㅎㅎㅎ



    //그래프를 삭제한다. 오브젝트를 Destory하고 다시 새로운 값으로 생성한다면? >>>>>>>>>>>> 이건 사용하지 않/
    IEnumerator DeleteGraph()
    {

        Debug.Log("Start Coroutine 3, delete pre graph");

        Destroy(GameObject.FindWithTag("dots"));
        Destroy(GameObject.FindWithTag("lines"));


        //GameObject[] objdot = GameObject.FindGameObjectsWithTag("dots");

        //for (int j = 0; j < 21; j++)
        //{
        //    foreach (GameObject obdot in objdot)
        //    {
        //        Destroy(obdot, destroyTime2);
        //    }

        //}

        //GameObject[] obj = GameObject.FindGameObjectsWithTag("lines");

        //for (int k = 0; k < 21; k++)
        //{
        //    foreach (GameObject ob in obj)
        //    {
        //        Destroy(ob, destroyTime2);
        //    }

        //}

        yield return new WaitForSeconds(1.0f);
    }








    // 랜덤으로 값을 생성, 중복된 값은 생성하지 않음, 그래야 주식마켓의 이상적인 그래프가 생성됨
    public int[] GetRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                randArray[i] = UnityEngine.Random.Range(min, max);
                isSame = false;
                
                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        
        return randArray;
    }






    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.tag = "dots";
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 50f;

        GameObject lastCircleGameObject = null;
        GameObject dotConnection = null;

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize +30;
            float yPosition = (valueList[i] / yMaximum) * graphHeight+180;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));

            if (lastCircleGameObject != null)
            {
                //ㄹㅏ인 그리는것은 임시로 지
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                
            }
            else
            {
                Destroy(dotConnection, destroyTime); // 라인이 모두 생성되어  lastDotConnection의 값이 생성되면,  생성했던 dotconnection을 삭제한다.
            }


            lastCircleGameObject = circleGameObject;
            




            Destroy(circleGameObject, destroyTime);//삭제한다. 4초후에 말이지...ㅎ

            Debug.Log("showGraph");


            

            
            //Destroy(GameObject.FindWithTag("ddddsasasaslines"), destroyTime);///////////////>>>>>>>>> 라인을 없앨 수 있는가?


            //if (circleGameObject == null)
            //{

            //Destroy(GameObject.FindWithTag("lines")); //20개만 사라ㅈ
            //} 


            //if (circleGameObject == null)
            //{
            //    Debug.Log("delete lines");
            //    Destroy(GameObject.FindWithTag("lines"));
            //}


            /*
            GameObject[] obj = GameObject.FindGameObjectsWithTag("lines");
            Debug.Log("delete lines");
            for (int k = 0 ;  k < 10 ; k++)
            {
                
                foreach (GameObject ob in obj)
                {
                    
                    Destroy(ob, destroyTime2);
                }
                
            }
            */

            // StartCoroutine("DeleteGraph");
            // Destroy(GameObject.FindWithTag("dots"));


        }
    }



    private void CreateDotConnection(Vector2 dotPositonA, Vector2 dotPositionB ) // 이 메소드로 그린 라인을 다시 없애야함, 닷이 사라질 때 같이 없애야 bool m_isDestroy = false;
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.tag = "lines";
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        Vector2 dir = (dotPositionB - dotPositonA).normalized;
        float distance = Vector2.Distance(dotPositonA, dotPositionB);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositonA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        Destroy(gameObject, destroyTime);
        
    }

}

