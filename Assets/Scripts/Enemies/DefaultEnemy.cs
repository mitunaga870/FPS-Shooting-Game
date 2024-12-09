using System;
using AClass;
using lib;
using UnityEngine;

namespace Enemies
{
    public class DefaultEnemy : AEnemy
    {
        private const float ZeroHealthOffset = -1.2f;
        
        private int _prevTime;
        
        private float _currentAnimationTime;
        
        private float _stopAnimationTime;
        
        private Action _onEndAnimation;
        
        private bool _invokedEndAnimation;
        
        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private TextMeshProGeometryAnimator textMeshProGeometryAnimator;
        
        [SerializeField]
        private TMPro.TextMeshProUGUI damageText;
        
        [SerializeField]
        private GameObject hpBar;

        private new void FixedUpdate()
        {
            // 基底クラスのFixedUpdateを呼び出す
            base.FixedUpdate();
            
            // HPバーの位置を設定
            hpBar.transform.localPosition = new Vector3(ZeroHealthOffset/MaxHP * HP - ZeroHealthOffset, 0, 0);
            
            // ゲーム時刻取得
            var current = SceneController.GameTime;
            // 経過時間
            var timeDelta = current - _prevTime;
            
            _currentAnimationTime += timeDelta * 0.2f * 0.03f;

            _prevTime = current;
            
            // アニメーション終了時間が設定されている場合
            if (_stopAnimationTime > 0 && _currentAnimationTime >= _stopAnimationTime &&
                !_invokedEndAnimation) // 初回のみ実行
            {
                _onEndAnimation?.Invoke();
                _invokedEndAnimation = true;
                return;
            }
            
            if (_stopAnimationTime > 0 && _currentAnimationTime >= _stopAnimationTime && _invokedEndAnimation) // 2回目は返すだけ
                return;
            
            animator.Update(timeDelta * 0.2f * 0.03f);
        }

        protected override void PlayDeathAnimation(bool destroy = true)
        {
            // knockバックを再生
            animator.Play("KnockBack");
            
            // アニメーション時間をリセット
            _currentAnimationTime = 0;
            
            // 終了時間を設定
            _stopAnimationTime = 0.6f;
            
            // 終了時の処理を設定
            _onEndAnimation = () =>
            {
                // ゲームオブジェクトを破棄
                if (destroy)
                    Destroy(gameObject, 0.5f);
            
                _invokedEndAnimation = false;
            };
        }

        protected override void PlayKnockBackAnimation()
        {
            // knockバックを再生
            animator.Play("KnockBack");
            
            // アニメーション時間をリセット
            _currentAnimationTime = 0;
            
            // 終了時間を設定
            _stopAnimationTime = 9f / 24f;
        }

        protected override void PlayMoveAnimation()
        {
            animator.Play("Running");
            
            // アニメーション時間をリセット
            _currentAnimationTime = 0;
            
            // 終了時間を設定(終了なし)
            _stopAnimationTime = 0;
        }

        protected override void PlayKnockBackEndAnimation()
        {
            // knockバックを再生
            animator.Play("KnockBack");
            
            // 終了時間を設定
            _stopAnimationTime = 0;
        }

        protected override float GetHeight()
        {
            return 0.5f;
        }

        protected override void OnDamage(int damage)
        {
            // ダメージテキストを表示
            damageText.text = damage.ToString();
            textMeshProGeometryAnimator.gameObject.SetActive(true);
            
            // 指定時間後に非表示にする
            var disableCoroutine = General.DelayCoroutine(0.5f, () =>
            {
                textMeshProGeometryAnimator.gameObject.SetActive(false);
            });
            StartCoroutine(disableCoroutine);
        }
    }
}