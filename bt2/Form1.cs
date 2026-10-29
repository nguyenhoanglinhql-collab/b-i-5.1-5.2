using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Thư viện kết nối MySQL

namespace bt2
{
    public partial class Form1 : Form
    {
        // Chuỗi kết nối tới cơ sở dữ liệu ql_sinhvien vừa tạo trên XAMPP
        string connString = "server=localhost; port=3306; database=ql_sinhvien; uid=root; pwd=;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load danh sách khóa học vào ComboBox
            var courses = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int,string>(1, "C# Basic"),
                new KeyValuePair<int,string>(2, ".NET Windows Forms"),
                new KeyValuePair<int,string>(3, "ASP.NET Core"),
                new KeyValuePair<int,string>(4, "Entity Framework")
            };

            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            comboBox1.DataSource = courses;

            // Gọi hàm load dữ liệu sinh viên lên DataGridView (nếu form của bạn đã kéo DataGridView vào)
            // LoadDataToGridView();
        }

        // Hàm lấy dữ liệu từ bảng SinhVien đổ vào DataGridView
        private void LoadDataToGridView()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM SinhVien";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Nếu trên form bạn có kéo DataGridView (ví dụ dataGridView1) thì bỏ comment dòng dưới:
                    // dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string phone = maskedTextBox1.Text;
            string birth = dateTimePicker1.Value.ToString("yyyy-MM-dd"); // Định dạng ngày cho chuẩn MySQL
            string gender = radioButton1.Checked ? radioButton1.Text : radioButton2.Checked ? radioButton2.Text : "Khác";

            string courseName = comboBox1.Text;
            bool agreed = checkBox1.Checked;

            if (!agreed)
            {
                MessageBox.Show("Bạn phải đồng ý điều khoản đăng ký!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thực hiện lưu dữ liệu vào bảng SinhVien trong MySQL
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string query = "INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, Lop, DiemTB) VALUES (@masv, @hoten, @ngaysinh, @gioitinh, @lop, @diemtb)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@masv", "SV" + new Random().Next(10, 99)); // Mã sinh viên tự sinh tạm thời
                        cmd.Parameters.AddWithValue("@hoten", "Nguyễn Hoàng Linh"); // Hoặc lấy từ TextBox nhập tên
                        cmd.Parameters.AddWithValue("@ngaysinh", birth);
                        cmd.Parameters.AddWithValue("@gioitinh", gender);
                        cmd.Parameters.AddWithValue("@lop", courseName);
                        cmd.Parameters.AddWithValue("@diemtb", 8.50);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Thêm dữ liệu thành công vào database ql_sinhvien!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Sau khi thêm xong có thể gọi lại hàm load dữ liệu nếu cần
                        // LoadDataToGridView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối hoặc lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}