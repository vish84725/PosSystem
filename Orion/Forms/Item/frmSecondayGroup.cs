using Orion.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Orion
{
    public partial class frmSecondaryGroup : Form
    {
        public frmSecondaryGroup()
        {
            InitializeComponent();
        }

        private void frmGroup_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            btnDelete.Enabled = false;
            btnAlter.Enabled = false;
            btnReset.PerformClick();
            LoadLanguegePack();
        }

        private void LoadLanguegePack()
        {
            if (Properties.Settings.Default.App_Default_Language)
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(@"Language\" + Properties.Settings.Default.App_Language + ".xml");
                    XmlNodeList languageNodes = xmlDocument.GetElementsByTagName("language");
                    foreach (XmlNode languageNode in languageNodes)
                    {
                        XmlNode l1172 = languageNode["l1172"];
                        lbl1172.Text = l1172.InnerText;

                        XmlNode l1173 = languageNode["l1173"];
                        lbl1173.Text = l1173.InnerText;

                        XmlNode l1174 = languageNode["l1174"];
                        lbl1174.Text = l1174.InnerText;

                        XmlNode ctrlSave = languageNode["ctrlSave"];
                        btnSubmit.Text = ctrlSave.InnerText;

                        XmlNode ctrlClose = languageNode["ctrlClose"];
                        btnClose.Text = ctrlClose.InnerText;

                        XmlNode ctrlAlter = languageNode["ctrlAlter"];
                        btnAlter.Text = ctrlAlter.InnerText;

                        XmlNode ctrlDelete = languageNode["ctrlDelete"];
                        btnDelete.Text = ctrlDelete.InnerText;

                        XmlNode ctrlReset = languageNode["ctrlReset"];
                        btnReset.Text = ctrlReset.InnerText;

                        XmlNode ctrlRefresh = languageNode["ctrlRefresh"];
                        btnRefresh.Text = ctrlRefresh.InnerText;

                        XmlNode l1182 = languageNode["l1182"];
                        dataGridView1.Columns["Column1"].HeaderText = l1182.InnerText;
                        dataGridView1.Columns["Column2"].HeaderText = l1174.InnerText;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadData()
        {
            LoadGroups();
            LoadTableData();
        }

        private void LoadTableData()
        {
            int groupId = -1;
            Int32.TryParse(cmbGroup.SelectedValue.ToString(), out groupId);
            
            if(groupId != -1)
            {
                string query = @"SELECT GROUP_NAME, SECONDARY_GROUP_ID ,SECONDARY_GROUP_NAME
                                FROM ItemSecondoryGroup 
                                INNER JOIN itemgroup On ItemSecondoryGroup.GROUP_ID = itemgroup.GROUP_ID
                                WHERE itemgroup.GROUP_ID = " + groupId + @" 
                                ORDER BY SECONDARY_GROUP_NAME ASC";
                clsUtility.FillDataGrid(query, dataGridView1);
            }

           
        }

        private void LoadGroups()
        {
            clsUtility.FillComboBox(" SELECT  GROUP_ID, GROUP_NAME  FROM  ItemGroup  ORDER BY GROUP_NAME", "GROUP_ID", "GROUP_NAME", cmbGroup);
        }

        private void InitializeData(int groupId)
        {
            if(groupId != 0 && groupId != -1)
            {
                cmbGroup.SelectedValue = groupId;
            }
        }

        private ErrorResponse ValidateData()
        {
            int groupId = -1;

            Int32.TryParse(cmbGroup.SelectedValue.ToString(), out groupId);

            if(groupId == -1)
            {
                return new ErrorResponse(false, Constants.Validation.GROUP_NAME_REQUIRED);
            }
            if(string.IsNullOrEmpty(txtSecondaryGroupName.Text))
            {
                return new ErrorResponse(false, Constants.Validation.SECONDARY_GROUP_NAME_REQUIRED);
            }
            return new ErrorResponse(true, Constants.Validation.SUCCESS); ;
        }

        private int GetSelectedGroupId()
        {
            int groupId = -1;
            Int32.TryParse(cmbGroup.SelectedValue.ToString(),out groupId);
            return groupId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTableData();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            btnAlter.Enabled = false;
            btnSubmit.Enabled = true;
            txtSecondaryGroupID.Text = "";
            txtSecondaryGroupName.Text = "";
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSecondaryGroupID.Text = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
            txtSecondaryGroupName.Text = dataGridView1.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
            btnSubmit.Enabled = false;
            btnDelete.Enabled = true;
            btnAlter.Enabled = true;
            txtSecondaryGroupName.Select();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            clsUtility.ExecuteSQLQuery(" SELECT  * FROM   Users  WHERE   USER_ID = '" + clsUtility.UserID + "' AND   Can_Add = 'Y' ");
            if (clsUtility.sqlDT.Rows.Count > 0)
            {
                ///////////////////////////////////////////
                ErrorResponse res = ValidateData();
                if(res != null)
                {
                    if(res.IsSuccess)
                    {
                        errorProvider.Clear();
                        try
                        {
                            var groupId = GetSelectedGroupId();
                            clsUtility.ExecuteSQLQuery(" INSERT INTO ItemSecondoryGroup (SECONDARY_GROUP_NAME,GROUP_ID) VALUES ('" + txtSecondaryGroupName.Text + "', " + groupId + ") ");
                            btnReset.PerformClick();
                            InitializeData(groupId);
                            LoadTableData();
                            clsUtility.MesgBoxShow("msgSaved", "info");
                        }
                        catch (Exception ex)
                        { MessageBox.Show(ex.Message); }
                    }
                    else
                    {
                        errorProvider.SetError(txtSecondaryGroupName, res.Message);
                    }
                }
                ///////////////////////////////////////////
            }
            else
            {
                clsUtility.MesgBoxShow("msgPermission", "err");
            }
        }

        private void btnAlter_Click(object sender, EventArgs e)
        {
            clsUtility.ExecuteSQLQuery(" SELECT  * FROM   Users  WHERE   USER_ID = '" + clsUtility.UserID + "' AND   Can_Edit = 'Y' ");
            if (clsUtility.sqlDT.Rows.Count > 0)
            {
                ///////////////////////////////////////////
                if (string.IsNullOrWhiteSpace(this.txtSecondaryGroupName.Text) | string.IsNullOrWhiteSpace(this.txtSecondaryGroupID.Text))
                { errorProvider.SetError(txtSecondaryGroupName, "Required"); }
                else
                {
                    errorProvider.Clear();
                    try
                    {
                        clsUtility.ExecuteSQLQuery("UPDATE  ItemGroup SET GROUP_NAME='" + txtSecondaryGroupName.Text + "' WHERE  GROUP_ID='" + txtSecondaryGroupID.Text + "'  ");
                        btnReset.PerformClick();
                        clsUtility.MesgBoxShow("msgUpdate", "info");
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message); }
                }
                ///////////////////////////////////////////
            }
            else
            {
                clsUtility.MesgBoxShow("msgPermission", "err");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            clsUtility.ExecuteSQLQuery(" SELECT  * FROM   Users  WHERE   USER_ID = '" + clsUtility.UserID + "' AND   Can_Delete = 'Y' ");
            if (clsUtility.sqlDT.Rows.Count > 0)
            {
                ///////////////////////////////////////////
                if (string.IsNullOrWhiteSpace(this.txtSecondaryGroupID.Text))
                { errorProvider.SetError(txtSecondaryGroupName, "Required"); }
                else
                {
                    errorProvider.Clear();
                    try
                    {
                        clsUtility.ExecuteSQLQuery("DELETE FROM  ItemGroup WHERE  GROUP_ID='" + txtSecondaryGroupID.Text + "'  ");
                        btnReset.PerformClick();
                        clsUtility.MesgBoxShow("msgDelete", "info");
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message); }
                }
                ///////////////////////////////////////////
            }
            else
            {
                clsUtility.MesgBoxShow("msgPermission", "err");
            }
        }

    }
}
