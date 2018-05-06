using System;

namespace FnExecutor
{
    public class BusinessException : Exception
    {
        /// <summary>
        /// The exception code
        /// </summary>
        public string Code { get; set; }

        public BusinessException(string message) : base(message)
        {

        }

        public BusinessException(string message, string code) : this(message)
        {
            Code = code;
        }

        public BusinessException(BusinessException exception) : this(exception?.Message, exception?.Code)
        {

        }
    }
}
