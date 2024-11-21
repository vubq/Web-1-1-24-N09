using System.ComponentModel.DataAnnotations;

namespace BT.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Tên phải từ 4 đến 100 ký tự")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi gmail.com")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "Mật khẩu phải có ký tự viết hoa, ký tự viết thường, chữ số và ký tự đặc biệt")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Chi nhánh là bắt buộc")]
        public Branch Branch { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        public Gender Gender { get; set; }

        public bool IsRegular { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        public string? Address { get; set; }

        [Range(typeof(DateTime), "1/1/1963", "12/31/2005", ErrorMessage = "Ngày sinh phải từ năm 1963 đến 2005")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        public DateTime? DateOfBorth { get; set; }

        [Required(ErrorMessage = "Điểm là bắt buộc")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải từ 0.0 đến 10.0")]
        public double? Score { get; set; }

        public string? Avatar { get; set; }
    }
}
