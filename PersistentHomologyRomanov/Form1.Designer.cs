namespace PersistentHomologyRomanov
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.OpenImage = new System.Windows.Forms.Button();
            this.SershFriend = new System.Windows.Forms.Button();
            this.textBoxRadius = new System.Windows.Forms.TextBox();
            this.textBoxIteration = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectImage = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SershDir = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.Poisk = new System.Windows.Forms.Button();
            this.WritenInFile = new System.Windows.Forms.CheckBox();
            this.Visualisers = new System.Windows.Forms.CheckBox();
            this.TextboxNextPoint = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxClaster = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxWildViev = new System.Windows.Forms.CheckBox();
            this.checkBoxVievStep = new System.Windows.Forms.CheckBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBoxNameObject = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenImage
            // 
            this.OpenImage.Location = new System.Drawing.Point(10, 182);
            this.OpenImage.Name = "OpenImage";
            this.OpenImage.Size = new System.Drawing.Size(106, 23);
            this.OpenImage.TabIndex = 0;
            this.OpenImage.Text = "Открыть контур";
            this.OpenImage.UseVisualStyleBackColor = true;
            this.OpenImage.Click += new System.EventHandler(this.OpenImage_Click);
            // 
            // SershFriend
            // 
            this.SershFriend.Location = new System.Drawing.Point(10, 211);
            this.SershFriend.Name = "SershFriend";
            this.SershFriend.Size = new System.Drawing.Size(93, 23);
            this.SershFriend.TabIndex = 2;
            this.SershFriend.Text = "Поиск друзей";
            this.SershFriend.UseVisualStyleBackColor = true;
            this.SershFriend.Click += new System.EventHandler(this.SershFriend_Click);
            // 
            // textBoxRadius
            // 
            this.textBoxRadius.Location = new System.Drawing.Point(96, 266);
            this.textBoxRadius.Name = "textBoxRadius";
            this.textBoxRadius.Size = new System.Drawing.Size(100, 20);
            this.textBoxRadius.TabIndex = 3;
            this.textBoxRadius.Text = "6";
            // 
            // textBoxIteration
            // 
            this.textBoxIteration.Location = new System.Drawing.Point(97, 292);
            this.textBoxIteration.Name = "textBoxIteration";
            this.textBoxIteration.Size = new System.Drawing.Size(100, 20);
            this.textBoxIteration.TabIndex = 3;
            this.textBoxIteration.Text = "400";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 273);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Диаметр";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 299);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Итераций";
            // 
            // SelectImage
            // 
            this.SelectImage.Location = new System.Drawing.Point(10, 153);
            this.SelectImage.Name = "SelectImage";
            this.SelectImage.Size = new System.Drawing.Size(139, 23);
            this.SelectImage.TabIndex = 5;
            this.SelectImage.Text = "Вырать изображение";
            this.SelectImage.UseVisualStyleBackColor = true;
            this.SelectImage.Click += new System.EventHandler(this.SelectImage_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(329, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Итерация";
            // 
            // SershDir
            // 
            this.SershDir.Location = new System.Drawing.Point(110, 211);
            this.SershDir.Name = "SershDir";
            this.SershDir.Size = new System.Drawing.Size(86, 23);
            this.SershDir.TabIndex = 7;
            this.SershDir.Text = "ДыроПоиск";
            this.SershDir.UseVisualStyleBackColor = true;
            this.SershDir.Click += new System.EventHandler(this.SershDir_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(329, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Дыр";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(329, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Симплексов";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 346);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "МаксБети";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(122, 182);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Открыть точки";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(274, 285);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(161, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Запуск алгоритма";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(274, 211);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(161, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Строка заголовков в файле";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Poisk
            // 
            this.Poisk.Location = new System.Drawing.Point(274, 320);
            this.Poisk.Name = "Poisk";
            this.Poisk.Size = new System.Drawing.Size(111, 23);
            this.Poisk.TabIndex = 12;
            this.Poisk.Text = "Определить класс";
            this.Poisk.UseVisualStyleBackColor = true;
            this.Poisk.Click += new System.EventHandler(this.Poisk_Click);
            // 
            // WritenInFile
            // 
            this.WritenInFile.AutoSize = true;
            this.WritenInFile.Location = new System.Drawing.Point(12, 128);
            this.WritenInFile.Name = "WritenInFile";
            this.WritenInFile.Size = new System.Drawing.Size(102, 17);
            this.WritenInFile.TabIndex = 13;
            this.WritenInFile.Text = "Запись В файл";
            this.WritenInFile.UseVisualStyleBackColor = true;
            // 
            // Visualisers
            // 
            this.Visualisers.AutoSize = true;
            this.Visualisers.Location = new System.Drawing.Point(122, 128);
            this.Visualisers.Name = "Visualisers";
            this.Visualisers.Size = new System.Drawing.Size(98, 17);
            this.Visualisers.TabIndex = 13;
            this.Visualisers.Text = "Визуализация";
            this.Visualisers.UseVisualStyleBackColor = true;
            // 
            // TextboxNextPoint
            // 
            this.TextboxNextPoint.Location = new System.Drawing.Point(96, 240);
            this.TextboxNextPoint.Name = "TextboxNextPoint";
            this.TextboxNextPoint.Size = new System.Drawing.Size(100, 20);
            this.TextboxNextPoint.TabIndex = 3;
            this.TextboxNextPoint.Text = "25";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Пропуск точек";
            // 
            // textBoxClaster
            // 
            this.textBoxClaster.Location = new System.Drawing.Point(97, 318);
            this.textBoxClaster.Name = "textBoxClaster";
            this.textBoxClaster.Size = new System.Drawing.Size(100, 20);
            this.textBoxClaster.TabIndex = 3;
            this.textBoxClaster.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 325);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Кластеризация";
            // 
            // checkBoxWildViev
            // 
            this.checkBoxWildViev.AutoSize = true;
            this.checkBoxWildViev.Location = new System.Drawing.Point(122, 105);
            this.checkBoxWildViev.Name = "checkBoxWildViev";
            this.checkBoxWildViev.Size = new System.Drawing.Size(97, 17);
            this.checkBoxWildViev.TabIndex = 13;
            this.checkBoxWildViev.Text = "Дикая визуал";
            this.checkBoxWildViev.UseVisualStyleBackColor = true;
            // 
            // checkBoxVievStep
            // 
            this.checkBoxVievStep.AutoSize = true;
            this.checkBoxVievStep.Location = new System.Drawing.Point(122, 82);
            this.checkBoxVievStep.Name = "checkBoxVievStep";
            this.checkBoxVievStep.Size = new System.Drawing.Size(121, 17);
            this.checkBoxVievStep.TabIndex = 13;
            this.checkBoxVievStep.Text = "Визуализация Шаг";
            this.checkBoxVievStep.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(469, 13);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(771, 567);
            this.chart1.TabIndex = 14;
            this.chart1.Text = "chart1";
            // 
            // textBoxNameObject
            // 
            this.textBoxNameObject.Location = new System.Drawing.Point(274, 266);
            this.textBoxNameObject.Name = "textBoxNameObject";
            this.textBoxNameObject.Size = new System.Drawing.Size(161, 20);
            this.textBoxNameObject.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(271, 250);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(164, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Название обучаемого объекта";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 592);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxNameObject);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.checkBoxWildViev);
            this.Controls.Add(this.checkBoxVievStep);
            this.Controls.Add(this.Visualisers);
            this.Controls.Add(this.WritenInFile);
            this.Controls.Add(this.Poisk);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SershDir);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SelectImage);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxClaster);
            this.Controls.Add(this.textBoxIteration);
            this.Controls.Add(this.textBoxRadius);
            this.Controls.Add(this.TextboxNextPoint);
            this.Controls.Add(this.SershFriend);
            this.Controls.Add(this.OpenImage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenImage;
        private System.Windows.Forms.Button SershFriend;
        private System.Windows.Forms.TextBox textBoxRadius;
        private System.Windows.Forms.TextBox textBoxIteration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SelectImage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SershDir;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button Poisk;
        private System.Windows.Forms.CheckBox WritenInFile;
        private System.Windows.Forms.CheckBox Visualisers;
        private System.Windows.Forms.TextBox TextboxNextPoint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxClaster;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxWildViev;
        private System.Windows.Forms.CheckBox checkBoxVievStep;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox textBoxNameObject;
        private System.Windows.Forms.Label label8;
    }
}

