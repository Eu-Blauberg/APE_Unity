using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEvent : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] ItemDataBase itemDataBase;
    [SerializeField] private GameObject damageEffectPanel;
    [SerializeField] private Text noticeText;

    private Sprite damageEffectSprite;
    private Sprite NullSprite;
    private ControlGameDisplay controlGameDisplay;
    private bool isDamageTime = false; //ダメージを受けるかどうか
    private static float damageTime = 3.0f; //ダメージを受ける感覚

    void Start()
    {
        controlGameDisplay = GameObject.FindWithTag("GameDisplayMaster").GetComponent<ControlGameDisplay>();
        damageEffectSprite = Resources.Load<Sprite>("DamageSprite");
        NullSprite = Resources.Load<Sprite>("NullSprite");

        damageEffectPanel.GetComponent<Image>().sprite = NullSprite;

        //BGM再生
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Main);
    }

    //敵と衝突した時の処理
    private void OnTriggerEnter(Collider collider)
    {    
        //ダメージを受ける感覚がある場合は処理を終了
        if (isDamageTime) return;

        //衝突したオブジェクトが敵でない場合は処理を終了
        if (!collider.gameObject.CompareTag("Enemy")) return;


        if(itemDataBase.GetItemNum(itemDataBase.GetItemByName("ガコンの首飾り")) > 0)
        {
            //シールドを持っている場合
            //シールドを削除
            itemDataBase.RemoveItem(itemDataBase.GetItemByName("ガコンの首飾り"));
            //インベントリUIを更新
            controlGameDisplay.UpdateInventoryUI();

            //テキスト表示
            StartCoroutine(DisplayNoticeText("ガコンの首飾りを使った"));

            //ダメージを再度受けるまでの間隔を設定
            StartCoroutine(DamageInterval());
            return;
        }
        else{
            //プレイヤーのライフを減らす
            playerData.life --;

            //ライフUIを更新
            controlGameDisplay.UpdateLifeUI();

            //ダメージ
            SoundManager.Instance.PlaySE(SESoundData.SE.Damage);
            StartCoroutine(DisplayDamageEffect());

            //ダメージを再度受けるまでの間隔を設定
            StartCoroutine(DamageInterval());

            //プレイヤーのライフが0以下の場合
            if (playerData.life <= 0)
            {
                //シーン遷移
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    private IEnumerator DamageInterval()
    {
        isDamageTime = true;
        yield return new WaitForSeconds(damageTime);
        isDamageTime = false;
    }

    //ダメージエフェクトの表示
    public IEnumerator DisplayDamageEffect()
    {
        damageEffectPanel.GetComponent<Image>().sprite = damageEffectSprite;

        //徐々に透明にする
        var _color = damageEffectPanel.GetComponent<Image>().color;
        _color.a = 1.0f;
        damageEffectPanel.GetComponent<Image>().color = _color;
        for (float i = 1.0f; i >= 0; i -= 0.05f)
        {
            _color.a = i;
            damageEffectPanel.GetComponent<Image>().color = _color;
            yield return new WaitForSeconds(0.1f);
        }

        damageEffectPanel.GetComponent<Image>().sprite = NullSprite;
    }

    //アイテムを使った時のテキスト表示
    public IEnumerator DisplayNoticeText(string message)
    {
        noticeText.text = message;
        yield return new WaitForSeconds(3.0f);
        noticeText.text = "";
    }

}
