using System;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private string currentCity;
        private string currentStation;

        private IntermediaryService.Service1Client client;

        public Form1()
        {
            InitializeComponent();
            client = new IntermediaryService.Service1Client();

            currentCity = null;
            currentStation = null;

            SetCitiesAsync();
        }
        
        private async void SetCitiesAsync()
        {
            string[] cities = await client.GetCitiesAsync();
            // string[] cities = client.getCities();

            comboBoxCity.Text = "";
            comboBoxCity.Items.Clear();

            if (cities != null)
            {
                for (int i = 0; i < cities.Length; i++)
                {
                    comboBoxCity.Items.Add(cities[i]);
                }
            }

            else
            {
                textBox.Text += "Vérifiez votre connexion internet ou rafraichissez la page" + Environment.NewLine;
            }
        }

        private async void SetStationsAsync(string city)
        {
            string[] stations = await client.GetStationsAsync(city);
            // string[] stations = client.getStations(currentCity);

            comboBoxStation.Items.Clear();
            comboBoxStation.Text = "";

            if(stations != null)
            {
                for (int i = 0; i < stations.Length; i++)
                {
                    comboBoxStation.Items.Add(stations[i]);
                }
            }
        }

        private async void SetDetailsAsync(string city, string station, bool refresh = false)
        {
            string[] data = await client.GetDetailsAsync(city, station, refresh);
            //string[] data = client.GetDetails(currentCity, currentStation, refresh);

            textBox.Multiline = true;
            textBox.Clear();

            if (data != null)
            {
                foreach (string line in data)
                {
                    textBox.Text += line + Environment.NewLine;
                }
            }
        }

        private void comboBoxStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            currentStation = comboBoxStation.SelectedItem.ToString();

            SetDetailsAsync(currentCity, currentStation);
            Cursor.Current = Cursors.Default;
        }

        private void comboBoxCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            currentCity = comboBoxCity.SelectedItem.ToString();

            textBox.Multiline = true;
            textBox.Clear();

            SetStationsAsync(currentCity);
            Cursor.Current = Cursors.Default;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SetDetailsAsync(currentCity, currentStation, true);
            Cursor.Current = Cursors.Default;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            client = new IntermediaryService.Service1Client();

            string[] cities = client.GetCities();

            currentCity = null;
            currentStation = null;

            textBox.Multiline = true;
            textBox.Clear();

            comboBoxCity.Text = "";
            comboBoxStation.Text = "";

            comboBoxCity.Items.Clear();
            comboBoxStation.Items.Clear();

            SetCitiesAsync();

            Cursor.Current = Cursors.Default;
        }

        private void InitializeComponent()
        {
            this.labelCity = new System.Windows.Forms.Label();
            this.comboBoxCity = new System.Windows.Forms.ComboBox();
            this.labelStation = new System.Windows.Forms.Label();
            this.comboBoxStation = new System.Windows.Forms.ComboBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCity.Location = new System.Drawing.Point(12, 9);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(185, 20);
            this.labelCity.TabIndex = 0;
            this.labelCity.Text = "Choisissez votre ville :";
            this.labelCity.Click += new System.EventHandler(this.label1_Click);
            // 
            // comboBoxCity
            // 
            this.comboBoxCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxCity.FormattingEnabled = true;
            this.comboBoxCity.Location = new System.Drawing.Point(238, 6);
            this.comboBoxCity.Name = "comboBoxCity";
            this.comboBoxCity.Size = new System.Drawing.Size(327, 28);
            this.comboBoxCity.TabIndex = 1;
            this.comboBoxCity.SelectedIndexChanged += new System.EventHandler(this.comboBoxCity_SelectedIndexChanged);
            // 
            // labelStation
            // 
            this.labelStation.AutoSize = true;
            this.labelStation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStation.Location = new System.Drawing.Point(12, 50);
            this.labelStation.Name = "labelStation";
            this.labelStation.Size = new System.Drawing.Size(210, 20);
            this.labelStation.TabIndex = 2;
            this.labelStation.Text = "Choisissez votre station :";
            this.labelStation.Click += new System.EventHandler(this.label2_Click);
            // 
            // comboBoxStation
            // 
            this.comboBoxStation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxStation.FormattingEnabled = true;
            this.comboBoxStation.Location = new System.Drawing.Point(238, 47);
            this.comboBoxStation.Name = "comboBoxStation";
            this.comboBoxStation.Size = new System.Drawing.Size(327, 28);
            this.comboBoxStation.TabIndex = 3;
            this.comboBoxStation.SelectedIndexChanged += new System.EventHandler(this.comboBoxStation_SelectedIndexChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRefresh.Location = new System.Drawing.Point(355, 294);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(210, 48);
            this.buttonRefresh.TabIndex = 4;
            this.buttonRefresh.Text = "Rafraîchir";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // textBox
            // 
            this.textBox.AllowDrop = true;
            this.textBox.Location = new System.Drawing.Point(16, 99);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(549, 179);
            this.textBox.TabIndex = 5;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // buttonReset
            // 
            this.buttonReset.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReset.Location = new System.Drawing.Point(136, 294);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(213, 48);
            this.buttonReset.TabIndex = 6;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(583, 354);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.comboBoxStation);
            this.Controls.Add(this.labelStation);
            this.Controls.Add(this.comboBoxCity);
            this.Controls.Add(this.labelCity);
            this.Name = "Form1";
            this.Text = "Window";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void textBox_TextChanged(object sender, EventArgs e) { }
    }
}
