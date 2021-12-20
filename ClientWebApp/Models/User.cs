using ClientWebApp.Models.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientWebApp.Models
{
    public class User
    {
        public string Id { get; set; }
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9]{3,19}",
            ErrorMessage = "نام کاربری باید با حرف لاتین شروع شود و حداقل 4 وحداکثر 20 کاراکتر باشد")]
        [Required(ErrorMessage = "نام کاربری را وارد نمایید")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "پسورد را وارد نمایید")]
        [DataType(DataType.Password)]
        [Display(Name = "پسورد")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,20}$",
            ErrorMessage = "پسورد باید حداقل 8 کاراکتر و شامل حرف بزرگ، حرف کوچک، عدد و کاراکترهای خاص باشد ")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "تکرار پسورد")]
        [Compare("Password", ErrorMessage = "پسورد و تکرار آن مشابه نمی باشد")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "نام را وارد نمایید")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "نام خانوادگی را وارد نمایید")]
        [Display(Name = "نام خانوادگی")]
        [Remote("IsLastNameExist", "Account",
               ErrorMessage = "کاربر با این نام خانوادگی قبلا ثبت شده است")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "نام پدر را وارد نمایید")]
        [Display(Name = "نام پدر")]
        public string FatherName { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "کد ملی باید 10 رقم باشد")]

        [Display(Name = "کدملی")]
        public string Code { get; set; }

        [Display(Name = "تلفن همراه")]
        [RegularExpression(@"09[0-9]{9}", ErrorMessage = "شماره همراه را به صورت صحیح وارد نمایید")]
        public string PhoneNumber { get; set; }
        [Display(Name = "نقش کاربری")]
        public string Role { get; set; }
        public string CreatorUserName { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "نام کاربری را وارد نمایید")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9]{3,19}",
            ErrorMessage = "نام کاربری باید با حرف لاتین شروع شود و حداقل 4 وحداکثر 20 کاراکتر باشد")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "پسورد را وارد نمایید")]
        [DataType(DataType.Password)]
        [Display(Name = "پسورد")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,20}$",
            ErrorMessage = "پسورد باید حداقل 8 کاراکتر و شامل حرف بزرگ، حرف کوچک، عدد و کاراکترهای خاص باشد ")]
        public string Password { get; set; }
    }


    public class RegisterViewModel : User
    {

        [FileSizeLimit(1, 2)]
        [Display(Name = "تصویر")]
        public IFormFile Image { get; set; }    
    }
    public class ApiUser : User 
    {
        public string Image { get; set; }       
    }

    public class EditViewModel
    {
        public string Id { get; set; }
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9]{3,19}",
            ErrorMessage = "نام کاربری باید با حرف لاتین شروع شود و حداقل 4 وحداکثر 20 کاراکتر باشد")]
        [Required(ErrorMessage = "نام کاربری را وارد نمایید")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }        
        [Required(ErrorMessage = "نام را وارد نمایید")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "نام خانوادگی را وارد نمایید")]
        [Display(Name = "نام خانوادگی")]
        [Remote("IsLastNameExist", "Account",
               ErrorMessage = "کاربر با این نام خانوادگی قبلا ثبت شده است")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "نام پدر را وارد نمایید")]
        [Display(Name = "نام پدر")]
        public string FatherName { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "کد ملی باید 10 رقم باشد")]

        [Display(Name = "کدملی")]
        [Required(ErrorMessage = "کدملی را وارد نمایید")]
        public string Code { get; set; }

        [Display(Name = "تلفن همراه")]
        [RegularExpression(@"09[0-9]{9}", ErrorMessage = "شماره همراه را به صورت صحیح وارد نمایید")]
        public string PhoneNumber { get; set; }
        [Display(Name = "نقش کاربری")]
        public string Role { get; set; }
        [FileSizeLimit(1, 2)]
        [Display(Name = "تصویر")]
        public IFormFile Image { get; set; }
        public string CreatorUserName { get; set; }
    }


}

