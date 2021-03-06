using System;
using System.Windows.Forms;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using System.IO;

namespace Reporting_Create_Multi_Value_Report_Parameter {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            // Create a report instance.
            var report = new XtraReport1();
            configureDataSource(ref report);

            // Create a multi-value parameter and specify its properties. 
            Parameter parameter1 = new Parameter();
            parameter1.Name = "CategoryIDs";
            parameter1.Type = typeof(System.Int32);
            parameter1.MultiValue = true;
            parameter1.Description = "Categories: ";

            // Create a DynamicListLookUpSettings instance and set up its properties.
            DynamicListLookUpSettings lookupSettings = new DynamicListLookUpSettings();
            lookupSettings.DataSource = report.DataSource;
            lookupSettings.DataMember = "Categories";
            lookupSettings.DisplayMember = "CategoryName";
            lookupSettings.ValueMember = "CategoryId";

            // Assign the settings to the parameter's LookUpSettings property.
            parameter1.LookUpSettings = lookupSettings;

            // Set the parameter's Visible and SelectAllValues properties to true to
            // make the parameter visible in the Parameters Panel and select all
            // values as defaults.
            parameter1.Visible = true;
            parameter1.SelectAllValues = true;

            // Add the parameter to the report's Parameters collection.
            report.Parameters.Add(parameter1);

            // Use the parameter to filter the report's data.
            report.FilterString = "CategoryID in (?CategoryIDs)";

            report.ShowPreview();
        }

        private void configureDataSource(ref XtraReport1 report) {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var databasePath = Path.Combine(projectDirectory, "nwind.db");
            var connectionParameters = new SQLiteConnectionParameters(databasePath, "");
            var dataSource = new SqlDataSource(connectionParameters);

            var ordersQuery = new CustomSqlQuery();
            ordersQuery.Name = "Categories";
            ordersQuery.Sql = "SELECT * FROM Categories";

            dataSource.Queries.Add(ordersQuery);

            report.DataSource = dataSource;
            report.DataMember = "Categories";
        }
    }
}
