using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs
{
    public class OperationResponseDTO<Object>
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public Object data { get; set; }
    }
}
