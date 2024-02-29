using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using NoteTakingApp.Data;
using System.Collections.Generic;

namespace NoteTakingApp
{
    public partial class NoteFrom : Form
    {
        DataTable table;
        List<NoteEntry> noteEntries;
        public NoteFrom()
        {
            InitializeComponent();
            
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.FormClosing += NoteFrom_FormClosing;
        }

        private void NoteFrom_FormClosing(object sender, EventArgs e)
        {
            table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Content", typeof(string));

            savedNotes.DataSource = table;

            savedNotesTable.Columns["Content"].Visible = false;

            savedNotesTable.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            savedNotesTable.Columns["Title"].FillWeight = 100;

            // Load data from json file
            LoadDataFromJsonFile();
        }

        private void NoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDataToJsonFile();
        }

        private void LoadDataFromJsonFile()
        {
            string jsonFilePath = "notes.json";
            if (File.Exists(jsonFilePath))
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                noteEntries = JsonConvert.DeserializeObject<List<NoteEntry>>(jsonData);
                foreach (var note in noteEntries)
                {
                    table.Rows.Add(note.Title, note.Content);
                }
            }
        }

        private void SaveDataToJsonFile()
        {
            string jsonFilePath = "notes.json";
            noteEntries = new List<NoteEntry>();
            foreach (DataGridViewRow row in savedNotesTable.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    NoteEntry note = new NoteEntry();
                    note.Title = row.Cells[0].Value.ToString();
                    note.Content = row.Cells[1].Value.ToString();
                    noteEntries.Add(note);
                }
            }
            string jsonData = JsonConvert.SerializeObject(noteEntries);
            File.WriteAllText(jsonFilePath, jsonData);
        }

        private void savedNotesTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.savedNotesTable.Rows[e.RowIndex];
                titleTextBox.Text = row.Cells[0].Value.ToString();
                contentTextBox.Text = row.Cells[1].Value.ToString();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            table.Rows.Add(titleTextBox.Text, contentTextBox.Text);
            titleTextBox.Text = "";
            contentTextBox.Text = "";
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (savedNotesTable.SelectedRows.Count > 0)
            {
                savedNotesTable.Rows.RemoveAt(savedNotesTable.SelectedRows[0].Index);
            }

            titleTextBox.Text = "";

    }
}