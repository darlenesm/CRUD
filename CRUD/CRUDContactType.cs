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
    public partial class CRUDContactType : Form
    {
        public CRUDContactType()
        {
            InitializeComponent();
        }
        List<string> Msg = new List<string>();
        ConstruccionSoftwEntities db = new ConstruccionSoftwEntities();
        int ContactTypeId = 0;
        private void FrmContactType_Load(object sender, EventArgs e)
        {
            GetContactTypes();
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
                //lblTypeName.Visible = true;
                result = false;
            }

            if (string.IsNullOrEmpty(txtTypeDescription.Text))
            {
                Msg.Add("The field (Type Description) is required.");
                //lblTypeDescription.Visible = true;
                result = false;
            }

            return result;
        }

        private void GetContactTypes()
        {
            var peoples = (from a in db.ContactTypes
                           select new { a.Id, a.Name, a.Description, a.Enabled, a.CreatedDate }).ToList();


            dgvContactTypes.DataSource = peoples;
            dgvContactTypes.Columns[0].Visible = false;
        }

        private void SaveForm()
        {
            if (ContactTypeId != 0)
            {
                var contactType = db.ContactTypes.FirstOrDefault(x => x.Id == ContactTypeId);
                if (contactType != null)
                {
                    contactType.Name = txtTypeName.Text;
                    contactType.Description = txtTypeDescription.Text;
                    var contactTypeSaved = db.SaveChanges() > 0;
                    //txtCreatedDate.Text = people.CreatedDate.ToString("MM/dd/yyyy");
                    if (contactTypeSaved)
                    {

                        MessageBox.Show("The contact type has been modified.");

                        GetContactTypes();
                        DefaultControls();
                        ContactTypeId = 0;
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


                var contactType = new ContactType();
                Random rnd = new Random();

                int id = rnd.Next(100000);
                contactType.Id = id;
                contactType.Name = txtTypeName.Text;
                contactType.Description = txtTypeDescription.Text;
                contactType.Enabled = true;
                contactType.CreatedDate = DateTime.Now;

                db.ContactTypes.Add(contactType);
                var contactTypeSaved = db.SaveChanges() > 0;

                if (contactTypeSaved)
                {

                    MessageBox.Show("The contact type has been added.");

                    GetContactTypes();
                    DefaultControls();

                    pnlForm.Enabled = false;
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnSave.ForeColor = Color.Black;
                    btnCancel.Enabled = false;

                }
            }
        }
        private void GetContactTypeById(int contactTypeId)
        {
            DefaultControls();

            var contactType = db.ContactTypes.FirstOrDefault(x => x.Id == contactTypeId);
            if (contactType != null)
            {
                txtTypeName.Text = contactType.Name;
                txtTypeDescription.Text = contactType.Description;

                //txtCreatedDate.Text = people.CreatedDate.ToString("MM/dd/yyyy");

            }
        }

     

        private void DefaultControls()
        {
            txtTypeName.Text = string.Empty;
            txtTypeDescription.Text = string.Empty;
        }

       

        private void pnlForm_Paint(object sender, PaintEventArgs e)
        {

        }

   
        private void dgvContactTypes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ContactTypeId = 0;

            if (!string.IsNullOrEmpty(dgvContactTypes.SelectedRows[0].Cells["Id"].Value.ToString()))
            {
                ContactTypeId = int.Parse(dgvContactTypes.SelectedRows[0].Cells["Id"].Value.ToString());
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
            }
            else
            {
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
        }

        private void dgvContactTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            pnlForm.Enabled = true;
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnSave.ForeColor = Color.Green;
            btnCancel.Enabled = true;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
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

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            GetContactTypes();
            DefaultControls();

            pnlForm.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnSave.ForeColor = Color.Black;
            btnCancel.Enabled = false;
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            GetContactTypeById(ContactTypeId);

            pnlForm.Enabled = true;
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ContactTypeId != 0)
            {
                var contactType = db.ContactTypes.FirstOrDefault(x => x.Id == ContactTypeId);
                if (contactType != null)
                {
                    if(db.People.Where(x=>x.ContactTypeId== ContactTypeId).Count() > 0)
                        MessageBox.Show("The contact type cannot be deleted.");
                    else
                            {
                                db.ContactTypes.Remove(contactType);
                                var contactTypeDeleted = db.SaveChanges() > 0;
                                //txtCreatedDate.Text = people.CreatedDate.ToString("MM/dd/yyyy");
                                if (contactTypeDeleted)
                                {

                                    MessageBox.Show("The contact type has been deleted.");

                                    GetContactTypes();
                                    DefaultControls();
                                    ContactTypeId = 0;
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
