using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KhachSan.All_User_Control
{
    public partial class UC_PieChart : UserControl
    {
        public UC_PieChart()
        {
            InitializeComponent();
        }
        DataProvider dataProvider = new DataProvider();

        private void btnTinh_Click(object sender, EventArgs e)
        {
            loadAreaChart();
            loadTong();
            loadRoomTypeDistribution();
            loadTotalBookedRooms();
            loadTotalServiceRevenue();
        }

        private void loadAreaChart()
        {
            string tungay = dtpTuNgay.Value.ToString("yyyy-MM-dd");
            string denngay = dtpDenNgay.Value.ToString("yyyy-MM-dd");
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("select MONTH(paydate) as Month, YEAR(paydate) as Year, sum(total) as Tong from myHotel1.dbo.pay where paydate between ");
            query.Append(string.Format("'{0} {1}'", tungay, "00:00:00"));
            query.Append(string.Format("and '{0} {1}'", denngay, "23:59:59"));
            query.Append("group by MONTH(paydate), YEAR(paydate)");
            dt = dataProvider.execQuery(query.ToString());
            chart1.DataSource = dt;
            chart1.Series["Series1"].XValueMember = "Month";
            chart1.Series["Series1"].YValueMembers = "Tong";
            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            chart1.Series["Series1"].IsValueShownAsLabel = true;
        }

        private void loadTong()
        {
            string tungay = dtpTuNgay.Value.ToString("dd/MM/yyyy");
            string denngay = dtpDenNgay.Value.ToString("dd/MM/yyyy");
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("select sum(total) as Tong from myHotel1.dbo.pay where paydate between ");
            query.Append(string.Format("convert(datetime,'{0} {1}',103)", tungay, "00:00:00"));
            query.Append(string.Format("and convert(datetime,'{0} {1}',103)", denngay, "23:59:59"));
            dt = dataProvider.execQuery(query.ToString());
            if (dt.Rows.Count > 0)
            {
                txtTong.Text = dt.Rows[0]["Tong"].ToString();
            }
            else
            {
                txtTong.Text = "0";
            }
        }

        private void loadRoomTypeDistribution()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("select roomType, count(*) as Count from myHotel1.dbo.rooms group by roomType");
            dt = dataProvider.execQuery(query.ToString());
            chart2.DataSource = dt;
            chart2.Series["Series1"].XValueMember = "roomType";
            chart2.Series["Series1"].YValueMembers = "Count";
            chart2.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            chart2.Series["Series1"].IsValueShownAsLabel = true;
        }

        private void loadTotalBookedRooms()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("select count(*) as Total from myHotel1.dbo.rooms where booked = 'YES'");
            dt = dataProvider.execQuery(query.ToString());
            if (dt.Rows.Count > 0)
            {
                txtTotalBookedRooms.Text = dt.Rows[0]["Total"].ToString();
            }
            else
            {
                txtTotalBookedRooms.Text = "0";
            }
        }

        private void loadTotalServiceRevenue()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("select sum(s.price * u.quanlity) as Total from myHotel1.dbo.services s inner join myHotel1.dbo.Use_services u on s.serviceid = u.service_id");
            dt = dataProvider.execQuery(query.ToString());
            if (dt.Rows.Count > 0)
            {
                txtTotalServiceRevenue.Text = dt.Rows[0]["Total"].ToString();
            }
            else
            {
                txtTotalServiceRevenue.Text = "0";
            }
        }

    }
}
