using System.Collections.Generic;
using System.Linq;

namespace FnExecutor
{
    public interface IBusinessExecutor
    {
        /// <summary>
        /// true if previous executor is failed
        /// </summary>
        bool IsFailure { get; set; }

        /// <summary>
        /// true if previous executor is succeed
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// business exception messages
        /// </summary>
        List<BusinessException> Errors { get; set; }



        /// <summary>
        /// The first exception
        /// </summary>
        BusinessException FirstException { get; }

        /// <summary>
        /// The last exception
        /// </summary>
        BusinessException LastException { get; }
    }

    public class BusinessExecutor : IBusinessExecutor
    {
        /// <summary>
        /// true if previous executor is failed
        /// </summary>
        public bool IsFailure { get; set; }

        /// <summary>
        /// true if previous executor is succeed
        /// </summary>
        public bool IsSuccess => !IsFailure;

        /// <summary>
        /// business exception messages
        /// </summary>
        public List<BusinessException> Errors { get; set; } = new List<BusinessException>();

        /// <summary>
        /// The first exception
        /// </summary>
        public BusinessException FirstException => Errors.OrderBy(x => x.ExceptionDate).FirstOrDefault();

        /// <summary>
        /// The last exception
        /// </summary>
        public BusinessException LastException => Errors.OrderByDescending(x => x.ExceptionDate).FirstOrDefault();
    }
}
