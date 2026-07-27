using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScrolling : MonoBehaviour
{
    // 배경 스크롤링 스크립트 => 배경을 이미지로 안한 이유는 간단하게 직접 만들고 싶어서
    #region Inspector

    public GameObject spriteObject; // 배경 오브젝트
    public float interval; // 배경 오브젝트 간격
    public float speed = 1f; // 스크롤 속도

    #endregion

    private List<GameObject> spriteObjects = new List<GameObject>();
    private int firstIndex = 1;

    private void Awake()
    {
        // 원본 이미지를 하나 더 복제한다.
        GameObject newSpriteObject = Instantiate<GameObject>(spriteObject);
        newSpriteObject.transform.SetParent(this.transform);
        spriteObjects.Add(spriteObject);
        spriteObjects.Add(newSpriteObject);
        SortImage();
    }

    /// <summary>
    /// 오브젝트 정렬
    /// </summary>
    private void SortImage()
    {
        for (int i = spriteObjects.Count - 1; i >= 0; i--)
        {
            GameObject spriteObject = spriteObjects[i];
            spriteObject.transform.localPosition = Vector3.left * interval * i;
        }
    }

    private void Update()
    {
        UpdateMoveImages();
    }

    /// <summary>
    /// 오브젝트 이동 업데이트
    /// </summary>
    private void UpdateMoveImages()
    {
        float move = Time.deltaTime * speed;
        for (int i = 0; i < spriteObjects.Count; i++)
        {
            GameObject spriteRenderer = spriteObjects[i];
            spriteRenderer.transform.localPosition += Vector3.right * move;

            if (spriteRenderer.transform.localPosition.x >= interval)
            {
                spriteRenderer.transform.localPosition = new Vector3(spriteObjects[firstIndex].transform.localPosition.x - interval, 0f, 0f);
                firstIndex = spriteObjects.IndexOf(spriteRenderer);
            }
        }
    }

}
