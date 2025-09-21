using CandidateTest.API.DTO;

namespace CandidateTest.WinForms
{
    partial class ProductsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Size = new System.Drawing.Size(600, 200);
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // TextBoxes
            // 
            this.txtName.Location = new System.Drawing.Point(12, 220);
            this.txtPrice.Location = new System.Drawing.Point(12, 250);
            this.txtDescription.Location = new System.Drawing.Point(12, 280);
            this.txtQuantity.Location = new System.Drawing.Point(12, 310);
            // 
            // Buttons
            // 
            this.btnCreate.Location = new System.Drawing.Point(150, 220);
            this.btnCreate.Text = "Create";

            this.btnUpdate.Location = new System.Drawing.Point(150, 250);
            this.btnUpdate.Text = "Update";

            this.btnDelete.Location = new System.Drawing.Point(150, 280);
            this.btnDelete.Text = "Delete";

            this.btnRefresh.Location = new System.Drawing.Point(150, 310);
            this.btnRefresh.Text = "Refresh";
            // Event handler-e se registruju u ProductsForm.cs konstruktoru

            // 
            // ProductsForm
            // 
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Text = "Products CRUD";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}