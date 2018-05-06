using System;
using System.Collections.Generic;

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
        /// ensure the delegate is true
        /// </summary>
        /// <param name="truthy">The delegate function to check</param>
        /// <param name="message">The error message if delegate is false</param>
        /// <returns>The current business</returns>
        BusinessExecutor Ensure(Func<bool> truthy, string message);
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
        /// ensure the delegate is true
        /// </summary>
        /// <param name="truthy">The delegate function to check</param>
        /// <param name="message">The error message if delegate is false</param>
        /// <returns>The current business</returns>
        public BusinessExecutor Ensure(Func<bool> truthy, string message)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(message), "Ensure message must be not null or whitespace");

            if (IsFailure)
                return this;

            var @true = false;

            try
            {
                @true = truthy();
            }
            catch (BusinessException ex)
            {
                Errors.Add(ex);
            }
            catch
            {
                Errors.Add(new BusinessException(message));
            }

            if (!@true)
            {
                IsFailure = false;
                Errors.Add(new BusinessException(message));
            }

            return this;
        }
    }
}
