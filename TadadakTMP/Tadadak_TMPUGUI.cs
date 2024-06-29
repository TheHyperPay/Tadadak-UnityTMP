using UnityEngine;
using UnityEngine.Serialization;

namespace TMPro
{
    public class Tadadak_TMPUGUI : TextMeshProUGUI
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetText();
        }
#endif
        [Tooltip("write below the textbox what you want to display on the UI.")]
        [TextArea(5, 10)]
        [SerializeField]
        [FormerlySerializedAs("_fullText")]
        private string _fullText;
        public string fullText
        {
            get => this._fullText;
            set
            {
                displayCounter = 0;
                this._fullText = value;
            }
        }

        [Tooltip("It's going to be printed out after processing the Full Text on the textbox.")]
        [TextArea(3, 10)]
        [SerializeField]
        [FormerlySerializedAs("_bePrintedText")]
        private string _bePrintedText;
        protected string bePrintedText
        {
            get => this._bePrintedText;
            set
            {
                this.SetText();
            }
        }

        [Tooltip("if you check, start to display text progressively.\nif you uncheck, display whole text immediately.")]
        [SerializeField]
        [FormerlySerializedAs("_progressivelyPrintable")]
        private bool _progressivelyPrintable;
        public bool progressivelyPrintable
        {
            get => this._progressivelyPrintable;
            set
            {
                this._progressivelyPrintable = value;
            }
        }

        [Tooltip("pause doing text printing")]
        [SerializeField]
        [FormerlySerializedAs("_displayPause")]
        private bool _displayPause = false;
        public bool displayPause
        {
            get => this._displayPause;
            set
            {
                this._displayPause = value;
            }
        }

        public const float maxDisplaySpeed = 4.0f;
        public const float minDisplaySpeed = 0.1f;
        [Tooltip("set speed about text printing.\nDefault = x1 (20 letters per sec)")]
        [Range(minDisplaySpeed, maxDisplaySpeed)]
        [SerializeField]
        [FormerlySerializedAs("_displaySpeed")]
        private float _displaySpeed = 1f;
        public float displaySpeed
        {
            get => this._displaySpeed;
            set
            {
                if (this._displaySpeed < minDisplaySpeed)
                    this._displaySpeed = minDisplaySpeed;
                else if (this._displaySpeed > maxDisplaySpeed)
                    this._displaySpeed = maxDisplaySpeed;
                else
                    this._displaySpeed = value;
            }
        }


        private void SetText()
        {
            this._bePrintedText = PrintTextLikeProgressivly(this._fullText);
            this.text = this._bePrintedText;

            if (this.m_havePropertiesChanged)
            {
                this.ForceMeshUpdate();
            }
        }


        private string PrintTextLikeProgressivly(string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (!this.progressivelyPrintable)
            {
                return this._fullText;
            }

            if(this.displayCounter<this._fullText.Length)
            {
                return str.Substring(0, (int)this.displayCounter);
            }
            return str;
        }


        private uint displayCounter = 0;
        private float criticalTime = 0f;
        private void Update()
        {
            this.SetText();

            if(!this.progressivelyPrintable)
                this.displayCounter = 0;
            else if(this.displayCounter>=this._fullText.Length)
                this.displayCounter = (uint)this._fullText.Length;
            else if(this._displayPause)
            {
                //Pass here owing not to add displayCounter.
            }
            else
            {
                criticalTime += Time.deltaTime * this._displaySpeed;
                if(criticalTime >= 0.05f)
                {
                    this.displayCounter += 1;
                    criticalTime = 0f;
                }
            }
        }
    }
}
