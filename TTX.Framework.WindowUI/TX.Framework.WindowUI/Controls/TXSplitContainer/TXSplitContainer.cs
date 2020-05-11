using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TX.Framework.WindowUI.Controls
{
    [ToolboxBitmap(typeof(System.Windows.Forms.SplitContainer))]   
    public class TXSplitContainer : System.Windows.Forms.SplitContainer
    {
        #region Fields

        /// <summary>
        /// 圆角值
        /// </summary>
        private int _CornerRadius = 8;

        /// <summary>
        /// 边框宽度
        /// </summary>
        private int _BorderWidth = 1;

        /// <summary>
        /// 边框颜色
        /// </summary>
        private Color _BorderColor = SkinManager.CurrentSkin.BorderColor;

        /// <summary>
        /// 背景色
        /// </summary>
        private Color _BackBeginColor = Color.White;

        private Color _BackEndColor = Color.White;

        #endregion

        public TXSplitContainer() : base()
        {
            SetStyle(ControlStyles.ResizeRedraw
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
            base.BackColor = Color.Transparent;
            ControlHelper.BindMouseMoveEvent(this);
        }

        #region Properties

        [Category("TXProperties")]
        [DefaultValue(8)]
        [Description("圆角值")]
        public int CornerRadius
        {
            get { return this._CornerRadius; }
            set
            {
                this._CornerRadius = value > 0 ? value : 0;
                base.Invalidate();
            }
        }

        [Category("TXProperties")]
        [DefaultValue(typeof(Color), "White")]
        [Description("背景色1")]
        public Color BackBeginColor
        {
            get { return this._BackBeginColor; }
            set
            {
                this._BackBeginColor = value;
                base.Invalidate();
            }
        }

        [Category("TXProperties")]
        [DefaultValue(typeof(Color), "White")]
        [Description("背景色2")]
        public Color BackEndColor
        {
            get { return this._BackEndColor; }
            set
            {
                this._BackEndColor = value;
                base.Invalidate();
            }
        }

        [Category("TXProperties")]
        [DefaultValue(1)]
        [Description("边框宽度，若为0则无边框")]
        public int BorderWidth
        {
            get { return this._BorderWidth; }
            set
            {
                this._BorderWidth = value > 0 ? value : 0;
                this.Invalidate();
            }
        }

        [Category("TXProperties")]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return this._BorderColor; }
            set
            {
                this._BorderColor = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public new BorderStyle BorderStyle { get; set; }


        #endregion

        #region Override methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int w = this.BorderWidth > 0 ? this.BorderWidth : 0;
            Graphics g = e.Graphics;
            //缓冲
            //BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            //BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            //Graphics g = myBuffer.Graphics;

            GDIHelper.InitializeGraphics(g);
            GradientColor color = new GradientColor(this._BackBeginColor, this._BackEndColor, null, null);
            Rectangle rect = new Rectangle(0, 0, this.Size.Width - 1, this.Size.Height - 1);
            RoundRectangle roundRect = new RoundRectangle(rect, new CornerRadius(this._CornerRadius));
            GDIHelper.FillRectangle(g, roundRect, color);
            if (this._BorderWidth > 0)
            {
                rect.X += this._BorderWidth - 1; rect.Y += this._BorderWidth - 1;
                rect.Width -= this._BorderWidth - 1; rect.Height -= this._BorderWidth - 1;

                GDIHelper.DrawPathBorder(g, new RoundRectangle(rect, new CornerRadius(this._CornerRadius)), this._BorderColor, this.BorderWidth);
                // 上容器的边框
                Rectangle rectPanel1 = new Rectangle(0, 0, this.Panel1.Width - 1, this.Panel1.Height - 1);
                GDIHelper.DrawPathBorder(g, new RoundRectangle(rectPanel1, new CornerRadius(0)), this._BorderColor, this.BorderWidth);                
            }

            //g.SmoothingMode = SmoothingMode.HighQuality;
            //g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            //myBuffer.Render(e.Graphics);
            //g.Dispose();
            //myBuffer.Dispose();//释放资源
        }
        /// <summary>
        /// 避免加载控件时，闪烁
        /// https://www.cnblogs.com/kest/p/4683012.html
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000; // Turn off WS_CLIPCHILDREN 
                return parms;
            }
        } 
        #endregion
       
    }
}
