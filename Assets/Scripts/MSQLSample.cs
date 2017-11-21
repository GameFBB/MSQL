using MSQL;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// Unity ⇔ PHP ⇔ MySQL
/// 
/// Table : Score
/// 
/// Field : ObjectId, UserName, Score, Age
/// 
/// </summary>

public class MSQLSample : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        StartCoroutine(InsertMethod());
    }

    //1.Insert
    private IEnumerator InsertMethod()
    {
        //Bobby（Score:600, Age:10)
        MSQLObject obj = new MSQLObject("Score");
        obj.Add("UserName", "Bobby");
        obj.Add("Score", "600");
        obj.Add("Age", "10");
        yield return obj.SaveAync();
    }

    //2.Update
    private IEnumerator UpdateMethod()
    {
        //Bobby(Score:1500)
        MSQLObject obj = new MSQLObject("Score", "UserName", "Bobby");
        obj.Add("Score", "1500");
        yield return obj.SaveAync();
    }

    //3.Delete
    private IEnumerator DeleteMethod()
    {
        //Bobby's record.
        MSQLObject obj = new MSQLObject("Score", "UserName", "Bobby");
        yield return obj.DeleteAync();
    }

    //4.Find
    private IEnumerator FindMethod()
    {
        //TOP3 (Age >= 40)
        MSQLQuery query = new MSQLQuery("Score");
        query.Select("UserName", "Score");
        query.Where("Age", ">=", "40");    //bool("true" or "false")
        query.OrderBy("Score", "DESC");    //"ASC" or "DESC"
        query.Limit("3");
        yield return query.FindAync();

        if (query.Result != null)
        {
            Debug.Log("Count : " + query.Result.Count);

            foreach (IDictionary data in query.Result)
            {
                Debug.Log("UserName：" + data["UserName"]);
                Debug.Log("Score：" + data["Score"]);
            }
        }
        else
        {
            Debug.Log("no data");
        }
    }

    //5.Count
    private IEnumerator CountMethod()
    {
        //Score >= 300, Age < 40
        MSQLQuery query = new MSQLQuery("Score");
        query.Where("Score", ">=", "300");
        query.Where("Age", "<", "40");
        yield return query.CountAync();

        Debug.Log("Count : " + query.Count);
    }
}