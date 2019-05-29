using Olympic_graph_generator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PieChartMaker
{
    public partial class Form1 : Form
    {
        List<ItemClass> itemList = new List<ItemClass>();
        Graphics canvas;

        public Form1()
        {
            InitializeComponent();
            canvas = picCanvas.CreateGraphics();
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            lblTitle.Text = txtTitle.Text;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                btnColor.BackColor = colorPicker.Color;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (txtName.Text != "")
            {
                if (int.TryParse(txtData.Text, out int data))
                {
                    if(btnAdd.Text == "Add")
                    {
                        itemList.Add(new ItemClass(txtName.Text, data, btnColor.BackColor));
                    }
                    else
                    {
                        ItemClass item = itemList.ElementAt(lstData.SelectedIndex);
                        item.Name = txtName.Text;
                        item.Data = data;
                        item.Color = btnColor.BackColor;
                        btnAdd.Text = "Add";
                        btnDelete.Enabled = false;
                    }
                    RefreshListBox();
                }
                else
                {
                    MessageBox.Show("The Data field must be of valid numerical value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The Name field can not be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void RefreshListBox()
        {
            lstData.Items.Clear();
            foreach(ItemClass item in itemList)
            {
                lstData.Items.Add(item.ToString());
            }
        }

        private void btnGraph_Click(object sender, EventArgs e)
        {
            if(itemList.Count() > 0)
            {
                DrawPieChart(itemList);
            }
        }

        private void DrawPieChart(List<ItemClass> data)
        {
            int canvasWidth = picCanvas.Width;
            int canvasHeight = picCanvas.Height;

            canvas.ResetTransform();
            canvas.TranslateTransform(canvasWidth / 2, canvasHeight / 2);
            canvas.RotateTransform(-90);
            canvas.Clear(Color.White);
            Font font = new Font("Ariel", 14, GraphicsUnit.Pixel);
            float total = (float)data.Sum(n => n.Data);
            int graphBuffer = data.Max(n => TextRenderer.MeasureText(string.Format("{0} : {1}", n.Data, n.Name), font).Width);
            Rectangle border = new Rectangle(-canvasWidth / 2 + graphBuffer, -canvasHeight / 2 + graphBuffer, canvasWidth - graphBuffer * 2, canvasWidth - graphBuffer * 2);
            float prevAngle = 0;
            int textBuffer = 5;
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            foreach (ItemClass item in data)
            {
                float itemPercent = (float)item.Data / total;
                float sweepAngle = itemPercent * 360;
                canvas.FillPie(new SolidBrush(item.Color), border, prevAngle, sweepAngle);

                prevAngle += sweepAngle;
            }
            prevAngle = 0;
            foreach (ItemClass item in data)
            {
                float itemPercent = (float)item.Data / total;
                float sweepAngle = itemPercent * 360;
                string text = string.Format("{0} : {1}", item.Data, item.Name);
                Size stringSize = TextRenderer.MeasureText(text, font);

                if (prevAngle + sweepAngle / 2 < 180)
                {
                    canvas.DrawLine(new Pen(Color.Black, 2), 0, 0, border.Width / 2, 0);
                    canvas.RotateTransform(sweepAngle / 2);
                    canvas.DrawString(text, font, new SolidBrush(Color.Black),
                        border.Width / 2 + stringSize.Width / 2 + textBuffer,
                        -stringSize.Height / 2,
                        format
                        );
                    canvas.RotateTransform(sweepAngle / 2);
                }
                else
                {
                    canvas.ResetTransform();
                    canvas.TranslateTransform(canvasWidth / 2, canvasHeight / 2);

                    canvas.RotateTransform(-270 + prevAngle);
                    canvas.DrawLine(new Pen(Color.Black, 2), 0, 0, -border.Width / 2, 0);

                    canvas.RotateTransform(sweepAngle / 2);
                    canvas.DrawString(text, font, new SolidBrush(Color.Black),
                        -(border.Width / 2 + stringSize.Width / 2 + textBuffer),
                        -stringSize.Height / 2,
                        format
                        );
                }

                prevAngle += sweepAngle;
            }
        }

        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstData.SelectedIndex >= 0 && lstData.SelectedIndex < itemList.Count())
            {
                ItemClass item = itemList.ElementAt(lstData.SelectedIndex);
                txtName.Text = item.Name;
                txtData.Text = item.Data.ToString();
                btnColor.BackColor = item.Color;
                btnAdd.Text = "Change";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            itemList.RemoveAt(lstData.SelectedIndex);
            btnAdd.Text = "Add";
            btnDelete.Enabled = false;
            RefreshListBox();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            List<ItemClass> testItems = new List<ItemClass>();
            txtTitle.Text = "Change whatever you want!\nIf you have any questions on how to use this ask me.\nOr not, whatever.";
            testItems.Add(new ItemClass("Hey!", 1, Color.Black));
            testItems.Add(new ItemClass("Do", 1, Color.Yellow));
            testItems.Add(new ItemClass("You", 1, Color.Black));
            testItems.Add(new ItemClass("Like", 1, Color.Yellow));
            testItems.Add(new ItemClass("It", 1, Color.Black));
            testItems.Add(new ItemClass("?", 1, Color.Yellow));
            itemList = testItems;
            DrawPieChart(itemList);
            RefreshListBox();
        }
    }
}
