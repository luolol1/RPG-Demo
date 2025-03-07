using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;//�Ӳ�ЧӦ

    private float xPosition;
    void Start()
    {
        cam = GameObject.Find("Virtual Camera");

        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float DistanceMove = cam.transform.position.x * parallaxEffect;//����ͷ������player����cam.transform.position.x�Ǻ�player�����λ��
        transform.position = new Vector3(xPosition + DistanceMove, transform.position.y);
    }
}
