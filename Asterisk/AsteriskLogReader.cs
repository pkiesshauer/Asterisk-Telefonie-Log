using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Asterisk
{
    public partial class AsteriskLogReader : Form
    {
        Bitmap bitmap;

        private string[] KeywordsStop = { "ABANDON", "COMPLETEAGENT", "COMPLETECALLER" };
        private string KeywordAbandon = "ABANDON";

        // Draw Information 
        // Diese Informationen könnten auch als Einstellungen für den User verfügbar gemacht werden.
        private int ImageWidth = 1960;
        private int ImageHeight = 1080;
        private int OffsetX = 100;
        private int OffsetY = 50;
        private int BarBorder = 5;
        private int RowOffsetX = 10;
        private int BarHeight = 50;
        private int MarginRight = 10;
        private Color red = Color.FromArgb(230, 71, 71);
        private Color yellow = Color.FromArgb(230, 226, 46);
        private Color green = Color.FromArgb(143, 185, 53);
        private Color grey = Color.FromArgb(211, 211, 211);
        private bool DrawOutline = true;
        private int adjustedWidth;
        private int[] Steps = { 60, 120, 300, 600, 900, 1200, 1500, 1800, 3200, 3600 };

        public AsteriskLogReader()
        {
            InitializeComponent();
        }

        private void AsteriskLogReader_Load(object sender, EventArgs e)
        {
            ButtonCreateImage.Enabled = false;
            ButtonSaveBitmap.Enabled = false;
        }

        private void ProcessCallLog()
        {
            if (TextBoxInput.Lines.Count() == 0) { return; }
            string[] rawdata = TextBoxInput.Lines;
            List<AnrufDaten> anrufDaten = PrepareData(rawdata);
            if (anrufDaten.Count == 0) 
            {
                MessageBox.Show("Eine oder mehrere Zeilen im Telefon-Log enthält einen Fehler.", "Warnung");
                return; 
            }
            int maxParallel = anrufDaten.Select(a => anrufDaten.Where(b => b.TimestampStart <= a.TimestampStart && b.TimestampStop > a.TimestampStart).Count()).Max();
            ImageHeight = (BarHeight * maxParallel) + OffsetY;
            PrepareDrawData(anrufDaten, maxParallel);
            bitmap = new Bitmap(ImageWidth, ImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            ButtonSaveBitmap.Enabled = false;
            Graphics graphics = Graphics.FromImage(bitmap);
            DrawBackground(graphics, anrufDaten);
            DrawRowNumbers(graphics, maxParallel);
            DrawTimeLines(graphics, anrufDaten);
            DrawBars(graphics, anrufDaten);
            PictureBoxBitmap.Image = bitmap;
            PictureBoxBitmap.Show();
            TextBoxInput.Hide();
            ButtonSaveBitmap.Enabled = true;
        }

        #region " Data Handling "
        private List<AnrufDaten> PrepareData(string[] lines)
        {
            List<AnrufDaten> anrufDaten = new List<AnrufDaten>();
            string[][] calls = lines.Select(l => l.Split('|')).ToArray();
            if (ValidateDataLength(calls, 5))
            {
                calls = calls.Where(s => KeywordsStop.Contains(s[4])).ToArray();
                if (ValidateDataLength(calls, 8))
                {
                    string[][] callsAbandoned = calls.Where(s => s[4] == KeywordAbandon).ToArray();
                    string[][] callsConnected = calls.Where(s => s[4] != KeywordAbandon).ToArray();
                    for (int i = 0; i < callsConnected.Length; i++)
                    {
                        AnrufDaten call = new AnrufDaten();
                        call.TimestampStop = int.Parse(callsConnected[i][0]);
                        call.TimestampCallStart = call.TimestampStop - int.Parse(callsConnected[i][6]);
                        call.TimestampStart = call.TimestampCallStart - int.Parse(callsConnected[i][5]);
                        anrufDaten.Add(call);
                    }
                    for (int i = 0; i < callsAbandoned.Length; i++)
                    {
                        AnrufDaten call = new AnrufDaten();
                        call.TimestampStop = int.Parse(callsAbandoned[i][0]);
                        call.TimestampCallStart = call.TimestampStop;
                        call.TimestampStart = call.TimestampCallStart - int.Parse(callsAbandoned[i][7]);
                        anrufDaten.Add(call);
                    }
                }
            }
            return anrufDaten.OrderBy(a => a.TimestampStart).ToList();
        }

        private bool ValidateDataLength(string[][] anrufDaten, int numParam)
        {
            int minparam = anrufDaten.Min(a => a.Length);
            if (minparam < numParam) { return false; }
            return true;
        }

        private void PrepareDrawData(List<AnrufDaten> anrufDaten, int maxParallel)
        {
            int duration = anrufDaten.Last().TimestampStop - anrufDaten.First().TimestampStart;
            int mintime = anrufDaten.First().TimestampStart;
            adjustedWidth = ImageWidth - OffsetX - MarginRight;
            for (int i = 0; i < anrufDaten.Count; i++)
            { 
                int parallel = anrufDaten.Where(a => a.TimestampStart <= anrufDaten[i].TimestampStart && a.TimestampStop > anrufDaten[i].TimestampStart).Count();
                anrufDaten[i].StartDrawY = OffsetY + BarHeight * parallel;
                anrufDaten[i].height = BarHeight;
                anrufDaten[i].StartDrawX = (int)((anrufDaten[i].TimestampStart - mintime) / (float)duration * adjustedWidth) + OffsetX;
                anrufDaten[i].CallStartDrawX = Math.Max((int)(((anrufDaten[i].TimestampCallStart - mintime) / (float)duration) * adjustedWidth), anrufDaten[i].StartDrawX - OffsetX + 1) + OffsetX;
                anrufDaten[i].StopDrawX = Math.Max((int)(((anrufDaten[i].TimestampStop - mintime) / (float)duration) * adjustedWidth), anrufDaten[i].CallStartDrawX - OffsetX + 1) + OffsetX;
            }
        }
        #endregion 

        #region " Drawing "
        private void DrawBackground(Graphics graphics, List<AnrufDaten> anrufDaten)
        {
            Brush brush = new SolidBrush(grey);
            graphics.FillRectangle(brush, 0, 0, ImageWidth, ImageHeight);
            Pen pen = new Pen(Color.Black);
            graphics.DrawLine(pen, 0, ImageHeight - OffsetY, ImageWidth, ImageHeight - OffsetY);
        }

        private void DrawRowNumbers(Graphics graphics, int rows)
        {
            for (int i = 0; i < rows; i++)
            {
                int height = ImageHeight - OffsetY - (BarHeight * (i + 1)) + (int)(BarHeight * 0.5) - 8;
                RectangleF rectf = new RectangleF(RowOffsetX, height, OffsetX - RowOffsetX, BarHeight);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawString((i + 1).ToString(), new Font("Tahoma", 16), Brushes.Black, rectf);
            }
        }

        private void DrawTimeLines(Graphics graphics, List<AnrufDaten> anrufDaten)
        {
            //Kann vermutlich standardisiert werden um z.B. einen Tagesbericht mit fixer Zeitskala zu erstellen.
            int duration = anrufDaten.Last().TimestampStop - anrufDaten.First().TimestampStart;
            int mintime = anrufDaten.First().TimestampStart;
            DateTime tmptime = DateTimeOffset.FromUnixTimeSeconds(mintime).LocalDateTime;
            int addseconds = 0;
            float percent = addseconds / (float)duration;
            DrawTimeLine(graphics, percent, tmptime.ToString("HH:mm"));
            int step = Steps.Where(a => duration - addseconds > a).Max();
            while (addseconds < duration)
            {
                addseconds += step;
                percent = addseconds / (float)duration;
                tmptime = DateTimeOffset.FromUnixTimeSeconds(mintime + addseconds).LocalDateTime;
                DrawTimeLine(graphics, percent, tmptime.ToString("HH:mm"));
            }
        }

        private void DrawTimeLine(Graphics graphics, float percent, string label)
        {
            Pen pen = new Pen(Color.Black);
            int LabelSize = 75;
            int x = OffsetX + (int)(adjustedWidth * percent);
            int y = ImageHeight - OffsetY + BarBorder;
            graphics.DrawLine(pen, x, 0, x, ImageHeight - OffsetY + BarBorder);
            if (x + LabelSize > ImageWidth)
            {
                x -= LabelSize;
            }
            RectangleF rectf = new RectangleF(x, y, LabelSize, LabelSize);
            graphics.DrawString(label, new Font("Tahoma", 16), Brushes.Black, rectf);
        }

        public void DrawBars(Graphics graphics, List<AnrufDaten> anrufDaten)
        {
            for (int i = 0; i < anrufDaten.Count; i++)
            {
                DrawBar(graphics, anrufDaten[i]);
            }
        }

        private void DrawBar(Graphics graphics, AnrufDaten anrufData)
        {
            Brush brush;
            int x;
            int y;
            int width;
            int height;

            if (anrufData.TimestampCallStart == anrufData.TimestampStop)
            {
                brush = new SolidBrush(red);
                x = anrufData.StartDrawX;
                y = ImageHeight - (anrufData.StartDrawY - BarBorder);
                width = anrufData.StopDrawX - anrufData.StartDrawX;
                height = anrufData.height - (BarBorder * 2);
                graphics.FillRectangle(brush, x, y, width, height);
            }
            else
            {
                brush = new SolidBrush(yellow);
                x = anrufData.StartDrawX;
                y = ImageHeight - (anrufData.StartDrawY - BarBorder);
                width = anrufData.CallStartDrawX - anrufData.StartDrawX;
                height = anrufData.height - (BarBorder * 2);
                graphics.FillRectangle(brush, x, y, width, height);

                brush = new SolidBrush(green);
                x = anrufData.CallStartDrawX;
                width = anrufData.StopDrawX - anrufData.CallStartDrawX;
                graphics.FillRectangle(brush, x, y, width, height);
            }
            if (DrawOutline)
            {
                Pen pen = new Pen(Color.Black);
                x = anrufData.StartDrawX - 1;
                width = anrufData.StopDrawX - anrufData.StartDrawX + 2;
                graphics.DrawRectangle(pen, x, y, width, height);
            }
        }
        #endregion

        #region " UI Handling"

        private void ButtonReadFromFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    IEnumerable<string> lines = File.ReadLines(openFileDialog.FileName);
                    TextBoxInput.Lines = lines.ToArray();
                    TextBoxInput.Show();
                    PictureBoxBitmap.Hide();
                }
            }
        }

        private void ButtonReadFromText_Click(object sender, EventArgs e)
        {
            ProcessCallLog();
        }

        private void ButtonSaveBitmap_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            // Hier ggf. verschiedene Bildformate ermöglichen.
            dialog.DefaultExt = ".bmp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bitmap.Save(dialog.FileName, ImageFormat.Bmp);
            }
        }


        private void TextBoxInput_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxInput.Text.Equals(string.Empty))
            {
                ButtonCreateImage.Enabled = false;
            }
            else
            {
                ButtonCreateImage.Enabled = true;
            }
        }
        #endregion
    }

    public class AnrufDaten
    {
        public string ID;
        public int TimestampStart;
        public int TimestampCallStart;
        public int TimestampStop;

        public int StartDrawX;
        public int StartDrawY;
        public int CallStartDrawX;
        public int StopDrawX;
        public int height;
    }
}
