using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCombo : PopupScale
{
    public Image combo_img;
    public List<Sprite> L_SpriteCombo;

    private void OnEnable()
    {
        StartCoroutine(IE_DelayDeActivePopup());
    }

    public void InitCombo(int IndexCombo)
    {
        if (IndexCombo == 1)
        {
            combo_img.sprite = L_SpriteCombo[0];
            GameManager.ins.YourScore += 1;
            ScoreMove ComboScore = GameManager.ins.uiController.UigamePlay.ScoreCombo_txt.GetComponent<ScoreMove>();

            ComboScore.InitScore(1);
            ComboScore.AnimCoin();
            ComboScore.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 50, 0);

            GameManager.ins.uiController.UigamePlay.ScoreCombo_txt.gameObject.SetActive(true);
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundGood);
        }
        else if (IndexCombo == 2)
        {
            combo_img.sprite = L_SpriteCombo[1];
            GameManager.ins.YourScore += 3;
            ScoreMove ComboScore = GameManager.ins.uiController.UigamePlay.ScoreCombo_txt.GetComponent<ScoreMove>();

            ComboScore.InitScore(3);
            ComboScore.AnimCoin();
            ComboScore.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 50, 0);


            GameManager.ins.uiController.UigamePlay.ScoreCombo_txt.gameObject.SetActive(true);
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundGreat);
        }
        else
        {
            combo_img.sprite = L_SpriteCombo[2];
            GameManager.ins.YourScore += 5;

            ScoreMove ComboScore = GameManager.ins.uiController.UigamePlay.ScoreCombo_txt.GetComponent<ScoreMove>();
              
            ComboScore.InitScore(5);
            ComboScore.AnimCoin();
            ComboScore.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 50, 0);
            GameManager.ins.uiController.UigamePlay.ScoreCombo_txt.gameObject.SetActive(true);
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundPerfect);
        }
    }

    IEnumerator IE_DelayDeActivePopup()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine(IE_DelayDeActivePopup());
    }
}
