using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace AzureMonitoring
{
    public partial class Form1 : Form
    {
        private AzureApiAccess apiAccess;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (IsSuscriptionFound())
            {
                LoadSubscription();
                HideControls(true);
            }
            else
                HideControls(false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Publish Setting files (*.publishsettings)|*.publishsettings";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(openFileDialog.FileName))
                {
                    string desPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../Subscriptions/") + Path.GetFileName(openFileDialog.FileName);
                    File.Copy(openFileDialog.FileName, desPath);
                    LoadSubscription();
                    HideControls(true);
                }

            }

        }

        private bool IsSuscriptionFound()
        {
            DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../Subscriptions"));
            FileInfo[] Subscriptions = directory.GetFiles("*.publishsettings");
            return Subscriptions.Length > 0;
        }

        private void LoadSubscription()
        {
            DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../Subscriptions"));
            FileInfo[] Subscriptions = directory.GetFiles("*.publishsettings");
            if (Subscriptions.Length > 0)
            {
                var azureSubscription = new AzureSubscription(File.ReadAllText(Subscriptions[0].FullName));
                lblSubName.Text = azureSubscription.Subscription.Name;
                lblSubID.Text = azureSubscription.Subscription.Id;
                apiAccess = new AzureApiAccess(azureSubscription);
            }
        }

        private void HideControls(bool hasSuscription)
        {
            if (hasSuscription)
            {
                label1.Visible = false;
                linkLabel1.Visible = false;
                linkLabel2.Visible = false;
                label6.Visible = false;

                label2.Visible = true;
                label3.Visible = true;
                button1.Visible = true;
                textBox1.Visible = true;
            }
            else
            {
                label1.Visible = true;
                linkLabel1.Visible = true;
                linkLabel2.Visible = true;
                label6.Visible = true;

                label2.Visible = false;
                label3.Visible = false;
                button1.Visible = false;
                textBox1.Visible = false;
            }
        }

        private async void GetServices()
        {
            Task<String> task = apiAccess.GetHostedServices();
            textBox1.Text = await task;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = @"https://manage.windowsazure.com/publishsettings/index?client=xplat",
                UseShellExecute = true
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetServices();
        }
    }
}
