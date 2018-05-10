using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FnExecutor
{
    public static class BusinessExecutorExtension
    {
        /// <summary>
        /// ensure the delegate is true
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="truthy">The delegate function to check</param>
        /// <param name="message">The error message if delegate is false</param>
        /// <returns>The current business</returns>
        public static BusinessExecutor Ensure(this BusinessExecutor bus, Func<bool> truthy, string message = null)
        {
            if (bus.IsFailure)
                return bus;
            
            try
            {
                var @true = truthy();

                if (!@true)
                {
                    bus.IsFailure = true;
                    if (!string.IsNullOrWhiteSpace(message))
                        bus.Errors.Add(new BusinessException(message));
                }
            }
            catch (BusinessException ex)
            {
                if (string.IsNullOrWhiteSpace(message))
                    bus.Errors.Add(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + ex.StackTrace);
            }

            return bus;
        }

        /// <summary>
        /// ensure the delegate is true asynchronous
        /// </summary>
        /// <param name="busTask">The bus.</param>
        /// <param name="truthy">The delegate function to check</param>
        /// <param name="message">The error message if delegate is false</param>
        /// <param name="captureContext">if set to <c>true</c> [capture context].</param>
        /// <returns>
        /// The current business
        /// </returns>
        public static async Task<BusinessExecutor> Ensure(this Task<BusinessExecutor> busTask, Func<bool> truthy, string message, bool captureContext = false)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(message), "Ensure message must be not null or whitespace");

            var bus = await busTask.ConfigureAwait(captureContext);

            if (bus.IsFailure)
                return bus;
            
            try
            {
                var @true = truthy();

                if (!@true)
                {
                    bus.IsFailure = true;
                    if (!string.IsNullOrWhiteSpace(message))
                        bus.Errors.Add(new BusinessException(message));
                }
            }
            catch (BusinessException ex)
            {
                if (string.IsNullOrWhiteSpace(message))
                    bus.Errors.Add(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + ex.StackTrace);
            }

            return bus;
        }

        /// <summary>
        /// Ensure the object is not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus">The bus.</param>
        /// <param name="obj">The object to check</param>
        /// <param name="message">The message if object is null</param>
        /// <returns>The current business</returns>
        public static BusinessExecutor EnsureNotNull<T>(this BusinessExecutor bus, T obj, string message = null)
        {
            if (obj != null)
                return bus;

            if (string.IsNullOrWhiteSpace(message))
                message = $"Object of type {typeof(T)} must be not null";

            bus.IsFailure = true;
            bus.Errors.Add(new BusinessException(message));

            return bus;
        }

        /// <summary>
        /// Ensure the object is not null asynchronous
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="busTask">The bus task.</param>
        /// <param name="obj">The object to check</param>
        /// <param name="message">The message if object is null</param>
        /// <param name="captureContext">if set to <c>true</c> [capture context].</param>
        /// <returns>
        /// The current business
        /// </returns>
        public static async Task<BusinessExecutor> EnsureNotNull<T>(this Task<BusinessExecutor> busTask, T obj, string message = null, bool captureContext = false)
        {
            var bus = await busTask.ConfigureAwait(captureContext);

            if (obj != null)
                return bus;

            if (string.IsNullOrWhiteSpace(message))
                message = $"Object of type {typeof(T)} must be not null";

            bus.IsFailure = true;
            bus.Errors.Add(new BusinessException(message));

            return bus;
        }


        /// <summary>
        /// Execute the action synchronous
        /// </summary>
        /// <param name="busTask">The previous business</param>
        /// <param name="action">The action need to be executed</param>
        /// <returns>The executed business</returns>
        public static BusinessExecutor Execute(this BusinessExecutor busTask, Action<BusinessExecutor> action)
        {
            var result = busTask;
            if (result.IsSuccess)
            {
                try
                {
                    action(result);
                }
                catch (BusinessException ex)
                {
                    result.IsFailure = true;
                    result.Errors.Add(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Execute the action asynchronous
        /// </summary>
        /// <param name="busTask">The previous business task</param>
        /// <param name="action">The action need to be executed</param>
        /// <param name="captureContext">true to capture outside context, default is false</param>
        /// <returns>The executed business</returns>
        public static async Task<BusinessExecutor> Execute(this Task<BusinessExecutor> busTask, Action<BusinessExecutor> action, bool captureContext = false)
        {
            var result = await busTask.ConfigureAwait(captureContext);
            if (result.IsSuccess)
            {
                try
                {
                    action(result);
                }
                catch (BusinessException ex)
                {
                    result.IsFailure = true;
                    result.Errors.Add(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Execute the action asynchronous
        /// </summary>
        /// <param name="busTask">The previous business task</param>
        /// <param name="action">The action need to be executed asynchronously</param>
        /// <param name="captureContext">true to capture outside context, default is false</param>
        /// <returns>The executed business</returns>
        public static async Task<BusinessExecutor> Execute(this BusinessExecutor busTask, Func<BusinessExecutor, Task> action, bool captureContext = false)
        {
            var result = busTask;
            if (result.IsSuccess)
            {
                try
                {
                    await action(result).ConfigureAwait(captureContext);
                }
                catch (BusinessException ex)
                {
                    result.IsFailure = true;
                    result.Errors.Add(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Execute the action asynchronous
        /// </summary>
        /// <param name="busTask">The previous asynchronous business task</param>
        /// <param name="action">The action need to be executed asynchronously</param>
        /// <param name="captureContext">true to capture outside context, default is false</param>
        /// <returns>The executed business</returns>
        public static async Task<BusinessExecutor> Execute(this Task<BusinessExecutor> busTask, Func<BusinessExecutor, Task> action, bool captureContext = false)
        {
            var result = await busTask.ConfigureAwait(captureContext);
            if (result.IsSuccess)
            {
                try
                {
                    await action(result).ConfigureAwait(captureContext);
                }
                catch (BusinessException ex)
                {
                    result.IsFailure = true;
                    result.Errors.Add(ex);
                }
            }

            return result;
        }
    }
}
