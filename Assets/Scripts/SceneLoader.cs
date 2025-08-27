using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���Ǘ��@�\���g�����߂ɕK�v

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// �w�肳�ꂽ���O�̃V�[�������[�h���܂��B
    /// ���̃��\�b�h��UI�{�^����OnClick�C�x���g����Ăяo�����Ƃ�z�肵�Ă��܂��B
    /// </summary>
    /// <param name="sceneName">���[�h����V�[���̖��O</param>
    public void LoadScene(string sceneName)
    {
        // SceneManager.LoadScene() ���\�b�h���g���ăV�[�������[�h���܂��B
        SceneManager.LoadScene(sceneName);

        // �f�o�b�O���O���o�́i�C�Ӂj
        Debug.Log($"�V�[�������[�h���܂���: {sceneName}");
    }
}