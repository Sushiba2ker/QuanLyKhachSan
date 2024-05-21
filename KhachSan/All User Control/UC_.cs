using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Aspose.Words;
//using Aspose.Words.Tables;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace KhachSan.All_User_Control
{
    public partial class UC_ : UserControl
    {
        public UC_()
        {
            InitializeComponent();
            init();
        }
        private DataProvider dataProvider = new DataProvider();
        private void init()
        {
            loadKh();
            loadDv();
            loadDgPay();
        }
        private void loadKh()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("SELECT cid as [Mã Khách Hàng] ");
            query.Append(" ,cname as [Tên khách hàng]");
            query.Append(" ,rooms.roomid as [Mã phòng]");
            query.Append(" ,roomType as[Loại Phòng]");
            query.Append(" , rooms.bed as [Loại Giường]");
            query.Append(" , rooms.price as [Giá phòng]");
            query.Append(" ,numDays as [Số ngày ở đăng ki]");

            query.Append("FROM customer inner join rooms ON customer.roomid = rooms.roomid ");
            

            dt = dataProvider.execQuery(query.ToString());
            dgKhachHang.DataSource = dt;
        }
        int maKH = 0 ; int maP; int TienPhong = 0;
        private void dgKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = e.RowIndex;
                if (id < 0) id = 0;
                if (id == dgKhachHang.RowCount - 1) id = id - 1;

                DataGridViewRow row = dgKhachHang.Rows[id];

                maKH = (int)row.Cells[0].Value;
                maP = (int)row.Cells[2].Value;
                txtTENKH.Text = row.Cells[1].Value.ToString();
                txtMaPhong.Text = row.Cells[2].Value.ToString();
                txtLoaiPhong.Text = row.Cells[3].Value.ToString();
                txtLoaiGiuong.Text = row.Cells[4].Value.ToString();
                txtGiaPhong.Text = row.Cells[5].Value.ToString();
                txtSoNgay.Text = row.Cells[6].Value.ToString();
                TienPhong = Int32.Parse(txtGiaPhong.Text) * Int32.Parse(txtSoNgay.Text);
                txtTienPhong.Text = TienPhong.ToString();
                loadDvOFID(maKH);
                
            }
            catch
            {
                MessageBox.Show("Không có dữ liệu hoặc không có dòng nào được chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
       
        int totalGP;
        int totalDV;
       
        private void loadDv()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("SELECT cid as [Mã Khách Hàng] ");
            query.Append(" ,Use_services.service_id as [Mã Dịch Vụ]");
            query.Append(" ,services.serviceName as[Tên Dịch Vụ]");
            query.Append(" ,Use_services.quanlity as [Số Lượng]");
            query.Append(" ,(services.price * quanlity)  as [Thành Tiền]");

            query.Append("From Use_services INNER JOIN  services ON Use_services.service_id = services.serviceid INNER JOIN  customer ON Use_services.CustomerID = customer.cid");
            
            dt = dataProvider.execQuery(query.ToString());
            dgDichVu.DataSource = dt;
            
        }
        private void loadDvOFID(int maKH1)
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("SELECT cid as [Mã Khách Hàng] ");
            query.Append(" ,service_id as [Mã Dịch Vụ]");
            query.Append(" ,serviceName as[Tên Dịch Vụ]");
            query.Append(" ,quanlity as [Số Lượng]");
            query.Append(" ,(services.price * quanlity)  as [Thành Tiền]");

            query.Append("From Use_services INNER JOIN  services ON Use_services.service_id = services.serviceid INNER JOIN  customer ON Use_services.CustomerID = customer.cid");
            query.Append(" Where  CustomerID = "+ maKH1);
            dt = dataProvider.execQuery(query.ToString());
            dgDichVu.DataSource = dt;
            //TienPhong = (int)dataProvider.execScaler("SELECT sum(totalPrice) from customer where cid = " + maKH);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            loadDv();
            loadKh(); loadDgPay();
        }
        private void loadDgPay()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("SELECT cname as [Tên Khách Hàng]");
            query.Append(" ,pay.roomid as [Mã Phòng]");
            query.Append(" ,roomType as [Loại Phòng]");
            query.Append(" ,bed as [Loại Giường]");
            query.Append(" ,price as [Giá Phòng]");
            query.Append(" ,numDays as [Số ngày ở]");
            query.Append(" ,totalPrice as [Giá Phòng]");
            query.Append(" ,serviceMoney as [Giá Dịch Vụ]");
            query.Append(" ,total as [Tổng tiền]");
            query.Append(" ,paydate as [Ngày Thanh Toán]");
            query.Append(" FROM pay");
            query.Append(" INNER JOIN customer ON pay.customerId = customer.cid");
            query.Append(" INNER JOIN rooms ON pay.roomid = rooms.roomid");
            dt = dataProvider.execQuery(query.ToString());
            dgBill.DataSource = dt;
        }
        int TT;
        private void t2ThemDichVu_Click(object sender, EventArgs e)
        {
            if (flag1 == true)
            {
               DialogResult check = MessageBox.Show("Bạn có chắc chắn Thanh toán tiền của khách hàng có mã " + maKH + " không? ", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
               if(check == DialogResult.Yes)
               {
                    int tienDV = Int32.Parse( txtTienDV.Text );
                     TT = TienPhong + tienDV;
                    StringBuilder query = new StringBuilder("EXEC proc_pay_them ");
                    query.Append(" @room_id = " + maP);
                    query.Append(" ,@customer_id =" + maKH);
                    query.Append(" ,@room_Mon = " + TienPhong);
                    query.Append(" ,@service_Mon = " + tienDV);
                    query.Append(" ,@total = " + TT);
                    query.Append(" ,@paydate= '" + dateNow.Value + "'");
                    int reslut = dataProvider.execNonQuery(query.ToString());
                    if(reslut == 0)
                    {
                        MessageBox.Show("ADD bill không thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    else
                    {
                        dataProvider.execNonQuery("UPDATE rooms SET booked = 'NO' WHERE roomid = " + maP);

                        loadDgPay();
                    }
               }
            }
            else
            {
                MessageBox.Show("Hãy tính Tổng tiền dịch vụ rồi thử lai!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        bool flag1 = false;
        private void btnTnDV_Click(object sender, EventArgs e)
        {
            object res = dataProvider.execScaler("SELECT SUM(Use_services.quanlity*services.price) FROM services inner join Use_services on service_id = serviceid WHERE customerID = " + maKH);
            if (res != null) 
            {
                if (int.TryParse(res.ToString(), out int tienDichVu))
                {
                    txtTienDV.Text = tienDichVu.ToString();
                    flag1 = true;  
                }
            }
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgBill.CurrentRow;
                if (selectedRow == null)
                {
                    MessageBox.Show("No row selected in dgBill");
                    return;
                }

                // Create a new PDF document
                Document document = new Document();
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BillPrint.pdf");
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
                document.Open();

                Chunk titleChunk = new Chunk("Hotel Bill", FontFactory.GetFont("Arial", 25, iTextSharp.text.Font.BOLD, BaseColor.RED));
                Paragraph title = new Paragraph(titleChunk) { Alignment = Element.ALIGN_CENTER };
                document.Add(title);

                // Create a table to hold content
                PdfPTable table = new PdfPTable(1);
                table.TotalWidth = 500f;
                table.LockedWidth = true;

                // Add content to the table
                table.AddCell("Name Customer: " + selectedRow.Cells["Tên Khách Hàng"].Value.ToString());
                table.AddCell("Room: " + selectedRow.Cells["Mã Phòng"].Value.ToString());
                table.AddCell("Type Room: " + selectedRow.Cells["Loại Phòng"].Value.ToString());
                table.AddCell("Type Bed: " + selectedRow.Cells["Loại Giường"].Value.ToString());
                table.AddCell("Price Room: " + selectedRow.Cells["Giá Phòng"].Value.ToString());
                table.AddCell("Num Day: " + (selectedRow.Cells["Số ngày ở"].Value != DBNull.Value ? selectedRow.Cells["Số ngày ở"].Value.ToString() : "0"));
                table.AddCell("Price Service: " + (selectedRow.Cells["Giá Dịch Vụ"].Value != DBNull.Value ? selectedRow.Cells["Giá Dịch Vụ"].Value.ToString() : "0"));
                table.AddCell("Total: " + selectedRow.Cells["Tổng tiền"].Value.ToString());
                table.AddCell("Date Pay: " + selectedRow.Cells["Ngày Thanh Toán"].Value.ToString());

                // Add the table to the document
                document.Add(table);

                // Add thank you note to the document
                document.Add(new Paragraph("Thank you for using our service! We look forward to serving you in the near future."));

                // Close the document
                document.Close();

                // Open the document
                Process.Start(new ProcessStartInfo("BillPrint.pdf") { UseShellExecute = true });
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

    }
}
