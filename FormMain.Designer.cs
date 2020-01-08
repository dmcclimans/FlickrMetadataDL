namespace FlickrMetadataDL
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStop = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cbSearchAccount = new System.Windows.Forms.ComboBox();
            this.btnGetAlbums = new System.Windows.Forms.Button();
            this.dgvPhotosets = new System.Windows.Forms.DataGridView();
            this.chkSearchAllPhotos = new System.Windows.Forms.CheckBox();
            this.chkFilterDate = new System.Windows.Forms.CheckBox();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTakenDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddSearchAccount = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLoginAccount = new System.Windows.Forms.ComboBox();
            this.btnAddLoginAccount = new System.Windows.Forms.Button();
            this.btnRemoveLoginAccount = new System.Windows.Forms.Button();
            this.btnRemoveSearchAccount = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputFile = new System.Windows.Forms.Button();
            this.chkFindAllAlbums = new System.Windows.Forms.CheckBox();
            this.titleDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTakenDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timePeriodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photosetTitleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photoIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.photosetIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourcePhotosets = new System.Windows.Forms.BindingSource(this.components);
            this.EnableSearch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.titleDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberOfPhotosDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhotosets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePhotosets)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(684, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerStart.Location = new System.Drawing.Point(180, 315);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(100, 20);
            this.dateTimePickerStart.TabIndex = 15;
            // 
            // dateTimePickerStop
            // 
            this.dateTimePickerStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateTimePickerStop.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerStop.Location = new System.Drawing.Point(341, 315);
            this.dateTimePickerStop.Name = "dateTimePickerStop";
            this.dateTimePickerStop.Size = new System.Drawing.Size(100, 20);
            this.dateTimePickerStop.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(138, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Start:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(303, 318);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Stop:";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.Location = new System.Drawing.Point(10, 410);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Flickr account to search:";
            // 
            // cbSearchAccount
            // 
            this.cbSearchAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchAccount.FormattingEnabled = true;
            this.cbSearchAccount.Location = new System.Drawing.Point(138, 63);
            this.cbSearchAccount.Name = "cbSearchAccount";
            this.cbSearchAccount.Size = new System.Drawing.Size(375, 21);
            this.cbSearchAccount.TabIndex = 7;
            this.cbSearchAccount.SelectedIndexChanged += new System.EventHandler(this.cbSearchAccount_SelectedIndexChanged);
            // 
            // btnGetAlbums
            // 
            this.btnGetAlbums.Location = new System.Drawing.Point(199, 94);
            this.btnGetAlbums.Name = "btnGetAlbums";
            this.btnGetAlbums.Size = new System.Drawing.Size(75, 23);
            this.btnGetAlbums.TabIndex = 11;
            this.btnGetAlbums.Text = "Get Albums";
            this.btnGetAlbums.UseVisualStyleBackColor = true;
            this.btnGetAlbums.Click += new System.EventHandler(this.btnGetAlbums_Click);
            // 
            // dgvPhotosets
            // 
            this.dgvPhotosets.AllowUserToAddRows = false;
            this.dgvPhotosets.AllowUserToDeleteRows = false;
            this.dgvPhotosets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPhotosets.AutoGenerateColumns = false;
            this.dgvPhotosets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPhotosets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhotosets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EnableSearch,
            this.titleDataGridViewTextBoxColumn1,
            this.numberOfPhotosDataGridViewTextBoxColumn,
            this.DateCreated,
            this.descriptionDataGridViewTextBoxColumn1});
            this.dgvPhotosets.DataSource = this.bindingSourcePhotosets;
            this.dgvPhotosets.Location = new System.Drawing.Point(10, 126);
            this.dgvPhotosets.Name = "dgvPhotosets";
            this.dgvPhotosets.RowHeadersVisible = false;
            this.dgvPhotosets.Size = new System.Drawing.Size(662, 181);
            this.dgvPhotosets.TabIndex = 12;
            // 
            // chkSearchAllPhotos
            // 
            this.chkSearchAllPhotos.AutoSize = true;
            this.chkSearchAllPhotos.Location = new System.Drawing.Point(10, 98);
            this.chkSearchAllPhotos.Name = "chkSearchAllPhotos";
            this.chkSearchAllPhotos.Size = new System.Drawing.Size(182, 17);
            this.chkSearchAllPhotos.TabIndex = 10;
            this.chkSearchAllPhotos.Text = "Search all photos (ignore albums)";
            this.chkSearchAllPhotos.UseVisualStyleBackColor = true;
            this.chkSearchAllPhotos.CheckedChanged += new System.EventHandler(this.chkSearchAllPhotos_CheckedChanged);
            // 
            // chkFilterDate
            // 
            this.chkFilterDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkFilterDate.AutoSize = true;
            this.chkFilterDate.Location = new System.Drawing.Point(10, 317);
            this.chkFilterDate.Name = "chkFilterDate";
            this.chkFilterDate.Size = new System.Drawing.Size(89, 17);
            this.chkFilterDate.TabIndex = 13;
            this.chkFilterDate.Text = "Filter by date:";
            this.chkFilterDate.UseVisualStyleBackColor = true;
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            this.titleDataGridViewTextBoxColumn.Width = 52;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Width = 85;
            // 
            // dateTakenDataGridViewTextBoxColumn
            // 
            this.dateTakenDataGridViewTextBoxColumn.DataPropertyName = "DateTaken";
            this.dateTakenDataGridViewTextBoxColumn.HeaderText = "DateTaken";
            this.dateTakenDataGridViewTextBoxColumn.Name = "dateTakenDataGridViewTextBoxColumn";
            this.dateTakenDataGridViewTextBoxColumn.Width = 86;
            // 
            // btnAddSearchAccount
            // 
            this.btnAddSearchAccount.Location = new System.Drawing.Point(520, 63);
            this.btnAddSearchAccount.Name = "btnAddSearchAccount";
            this.btnAddSearchAccount.Size = new System.Drawing.Size(75, 23);
            this.btnAddSearchAccount.TabIndex = 8;
            this.btnAddSearchAccount.Text = "Add...";
            this.btnAddSearchAccount.UseVisualStyleBackColor = true;
            this.btnAddSearchAccount.Click += new System.EventHandler(this.btnAddSearchAccount_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Flickr login account:";
            // 
            // cbLoginAccount
            // 
            this.cbLoginAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLoginAccount.FormattingEnabled = true;
            this.cbLoginAccount.Location = new System.Drawing.Point(138, 31);
            this.cbLoginAccount.Name = "cbLoginAccount";
            this.cbLoginAccount.Size = new System.Drawing.Size(375, 21);
            this.cbLoginAccount.TabIndex = 3;
            this.cbLoginAccount.SelectedIndexChanged += new System.EventHandler(this.cbLoginAccount_SelectedIndexChanged);
            // 
            // btnAddLoginAccount
            // 
            this.btnAddLoginAccount.Location = new System.Drawing.Point(519, 29);
            this.btnAddLoginAccount.Name = "btnAddLoginAccount";
            this.btnAddLoginAccount.Size = new System.Drawing.Size(75, 23);
            this.btnAddLoginAccount.TabIndex = 4;
            this.btnAddLoginAccount.Text = "Add...";
            this.btnAddLoginAccount.UseVisualStyleBackColor = true;
            this.btnAddLoginAccount.Click += new System.EventHandler(this.btnAddLoginAccount_Click);
            // 
            // btnRemoveLoginAccount
            // 
            this.btnRemoveLoginAccount.Location = new System.Drawing.Point(600, 29);
            this.btnRemoveLoginAccount.Name = "btnRemoveLoginAccount";
            this.btnRemoveLoginAccount.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveLoginAccount.TabIndex = 5;
            this.btnRemoveLoginAccount.Text = "Remove...";
            this.btnRemoveLoginAccount.UseVisualStyleBackColor = true;
            this.btnRemoveLoginAccount.Click += new System.EventHandler(this.btnRemoveLoginAccount_Click);
            // 
            // btnRemoveSearchAccount
            // 
            this.btnRemoveSearchAccount.Location = new System.Drawing.Point(600, 63);
            this.btnRemoveSearchAccount.Name = "btnRemoveSearchAccount";
            this.btnRemoveSearchAccount.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveSearchAccount.TabIndex = 9;
            this.btnRemoveSearchAccount.Text = "Remove...";
            this.btnRemoveSearchAccount.UseVisualStyleBackColor = true;
            this.btnRemoveSearchAccount.Click += new System.EventHandler(this.btnRemoveSearchAccount_Click);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 380);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Output file:";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFile.Location = new System.Drawing.Point(138, 376);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(375, 20);
            this.txtOutputFile.TabIndex = 19;
            // 
            // btnBrowseOutputFile
            // 
            this.btnBrowseOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutputFile.Location = new System.Drawing.Point(520, 374);
            this.btnBrowseOutputFile.Name = "btnBrowseOutputFile";
            this.btnBrowseOutputFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOutputFile.TabIndex = 20;
            this.btnBrowseOutputFile.Text = "Browse...";
            this.btnBrowseOutputFile.UseVisualStyleBackColor = true;
            this.btnBrowseOutputFile.Click += new System.EventHandler(this.btnBrowseOutputFile_Click);
            // 
            // chkFindAllAlbums
            // 
            this.chkFindAllAlbums.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkFindAllAlbums.AutoSize = true;
            this.chkFindAllAlbums.Location = new System.Drawing.Point(10, 350);
            this.chkFindAllAlbums.Name = "chkFindAllAlbums";
            this.chkFindAllAlbums.Size = new System.Drawing.Size(197, 17);
            this.chkFindAllAlbums.TabIndex = 21;
            this.chkFindAllAlbums.Text = "Find all albums for each photo (slow)";
            this.chkFindAllAlbums.UseVisualStyleBackColor = true;
            // 
            // titleDataGridViewTextBoxColumn2
            // 
            this.titleDataGridViewTextBoxColumn2.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn2.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn2.Name = "titleDataGridViewTextBoxColumn2";
            this.titleDataGridViewTextBoxColumn2.ReadOnly = true;
            this.titleDataGridViewTextBoxColumn2.Width = 52;
            // 
            // descriptionDataGridViewTextBoxColumn2
            // 
            this.descriptionDataGridViewTextBoxColumn2.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn2.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn2.Name = "descriptionDataGridViewTextBoxColumn2";
            this.descriptionDataGridViewTextBoxColumn2.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn2.Width = 85;
            // 
            // dateTakenDataGridViewTextBoxColumn1
            // 
            this.dateTakenDataGridViewTextBoxColumn1.DataPropertyName = "DateTaken";
            this.dateTakenDataGridViewTextBoxColumn1.HeaderText = "Date Taken";
            this.dateTakenDataGridViewTextBoxColumn1.Name = "dateTakenDataGridViewTextBoxColumn1";
            this.dateTakenDataGridViewTextBoxColumn1.ReadOnly = true;
            this.dateTakenDataGridViewTextBoxColumn1.Width = 89;
            // 
            // timePeriodDataGridViewTextBoxColumn
            // 
            this.timePeriodDataGridViewTextBoxColumn.DataPropertyName = "TimePeriod";
            this.timePeriodDataGridViewTextBoxColumn.HeaderText = "Time of day";
            this.timePeriodDataGridViewTextBoxColumn.Name = "timePeriodDataGridViewTextBoxColumn";
            this.timePeriodDataGridViewTextBoxColumn.ReadOnly = true;
            this.timePeriodDataGridViewTextBoxColumn.Width = 87;
            // 
            // photosetTitleDataGridViewTextBoxColumn
            // 
            this.photosetTitleDataGridViewTextBoxColumn.DataPropertyName = "PhotosetTitle";
            this.photosetTitleDataGridViewTextBoxColumn.HeaderText = "Album";
            this.photosetTitleDataGridViewTextBoxColumn.Name = "photosetTitleDataGridViewTextBoxColumn";
            this.photosetTitleDataGridViewTextBoxColumn.ReadOnly = true;
            this.photosetTitleDataGridViewTextBoxColumn.Width = 61;
            // 
            // photoIdDataGridViewTextBoxColumn
            // 
            this.photoIdDataGridViewTextBoxColumn.DataPropertyName = "PhotoId";
            this.photoIdDataGridViewTextBoxColumn.HeaderText = "Photo ID";
            this.photoIdDataGridViewTextBoxColumn.Name = "photoIdDataGridViewTextBoxColumn";
            this.photoIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.photoIdDataGridViewTextBoxColumn.Width = 74;
            // 
            // photosetIdDataGridViewTextBoxColumn1
            // 
            this.photosetIdDataGridViewTextBoxColumn1.DataPropertyName = "PhotosetId";
            this.photosetIdDataGridViewTextBoxColumn1.HeaderText = "Album ID";
            this.photosetIdDataGridViewTextBoxColumn1.Name = "photosetIdDataGridViewTextBoxColumn1";
            this.photosetIdDataGridViewTextBoxColumn1.ReadOnly = true;
            this.photosetIdDataGridViewTextBoxColumn1.Width = 75;
            // 
            // bindingSourcePhotosets
            // 
            this.bindingSourcePhotosets.DataSource = typeof(FlickrMetadataDL.Photoset);
            // 
            // EnableSearch
            // 
            this.EnableSearch.DataPropertyName = "EnableSearch";
            this.EnableSearch.HeaderText = "";
            this.EnableSearch.MinimumWidth = 18;
            this.EnableSearch.Name = "EnableSearch";
            this.EnableSearch.Width = 18;
            // 
            // titleDataGridViewTextBoxColumn1
            // 
            this.titleDataGridViewTextBoxColumn1.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn1.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn1.Name = "titleDataGridViewTextBoxColumn1";
            this.titleDataGridViewTextBoxColumn1.ReadOnly = true;
            this.titleDataGridViewTextBoxColumn1.Width = 52;
            // 
            // numberOfPhotosDataGridViewTextBoxColumn
            // 
            this.numberOfPhotosDataGridViewTextBoxColumn.DataPropertyName = "NumberOfPhotos";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.numberOfPhotosDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.numberOfPhotosDataGridViewTextBoxColumn.HeaderText = "Count";
            this.numberOfPhotosDataGridViewTextBoxColumn.Name = "numberOfPhotosDataGridViewTextBoxColumn";
            this.numberOfPhotosDataGridViewTextBoxColumn.ReadOnly = true;
            this.numberOfPhotosDataGridViewTextBoxColumn.Width = 60;
            // 
            // DateCreated
            // 
            this.DateCreated.DataPropertyName = "DateCreated";
            this.DateCreated.HeaderText = "Date Created";
            this.DateCreated.Name = "DateCreated";
            this.DateCreated.ReadOnly = true;
            this.DateCreated.Width = 95;
            // 
            // descriptionDataGridViewTextBoxColumn1
            // 
            this.descriptionDataGridViewTextBoxColumn1.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn1.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn1.Name = "descriptionDataGridViewTextBoxColumn1";
            this.descriptionDataGridViewTextBoxColumn1.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn1.Width = 85;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 442);
            this.Controls.Add(this.chkFindAllAlbums);
            this.Controls.Add(this.btnBrowseOutputFile);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnRemoveSearchAccount);
            this.Controls.Add(this.btnRemoveLoginAccount);
            this.Controls.Add(this.btnAddLoginAccount);
            this.Controls.Add(this.cbLoginAccount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddSearchAccount);
            this.Controls.Add(this.chkFilterDate);
            this.Controls.Add(this.chkSearchAllPhotos);
            this.Controls.Add(this.btnGetAlbums);
            this.Controls.Add(this.cbSearchAccount);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePickerStop);
            this.Controls.Add(this.dateTimePickerStart);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dgvPhotosets);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(700, 480);
            this.Name = "FormMain";
            this.Text = "FlickrMetadataDL";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhotosets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePhotosets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbSearchAccount;
        private System.Windows.Forms.Button btnGetAlbums;
        private System.Windows.Forms.DataGridView dgvPhotosets;
        private System.Windows.Forms.BindingSource bindingSourcePhotosets;
        private System.Windows.Forms.CheckBox chkSearchAllPhotos;
        private System.Windows.Forms.CheckBox chkFilterDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateTakenDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateTakenDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn timePeriodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photosetTitleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photoIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn photosetIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button btnAddSearchAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLoginAccount;
        private System.Windows.Forms.Button btnAddLoginAccount;
        private System.Windows.Forms.Button btnRemoveLoginAccount;
        private System.Windows.Forms.Button btnRemoveSearchAccount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.Button btnBrowseOutputFile;
        private System.Windows.Forms.CheckBox chkFindAllAlbums;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EnableSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfPhotosDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateCreated;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn1;
    }
}

