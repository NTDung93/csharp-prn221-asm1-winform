using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Business
{

    public interface IMomAndChildrenResult
    {
        int Status { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }
    }

    public class MomAndChildrenResult : IMomAndChildrenResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public MomAndChildrenResult()
        {
            Status = -1;
            Message = "Action fail";
        }

        public MomAndChildrenResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public MomAndChildrenResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }


    }
}
