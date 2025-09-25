using System;
using System.Windows.Forms;

namespace Rechner
{
    public partial class Form2 : Form
    {
        private Button button1;
        private Label label1;

        public Form2()
        {
            InitializeComponent();
        }

        // Neuer Konstruktor zum direkten Übergeben der Fehlermeldung
        public Form2(string message) : this()
        {
            SetMessage(message);
        }

        // Öffentliche Methode, um den Meldungstext zu setzen
        public void SetMessage(string message)
        {
            if (label1 != null)
            {
                label1.Text = message ?? string.Empty;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Cyan;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Location = new System.Drawing.Point(20, 37);
            this.label1.MaximumSize = new System.Drawing.Size(440, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(440, 85);
            this.label1.TabIndex = 0;
            this.label1.Text = "Label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(200, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AcceptButton = this.button1;
            this.BackColor = System.Drawing.Color.Brown;
            this.ClientSize = new System.Drawing.Size(472, 203);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Cyan;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fehler";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.ResumeLayout(false);

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
