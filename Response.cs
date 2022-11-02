using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JobAssignment
{
    public class Response
    {
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Result
    {
        public string phoneNumber { get; set; }
        public int amount { get; set; }
        public string name { get; set; }
        public string accountNumber { get; set; }
    }

    public class StatementResp
    {
        public List<Result> result { get; set; }
        public string message { get; set; }
    }


    public class Account
    {
        public int balance { get; set; }
        public string accountNumber { get; set; }
    }
    public class TransactionResp
    {
        public int tranId { get; set; }
        public string Message { get; set; }
    }
    public class TransactionRespError
    {
        public string Title { get; set; }
        public Error Errors { get; set; }
        public int TranId { get; set; }
        public string Message { get; set; }
        public class Error
        {
            public string[] PhoneNumber { get; set; } = new string []{};
            public string[] Amount { get; set; } = new string[] { };
            public string[] Name { get; set; } = new string[] { };

            public string GetMessage() {
                var builder = new StringBuilder("");
                if (PhoneNumber.Length > 0)
                    builder.Append("PhoneNumber Error(s): ");
                foreach(var error in PhoneNumber)
                {
                    builder.Append(error + ", ");
                }
                if (Name.Length > 0)
                    builder.Append(" Name Error(s): ");
                foreach (var error in Name)
                {
                    builder.Append(error + ", ");
                }
                if (Amount.Length > 0)
                    builder.Append(" Amount Error(s): ");
                foreach (var error in Amount)
                {
                    builder.Append(error + ", ");
                }
                return builder.ToString();
            }
        }
    }
    public class LoginResp
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public object password { get; set; }
        public List<Account> accounts { get; set; }
    }
}