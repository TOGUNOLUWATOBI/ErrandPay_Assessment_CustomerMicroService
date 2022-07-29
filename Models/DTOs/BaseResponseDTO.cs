using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs
{
    public class BaseResponseDTO
    {
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
        
    }
}
