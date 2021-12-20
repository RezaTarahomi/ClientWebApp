using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientWebApp.Models.Validation
{
    public class FileSizeLimitAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        private readonly int _minFileSize;
        public FileSizeLimitAttribute(int minFileSize, int maxFileSize)
        {
            _maxFileSize = maxFileSize ;
            _minFileSize = minFileSize ;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as string;
            if (file != null)
            {
                if (file.Length > _maxFileSize * 1024 * 1024 || file.Length < _minFileSize * 1024 * 1024)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"سایز عکس باید بین { _minFileSize }  تا { _maxFileSize } مگابایت باشد.";
        }
    }
}
