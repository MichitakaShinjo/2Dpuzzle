using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdscript : MonoBehaviour
{
    //鳥のプレハブを格納する配列
    public GameObject[] birdPrefabs;


    //連鎖を消す最小数
    [SerializeField]
    private float removeBirdMinCount = 3;

    [SerializeField]
    ScoreManager scoreManager;

    //連鎖判定用の距離  const=そのあとの数値を変更できなくする（定数化）
    [SerializeField]
    const float birdDistance = 1.2f;

    //クリックされた鳥を格納
    private GameObject firstBird;
    private GameObject lastBird;
    private string currentName;
    List<GameObject> removableBirdList = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        TouchMnager.Began += (info) =>
        {
            //クリック地点でヒットしているオブジェクトを取得
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;
                //ヒットしたオブジェクトのTagを判断して初期化
                if(hitObj.tag == "Bird")
                {
                    firstBird = hitObj;
                    lastBird = hitObj;
                    currentName = hitObj.name;
                    removableBirdList = new List<GameObject>();
                    PushToBirdList(hitObj);

                }
            }
        };

        TouchMnager.Moved += (info) =>
        {
            if (!firstBird) { return; }
                //クリック地点でヒットしているオブジェクトを取得
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint), Vector2.zero);
            if (hit.collider) 
            { 
                GameObject hitObj = hit.collider.gameObject;
                //ヒットしたオブジェクトのTagが鳥、尚且つ名前が一緒、尚且つ最後にHitしたオブジェクトと違う、尚且つリストに格納されていない
                if (hitObj.tag == "Bird" && hitObj.name == currentName && hitObj != lastBird && 0 > removableBirdList.IndexOf(hitObj))
                {
                    //距離を測り、つなげるか判断
                    float distance = Vector2.Distance(hitObj.transform.position, lastBird.transform.position);
                    if (distance > birdDistance)
                    {
                        return;
                    }

                    lastBird = hitObj;
                    PushToBirdList(hitObj);
                }
            }
            
        };
        TouchMnager.Ended += (info) =>
        {
            //リストの格納数を取り出し、最小数と比較
            int removeCount = removableBirdList.Count;
            if (removeCount >= removeBirdMinCount)
            {
                //最小数以上の連鎖を消し、その分補充
                foreach (GameObject obj in removableBirdList)
                {
                    //最小数以上の連鎖を消し
                    Destroy(obj);
                }
                //その分補充
                StartCoroutine(DropBirds(removeCount));
                scoreManager.AddScore((int)Mathf.Pow(2, removeCount));
            }

            foreach (GameObject obj in removableBirdList)
            {
                ChangeColor(obj, 1.0f);
            }
            removableBirdList = new List<GameObject>();
            firstBird = null;
            lastBird = null;
        };
        StartCoroutine(DropBirds(65));
    }

    private void PushToBirdList(GameObject obj)
    {
        removableBirdList.Add(obj);
        ChangeColor(obj, 0.5f);
    }

    private void ChangeColor(GameObject obj, float transparency)
    {
        SpriteRenderer rendere = obj.GetComponent<SpriteRenderer>();
        rendere.color = new Color(rendere.color.r,
                                  rendere.color.g,
                                  rendere.color.b, transparency);
    }

    IEnumerator DropBirds(int count)  //コルーチンの中身
    {
        for (int i = 0; i < count; i++) 
        {
            //ランダムで出現位置を作成
            Vector2 pos = new Vector2(Random.Range(-8.5f, 8.5f), 9.0f);

            //ランダムで鳥を出現させてIDを格納 Length=格納された個数を確認できる
            int id = Random.Range(0,birdPrefabs.Length);

            //鳥を発生させる
            GameObject bird = (GameObject)Instantiate(birdPrefabs[id],pos,
                 Quaternion.AngleAxis(Random.Range(-40,40),Vector3.forward));

            //作成した鳥の名前を変更
            bird.name = "Bird" + id;

            //0.05秒待って対の処理へ yield return=待つ
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
