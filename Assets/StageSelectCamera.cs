using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCamera : MonoBehaviour
{
    //�v���C���[�̃I�u�W�F��ǐ�
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //�v���C���[�ǐՌ^�̃X�N���[��
        //�J�����ƃv���C���[�̈ʒu���擾
        var playerPosition = player.transform.position;
        var position = transform.position;

        //�����ŃJ�����Ƀv���C���[�̍��W����
        position.x = playerPosition.x;
        position.y = playerPosition.y + 2;
        position.z = playerPosition.z - 15;
        transform.position = position;

    }
}
