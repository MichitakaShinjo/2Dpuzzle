using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombscript : MonoBehaviour
{
    //爆弾のプレハブを格納する配列
    public GameObject[] bombPrefabs;

    //連鎖を消す最小数
    [SerializeField]
    private float removeBombMinCount = 3;

    //連鎖判定用の距離  const=そのあとの数値を変更できなくする（定数化）
    [SerializeField]
    const float bombDistance = 1.2f;

    //クリックされた鳥を格納
    private GameObject firstBomb;
    private GameObject lastBomb;
    private string currentName;
    List<GameObject> removableBombList = new List<GameObject>();
    List<GameObject> destroyBombList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        TouchMnager.Began += (info) =>
        {
            //クリック地点でヒットしているオブジェクトを取得
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint), Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;
                //ヒットしたオブジェクトのTagを判断して初期化
                if (hitObj.tag == "Bomb")
                {
                    firstBomb = hitObj;
                    lastBomb = hitObj;
                    currentName = hitObj.name;
                    removableBombList = new List<GameObject>();
                    PushToBombList(hitObj);

                }
            }
        };

        TouchMnager.Moved += (info) =>
        {
            if (!firstBomb) { return; }
            //クリック地点でヒットしているオブジェクトを取得
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint), Vector2.zero);
            if (hit.collider)
            {
                GameObject hitObj = hit.collider.gameObject;
                //ヒットしたオブジェクトのTagが鳥、尚且つ名前が一緒、尚且つ最後にHitしたオブジェクトと違う、尚且つリストに格納されていない
                if (hitObj.tag == "Bomb" && hitObj.name == currentName && hitObj != lastBomb && 0 > removableBombList.IndexOf(hitObj))
                {
                    //距離を測り、つなげるか判断
                    float distance = Vector2.Distance(hitObj.transform.position, lastBomb.transform.position);
                    if (distance > bombDistance)
                    {
                        return;
                    }

                    lastBomb = hitObj;
                    PushToBombList(hitObj);
                }
            }

        };
        TouchMnager.Ended += (info) =>
        {
            //リストの格納数を取り出し、最小数と比較
            int removeCount = removableBombList.Count;
            if (removeCount >= removeBombMinCount)
            {
                //最小数以上の連鎖を消し、その分補充
                foreach (GameObject obj in removableBombList)
                {
                    //最小数以上の連鎖を消し
                    Destroy(obj);
                }
                //その分補充
                StartCoroutine(DropBomb(removeCount));
            }

            foreach (GameObject obj in removableBombList)
            {
                ChangeColor(obj, 1.0f);
            }
            removableBombList = new List<GameObject>();
            firstBomb = null;
            lastBomb = null;
        };
        StartCoroutine(DropBomb(20));
    }
    private void PushToBombList(GameObject obj)
    {
        removableBombList.Add(obj);
        ChangeColor(obj, 0.5f);
    }

    private void ChangeColor(GameObject obj, float transparency)
    {
        SpriteRenderer rendere = obj.GetComponent<SpriteRenderer>();
        rendere.color = new Color(rendere.color.r,
                                  rendere.color.g,
                                  rendere.color.b, transparency);
    }

    IEnumerator DropBomb(int count)  //コルーチンの中身
    {
        for (int i = 0; i < count; i++)
        {
            //ランダムで出現位置を作成
            Vector2 pos = new Vector2(Random.Range(-7.0f, 7.0f), 15.0f);

            //ランダムで鳥を出現させてIDを格納 Length=格納された個数を確認できる
            int id = Random.Range(0, bombPrefabs.Length);

            //鳥を発生させる
            GameObject bomb = (GameObject)Instantiate(bombPrefabs[id], pos,
                 Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward));

            //作成した鳥の名前を変更
            bomb.name = "Bomb" + id;

            //0.05秒待って対の処理へ yield return=待つ
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Bomb" && transform.position.y <= -10f)
        {
            Destroy(gameObject);
            StartCoroutine(DropBomb(1));
        }
    }
}
