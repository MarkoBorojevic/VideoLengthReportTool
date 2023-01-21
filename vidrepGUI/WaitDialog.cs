using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vidrepGUI
{
    public partial class WaitDialog : Form
    {
        Task _task;

        public static WaitDialog instance { get; private set; }

        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public WaitDialog(string caption, string text, Action<CancellationToken> action)
        {
            InitializeComponent();

            instance = this;

            Text = caption;
            label1.Text = text;

            _task = Task.Factory.StartNew(async () => 
            {
                await Task.Run(() => action(tokenSource.Token), tokenSource.Token);

                BeginInvoke((Action)(() =>
                {
                    Finish();
                }));

            }, tokenSource.Token);
        }

        bool finished;

        void Finish()
        {
            finished = true;

            Close();
        }

        public void UpdateText(string text)
        {
            BeginInvoke((Action)(() => label1.Text = text));
        }

        private void ImportWait_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!finished) {
                if(MessageBox.Show("Are you sure you want to cancel this operation?", "Are you sure", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DialogResult = DialogResult.Cancel;
                } else
                {
                    e.Cancel = true;

                    tokenSource.Cancel();

                    _task.Dispose();
                }
            } else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void WaitDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
