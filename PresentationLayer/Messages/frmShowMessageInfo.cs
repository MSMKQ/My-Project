using PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Messages
{
    public partial class frmShowMessageInfo : Form
    {
        private int _FormPositionX;
        private int _FormPositionY;

        public enum EnMood { Error , Successfully }
        private EnMood _Mood;
        private string _Title;
        private string _Message;

        public frmShowMessageInfo(EnMood enMood, string Title, string Message)
        {
            InitializeComponent();

            _Mood = enMood;
            _Title = Title;
            _Message = Message;
        }

        private void frmShowMessageInfo_Load(object sender, EventArgs e)
        {
            Position();

            lblTitle.Text = _Title;
            lblMessage.Text = _Message;

            switch (_Mood)
            {
                case EnMood.Error:
                    {
                        panel1.BackColor = Color.Red;
                        pbImage.Image = Resources.cancel_Essentials_32;
                    }
                    break;

                case EnMood.Successfully:
                    {
                        panel1.BackColor = Color.Green;
                        pbImage.Image = Resources.check_Color_bold_style_32;
                    }
                    break;
            }
        }

        private void Position()
        {
            int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            _FormPositionX = ScreenWidth - Width - 10;
            _FormPositionY = ScreenHeight - Height - 10;

            Location = new Point(_FormPositionX, _FormPositionY);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
