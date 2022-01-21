using UnityEngine;
using UnityEngine.UI;
using Framework;
using TMPro;

namespace VRTKExtensions
{
    public class PosFollowTooltipPanel : MonoBehaviour, ISpawnAbleTooltip
    {
        private bool textValInvalid;

        private string textTip;

        public Transform FollowTarget { set; get; }

        public Vector3? FollowPos { set; get; }

        public string Key { get; set; }

        public Vector3 AdjuestPosOffset { set; get; }
         
        public string TextTip
        {
            set
            {
                if (textTip == value)
                    return;

                textTip = value;

                textValInvalid = true;
            }
        }

        public Canvas planeCanvas;

        public Text text;

        public TextMeshProUGUI textPro;

        [Tooltip("固定高度,宽度根据文字的多少自动适配 ")]
        [Range(0.05f, 0.3f)]
        public float fixViewSizeH;

        [Tooltip("是否根据观察距离刷新尺寸")]
        public bool autoAdjuestSize;
         
        public CanvasGroup canvasGroup;

        protected Camera RenderCam => VRTKHelper.HeadSetCamera != null ? VRTKHelper.HeadSetCamera : Camera.main;

        public virtual void OnDespawn()
        {
            FollowTarget = null;

            FollowPos = null;

            AdjuestPosOffset = Vector3.zero;

            gameObject.SetActive(false);
        }

        public virtual void OnSpawn()
        {
            gameObject.SetActive(true);
  
            canvasGroup.alpha = 0.0f;
        }

        protected void LookupCam()
        {
            if (RenderCam == null)
                return;

            var dir = RenderCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1.0f)) - RenderCam.transform.position;

            dir.Normalize();

            transform.forward = dir;
        }

        protected virtual void AdjuestSize()
        {
            if (RenderCam == null)
                return;

            // renderCam.projectionMatrix.m11 = near * (h / 2)
            var depth = RenderCam.WorldToViewportPoint(transform.position).z;

            // var w = depth / renderCam.projectionMatrix.m11 * renderCam.aspect * fixViewSizeW;
            var h = depth / RenderCam.projectionMatrix.m11 * fixViewSizeH;

            transform.localScale = new Vector3(h, h, 1.0f);
        }

        protected virtual void FollowTargetPos()
        {
            var targetPos = FollowPos == null ? FollowTarget.position : FollowPos.Value;

            transform.position = targetPos + AdjuestPosOffset;
        }

        protected virtual void LateUpdate()
        {
            if(textValInvalid)
            {
                textValInvalid = true;

                if (text != null)
                    text.text = textTip;
                else
                    textPro.text = textTip;

                LayoutRebuilder.ForceRebuildLayoutImmediate(planeCanvas.transform as RectTransform);
            }

            //待观察效果
            canvasGroup.alpha = 1;
             
            if (FollowPos != null || FollowTarget != null)
                FollowTargetPos();
             
            LookupCam();

            if (autoAdjuestSize)
                AdjuestSize();
        }
    }
}



