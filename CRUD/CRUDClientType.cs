using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class CRUDClientType : Form
    {
        public CRUDClientType()
        {
            InitializeComponent();
        }
        List<string> Msg = new List<string>();
        ConstruccionSoftwEntities db = new ConstruccionSoftwEntities();
        int ClientTypeId = 0;
    
        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            pnlForm.Enabled = true;
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnSave.ForeColor = Color.Green;
            btnCancel.Enabled = true;
        }
        private bool ValidateForm()
        {
            Msg = new List<string>();

            //lblTypeName.Visible = false;
            //lblTypeDescription.Visible = false;
   

            bool result = true;

            if (string.IsNullOrEmpty(txtTypeName.Text))
            {
                Msg.Add("The field (Type Name) is required.");
                lblTypeName.Visible = true;
                result = false;
            }

            if (string.IsNullOrEmpty(txtTypeDescription.Text))
            {
                Msg.Add("The field (Type Description) is required.");
                lblTypeDescription.Visible = true;
                result = false;
            }

            return result;
        }

        private void GetClientTypes()
        {
            var clientTypes = (from a in db.ClientTypes
                           select new { a.Id, a.Name,a.Description,a.Enabled, a.CreatedDate }).ToList();


            dgvClientTypes.DataSource = clientTypes;
            dgvClientTypes.Columns[0].Visible = false;
        }

        private void SaveForm()
        {
            if (ClientTypeId !=0) {
                var clientType = db.ClientTypes.FirstOrDefault(x => x.Id == ClientTypeId);
                if (clientType != null)
                {
                    clientType.Name = txtTypeName.Text;
                    clientType.Description=txtTypeDescription.Text ;
                    var clientTypeSaved= db.SaveChanges()>0;
                    //txtCreatedDate.Text = people.CreatedDate.ToString("MM/dd/yyyy");
                    if (clientTypeSaved)
                    {

                        MessageBox.Show("The client type has been modified.");

                        GetClientTypes();
                        DefaultControls();
                        ClientTypeId = 0;
                        pnlForm.Enabled = false;
                        btnAdd.Enabled = true;
                        btnSave.Enabled = false;
                        btnSave.ForeColor = Color.Black;
                        btnCancel.Enabled = false;

                    }
                }

               
            }
            else
            {
              
           
            var clientType = new ClientType();
            Random rnd = new Random();

            int id = rnd.Next(100000);
            clientType.Id = id;
            clientType.Name = txtTypeName.Text;
            clientType.Description= txtTypeDescription.Text;
            clientType.Enabled = true;
            clientType.CreatedDate = DateTime.Now;

            db.ClientTypes.Add(clientType);
            var clientTypeSaved = db.SaveChanges() > 0;

            if (clientTypeSaved)
            {
             
                    MessageBox.Show("The client type has been added.");

                    GetClientTypes();
                    DefaultControls();

                    pnlForm.Enabled = false;
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnSave.ForeColor = Color.Black;
                    btnCancel.Enabled = false;

            }
            }
        }
        private void GetClientTypeById(int clientTypeId)
        {
            DefaultControls();

            var clientType= db.ClientTypes.FirstOrDefault(x => x.Id == clientTypeId);
            if (clientType != null)
            {
                txtTypeName.Text = clientType.Name;
                txtTypeDescription.Text = clientType.Description;
           
                //txtCreatedDate.Text = people.CreatedDate.ToString("MM/dd/yyyy");

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SaveForm();
            }
            else
            {
                string errors = string.Empty;
                int errorIndex = 1;
                foreach (var item in Msg)
                {
                    errors += $"{errorIndex}. - {item.ToString()}\n";
                    errorIndex++;
                }

                MessageBox.Show(errors, "ERRORS", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GetClientTypes();
            DefaultControls();

            pnlForm.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnSave.ForeColor = Color.Black;
            btnCancel.Enabled = false;
        }

        private void DefaultControls()
        {
            txtTypeName.Text = string.Empty;
            txtTypeDescription.Text = string.Empty;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetClientTypeById(ClientTypeId);

            pnlForm.Enabled = true;
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }

        private void pnlForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmClientType_Load(object sender, EventArgs e)
        {
            GetClientTypes();
        }


    

        private void dgvClientTypes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClientTypeId = 0;

            if (!string.IsNullOrEmpty(dgvClientTypes.SelectedRows[0].Cells["Id"].Value.ToString()))
            {
                ClientTypeId = int.Parse(dgvClientTypes.SelectedRows[0].Cells["Id"].Value.ToString());
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
            }
            else
            {
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
        }

        private void dgvClientTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ClientTypeId != 0)
            {
                var clientType = db.ClientTypes.FirstOrDefault(x => x.Id == ClientTypeId);
                if (clientType != null)
                {
                    if (db.People.Where(x => x.ClientTypeId == ClientTypeId).Count() > 0)
                        MessageBox.Show("The client type cannot be deleted.");
                    else
                    {
                        db.ClientTypes.Remove(clientType);
                        var clientTypeDeleted = db.SaveChanges() > 0;
                        //txtCreatedDate.Text = people.CreatedDate.ToString("MM/dd/yyyy");
                        if (clientTypeDeleted)
                        {

                            MessageBox.Show("The client type has been deleted.");

                            GetClientTypes();
                            DefaultControls();
                            ClientTypeId = 0;
                            pnlForm.Enabled = false;
                            btnAdd.Enabled = true;
                            btnSave.Enabled = false;
                            btnSave.ForeColor = Color.Black;
                            btnCancel.Enabled = false;

                        }
                    }
                }
            }
        }
    }
}
