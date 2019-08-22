using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DCCS.ExceptionHelpers.NetStandard
{
    /// <summary>
    /// Extensions for the exception
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Returns the <see cref="exception"/> and all inner exceptions, beginning with the outer most exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>All exceptions</returns>
        public static IEnumerable<Exception> GetAllExceptionsInHirachy(this Exception exception)
        {
            yield return exception;
            if (exception is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    foreach (var inner in GetAllExceptionsInHirachy(innerException))
                    {
                        yield return inner;
                    }
                }
            }
            else
            {
                if (exception.InnerException != null)
                {
                    foreach (var inner in GetAllExceptionsInHirachy(exception.InnerException))
                    {
                        yield return inner;
                    }
                }
            }
        }

        /// <summary>
        /// Return the message of the <see cref="exception"/> and all inner exceptions.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="includeCallstack">If true, the callstack will be included.</param>
        /// <param name="separator">separator for the messages. If null, <see>Environment.NewLine</see> will be used. This parameter will not be used for the formatting of the stacktrace. The stacktrace will always be added with new lines.</param>
        /// <returns>Whole message</returns>
        public static string GetRecursiveMessage(this Exception exception, bool includeCallstack, string separator = null)
        {
            if (separator == null)
                separator = Environment.NewLine;
            if (!includeCallstack)
                return string.Join(separator, exception.GetAllExceptionsInHirachy().Select(ex => ex.Message));
            else
            {
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.Join(separator, exception.GetAllExceptionsInHirachy().Select(ex => ex.Message));
                messageBuilder.AppendLine();
                messageBuilder.Join(Environment.NewLine, exception.GetAllExceptionsInHirachy().Select(ex => ex.StackTrace));
                return messageBuilder.ToString();
            }
        }

        private static void Join(this StringBuilder builder, string separator, IEnumerable elements, CultureInfo cultureInfo = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            bool first = true;
            foreach (var entry in elements)
            {
                if (first)
                    first = false;
                else
                    builder.Append(separator);
                builder.AppendFormat(cultureInfo, "{0}", entry);
            }
        }
    }
}
