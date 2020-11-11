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
    public partial class frmThirdGroup : Form
    {
        #region Public Propeties
        public int CurrentGroupId { get; set; }
        public int CurrentSecondaryGroupId { get; set; }
        #endregion
        public frmThirdGroup()
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

        private void SetData()
        {
            SetGroup();
            SetSecondaryGroup();
        }

        private void LoadTableData()
        {
            SetSecondaryGroup();
            int secondoryGroupId = GetSelectedSecondaryGroupId();
            
            if(secondoryGroupId != -1)
            {
                string query = @"SELECT GROUP_NAME, THIRD_GROUP_ID ,SECONDARY_GROUP_NAME, THIRD_GROUP_NAME
                                    FROM ItemThirdGroup 
                                    LEFT OUTER JOIN ItemSecondoryGroup On ItemThirdGroup.SECONDARY_GROUP_ID = ItemSecondoryGroup.SECONDARY_GROUP_ID
                                    LEFT OUTER JOIN itemgroup On ItemSecondoryGroup.GROUP_ID = itemgroup.GROUP_ID
                                    WHERE ItemThirdGroup.SECONDARY_GROUP_ID = " + secondoryGroupId + @" 
                                    ORDER BY SECONDARY_GROUP_NAME ASC";
                clsUtility.FillDataGrid(query, dataGridView1);
            }

           
        }

        private void LoadGroups()
        {
            clsUtility.FillComboBox(" SELECT  GROUP_ID, GROUP_NAME  FROM  ItemGroup  ORDER BY GROUP_NAME", "GROUP_ID", "GROUP_NAME", cmbGroup);
            SetGroup();
        }

        private void LoadPaintModes(int groupId)
        {
            clsUtility.FillComboBox(@" SELECT  SECONDARY_GROUP_ID, SECONDARY_GROUP_NAME  
                                       FROM  ItemSecondoryGroup
                                       WHERE GROUP_ID =" + groupId + @"ORDER BY SECONDARY_GROUP_NAME"
                                , "SECONDARY_GROUP_ID", "SECONDARY_GROUP_NAME", cmbSecondaryGroup);
        }

        private void SetGroup()
        {
            try
            {
                if (CurrentGroupId != 0 && CurrentGroupId != -1)
                {
                    cmbGroup.SelectedValue = CurrentGroupId;
                    this.CurrentGroupId = -1;
                }
            }
            catch (Exception)
            {

            }
        }
        private void SetSecondaryGroup()
        {
            try
            {
                if (CurrentSecondaryGroupId != 0 && CurrentSecondaryGroupId != -1)
                {
                    cmbSecondaryGroup.SelectedValue = CurrentSecondaryGroupId;
                    this.CurrentSecondaryGroupId = -1;
                }
            }
            catch (Exception)
            {

            }
        }

        private ErrorResponse ValidateData()
        {
            int groupId = GetSelectedGroupId();
            int secondaryGroupId = GetSelectedSecondaryGroupId();

            if (groupId == -1)
            {
                return new ErrorResponse(false, Constants.Validation.GROUP_NAME_REQUIRED);
            }
            if(secondaryGroupId == -1)
            {
                return new ErrorResponse(false, Constants.Validation.SECONDARY_GROUP_NAME_REQUIRED);
            }
            if (string.IsNullOrEmpty(txtThirdGroupName.Text))
            {
                return new ErrorResponse(false, Constants.Validation.THIRD_GROUP_NAME_REQUIRED);
            }
            return new ErrorResponse(true, Constants.Validation.SUCCESS); ;
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
            txtThirdGroupID.Text = "";
            txtThirdGroupName.Text = "";
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtThirdGroupID.Text = dataGridView1.Rows[e.RowIndex].Cells["Column1"].Value.ToString();
            txtThirdGroupName.Text = dataGridView1.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
            btnSubmit.Enabled = false;
            btnDelete.Enabled = true;
            btnAlter.Enabled = true;
            txtThirdGroupName.Select();
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
                            this.CurrentGroupId = GetSelectedGroupId();
                            this.CurrentSecondaryGroupId = GetSelectedSecondaryGroupId();
                            clsUtility.ExecuteSQLQuery(" INSERT INTO ItemThirdGroup (THIRD_GROUP_NAME,SECONDARY_GROUP_ID) VALUES ('" + txtThirdGroupName.Text + "', " + CurrentSecondaryGroupId + ") ");
                            btnReset.PerformClick();
                            SetData();
                            LoadTableData();
                            clsUtility.MesgBoxShow("msgSaved", "info");
                        }
                        catch (Exception ex)
                        { MessageBox.Show(ex.Message); }
                    }
                    else
                    {
                        errorProvider.SetError(txtThirdGroupName, res.Message);
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
                if (string.IsNullOrWhiteSpace(this.txtThirdGroupName.Text) | string.IsNullOrWhiteSpace(this.txtThirdGroupID.Text))
                { errorProvider.SetError(txtThirdGroupName, "Required"); }
                else
                {
                    errorProvider.Clear();
                    try
                    {
                        clsUtility.ExecuteSQLQuery("UPDATE  ItemGroup SET GROUP_NAME='" + txtThirdGroupName.Text + "' WHERE  GROUP_ID='" + txtThirdGroupID.Text + "'  ");
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
                if (string.IsNullOrWhiteSpace(this.txtThirdGroupID.Text))
                { errorProvider.SetError(txtThirdGroupName, "Required"); }
                else
                {
                    errorProvider.Clear();
                    try
                    {
                        clsUtility.ExecuteSQLQuery("DELETE FROM  ItemGroup WHERE  GROUP_ID='" + txtThirdGroupID.Text + "'  ");
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

        private void cmbSecondaryGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }

        #region Helper Methods
        private int GetSelectedGroupId()
        {
            int groupId = -1;
            try
            {
                Int32.TryParse(cmbGroup.SelectedValue.ToString(), out groupId);
            }
            catch(Exception)
            {

            }

            return groupId;
        }
        private int GetSelectedSecondaryGroupId()
        {
            int secondaryGroupId = -1;
            try
            {
                Int32.TryParse(cmbSecondaryGroup.SelectedValue.ToString(), out secondaryGroupId);
            }
            catch (Exception)
            {

            }

            return secondaryGroupId;
        }
        #endregion

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int groupId = GetSelectedGroupId();
            if (groupId != 0 && groupId != -1)
            {
                LoadPaintModes(groupId);
            }
        }
    }
}
