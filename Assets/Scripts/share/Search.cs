
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BoxCastSearch.cs
/// クラス説明
/// BoxCastとBoxCastAllの結果を返す
///
/// 作成日: 9/4
/// 作成者: 湯元来輝
/// </summary>
public static class Search
{

    public static RaycastHit BoxCast(BoxCastStruct boxCastStruct)
    {

        //探索結果を格納
        RaycastHit hit = default;

        //Distanceを指定していない
        if (boxCastStruct._distance == 0)
        {

            //Distanceを指定している > LayerMaskを指定していない
            if (boxCastStruct._layerMask == 0)
            {

                Physics.BoxCast(boxCastStruct._originPos, boxCastStruct._size, boxCastStruct._direction, out hit, boxCastStruct._quaternion);

            }
            //Distanceを指定している > LayerMaskを指定している
            else
            {

                Physics.BoxCast(boxCastStruct._originPos, boxCastStruct._size, boxCastStruct._direction, out hit, boxCastStruct._quaternion, boxCastStruct._layerMask);

            }

        }
        //Distanceを指定している
        else
        {

            //Distanceを指定している > LayerMaskを指定していない
            if (boxCastStruct._layerMask == 0)
            {

                Physics.BoxCast(boxCastStruct._originPos, boxCastStruct._size, boxCastStruct._direction, out hit, boxCastStruct._quaternion, boxCastStruct._distance);

            }
            //Distanceを指定している > LayerMaskを指定している
            else
            {

                Physics.BoxCast(boxCastStruct._originPos, boxCastStruct._size, boxCastStruct._direction, out hit, boxCastStruct._quaternion, boxCastStruct._distance, boxCastStruct._layerMask);

            }

        }
        //探索して見つかったとき
        if (hit.collider && boxCastStruct._tags != null)
        {

            //求めているタグと合ったかの判定
            bool isMach = false;
            foreach(string tag in boxCastStruct._tags)
            {

                if (hit.collider.CompareTag(tag))
                {

                    isMach = true;

                }

            }
            if (!isMach)
            {

                hit = new RaycastHit();

            }

        }

        return hit;
    }

    public static RaycastHit[] BoxCastAll(BoxCastStruct boxCastStruct)
    {

        //探索結果を格納
        RaycastHit[] hits = default;
        //Distanceを指定していない
        if (boxCastStruct._distance == 0)
        {

            //Distanceを指定している > LayerMaskを指定していない
            if (boxCastStruct._layerMask == 0)
            {

                hits = Physics.BoxCastAll(boxCastStruct._originPos, boxCastStruct._size, boxCastStruct._direction, boxCastStruct._quaternion);

            }
            //Distanceを指定している > LayerMaskを指定している
            else
            {

                hits = Physics.BoxCastAll(boxCastStruct._originPos, boxCastStruct._size, boxCastStruct._direction, boxCastStruct._quaternion, boxCastStruct._layerMask);

            }

        }
        //Distanceを指定している
        else
        {

            //Distanceを指定している > LayerMaskを指定していない
            if (boxCastStruct._layerMask == 0)
            {

                hits = Physics.BoxCastAll(boxCastStruct._originPos, boxCastStruct._size, boxCastStruct._direction,boxCastStruct._quaternion, boxCastStruct._distance);

            }
            //Distanceを指定している > LayerMaskを指定している
            else
            {

                hits = Physics.BoxCastAll(boxCastStruct._originPos, boxCastStruct._size, boxCastStruct._direction, boxCastStruct._quaternion, boxCastStruct._distance, boxCastStruct._layerMask);

            }

        }

        //精査するためにリストにする
        List<RaycastHit> hitList = new List<RaycastHit>(hits);
        //探索して見つかったとき
        if (hitList.Count == 0 && boxCastStruct._tags != null)
        {

            //求めているタグと合ったかの判定
            bool isMach = false;
            /*タグで求められていないあたったものを配列から消す*/
            foreach (RaycastHit hit in hitList)
            {

                foreach (string tag in boxCastStruct._tags)
                {

                    if (hit.collider.CompareTag(tag))
                    {

                        isMach = true;

                    }
                    if (!isMach)
                    {

                        hitList.Remove(hit);

                    }

                }

            }
            //中身がないとき
            if(hitList.Count == 0)
            {

                return new RaycastHit[0];

            }
            //精査したリストを配列に戻す
            return hits = hitList.ToArray();

        }

        return hits;

    }

}