using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombscript2 : MonoBehaviour
{
    //爆弾のプレハブを格納する配列
    public GameObject[] bombPrefabs;

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
                    Destroy(this.gameObject);
                }
            }
        };
        StartCoroutine(DropBomb(20));
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
        
    }
}
