using System;
using System.Threading.Tasks;

namespace FnExecutor
{
    public static class BusinessExecutorExtension
    {
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
