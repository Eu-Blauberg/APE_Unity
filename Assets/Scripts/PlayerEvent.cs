using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEvent : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] private GameObject damageEffectPanel;

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
    }

    //敵と衝突した時の処理
    private void OnCollisionEnter(Collision collision)
    {    
        //ダメージを受ける感覚がある場合は処理を終了
        if (isDamageTime) return;

        //衝突したオブジェクトが敵でない場合は処理を終了
        if (!collision.gameObject.CompareTag("Enemy")) return;

        //プレイヤーのライフを減らす
        playerData.life --;

        //ライフUIを更新
        controlGameDisplay.UpdateLifeUI();

        //ダメージ
        StartCoroutine(DisplayDamageEffect());

        //ダメージを再度受けるまでの間隔を設定
        StartCoroutine(DamageInterval());

        //プレイヤーのライフが0以下の場合
        if (playerData.life <= 0)
        {
            /*
            ゲームオーバー処理
            */
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
}
