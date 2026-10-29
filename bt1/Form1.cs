using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Thêm thư viện kết nối MySQL

namespace bt1
{
    public partial class Form1 : Form
    {
        // Chuỗi kết nối tới MySQL trên XAMPP (port 3306, user: root, không có mật khẩu)
        // Bạn có thể đổi tên database tùy ý (ví dụ: quanly_dangnhap hoặc ql_sinhvien)
        string connString = "server=localhost; port=3306; database=ql_sinhvien; uid=root; pwd=;";

        public Form1()
        {
            InitializeComponent();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            bool isValid = true;
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider.SetError(txtUsername, "Không được để trống Tên đăng nhập!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider.SetError(txtPassword, "Không được để trống Mật khẩu!");
                isValid = false;
            }

            if (!isValid)
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    txtUsername.Focus();
                }
                else
                {
                    txtPassword.Focus();
                }
                return;
            }

            // --- KIỂM TRA TÀI KHOẢN TRONG CƠ SỞ DỮ LIỆU MYSQL ---
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Câu lệnh truy vấn kiểm tra tài khoản (Giả sử bảng trong CSDL của bạn tên là NguoiDung hoặc TaiKhoan)
                    // Nếu dùng bảng SinhVien, bạn có thể chỉnh lại cột cho phù hợp
                    string query = "SELECT COUNT(*) FROM NguoiDung WHERE Username = @user AND Password = @pass";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", password);

                        // Lấy số lượng bản ghi khớp
                        long count = (long)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Mở form tiếp theo ở đây nếu cần (Ví dụ: FormMain frm = new FormMain(); frm.Show(); this.Hide();)
                        }
                        else
                        {
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtUsername.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}