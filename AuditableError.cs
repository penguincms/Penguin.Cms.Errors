using Penguin.Auditing.Abstractions.Attributes;
using Penguin.Cms.Entities;
using Penguin.Extensions.Exceptions;
using Penguin.Persistence.Abstractions.Attributes.Control;
using Penguin.Persistence.Abstractions.Attributes.Validation;
using System;
using System.Diagnostics.Contracts;

namespace Penguin.Cms.Errors
{
    /// <summary>
    /// Represents an auditable error to be persisted
    /// </summary>
    [DontAuditChanges]
    public class AuditableError : Entity
    {
        /// <summary>
        /// The generated int Id for the error provided by the environment
        /// </summary>
        [DontAllow(DisplayContexts.List | DisplayContexts.Edit)]
        public long ErrorId { get; set; }

        /// <summary>
        /// The message provided with the error
        /// </summary>

        [DontAllow(DisplayContexts.Edit | DisplayContexts.BatchEdit)]
        public string Exception { get; set; }

        /// <summary>
        /// The full name of the type of error that was thrown
        /// </summary>

        [DontAllow(DisplayContexts.List | DisplayContexts.Edit)]
        public string ExceptionType { get; set; }

        /// <summary>
        /// Any form values that were posted if this was a web request.
        /// </summary>

        [DontAllow(DisplayContexts.List | DisplayContexts.Edit)]
        public string FormValues { get; set; }

        /// <summary>
        /// The top level inner exception message
        /// </summary>

        [DontAllow(DisplayContexts.Edit | DisplayContexts.BatchEdit)]
        public string InnerException { get; set; }

        /// <summary>
        /// If a web exception, this is the referrer for the request
        /// </summary>

        [DontAllow(DisplayContexts.List | DisplayContexts.Edit)]
        public string RawReferrer { get; set; }

        /// <summary>
        /// If a web exception, this is the Url that was requested
        /// </summary>

        [DontAllow(DisplayContexts.List | DisplayContexts.Edit)]
        public string RawRequestUrl { get; set; }

        /// <summary>
        /// If a web exception, this is the referrer for the request
        /// </summary>

        [DontAllow(DisplayContexts.List | DisplayContexts.Edit)]
        public string Referrer { get; set; }

        /// <summary>
        /// If a web exception, this is the Url that was requested
        /// </summary>

        [DontAllow(DisplayContexts.Edit | DisplayContexts.BatchEdit)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// The stack trace provided by the environment
        /// </summary>
        [MaxLength]
        [DontAllow(DisplayContexts.List | DisplayContexts.Edit)]
        public string StackTrace { get; set; }

        /// <summary>
        /// An ID representing the user that caused the error, or 0 if unknown
        /// </summary>

        [DontAllow(DisplayContexts.List | DisplayContexts.Edit)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Creates a new instance of this error class
        /// </summary>
        public AuditableError()
        {
            this.UserId = Guid.Empty;
            this.DateCreated = DateTime.Now;
        }

        /// <summary>
        /// Creates a new instance of this error class, and then fills it with information
        /// </summary>
        /// <param name="ex">The exception to use when filling this instance</param>
        public AuditableError(Exception ex)
        {
            Contract.Requires(ex != null);

            this.UserId = Guid.Empty;
            this.DateCreated = DateTime.Now;
            this.StackTrace = string.Empty;
            Type exceptionType = ex.GetType();
            this.ExceptionType = exceptionType.ToString();

            if (ex.InnerException != null)
            {
                this.InnerException = ex.InnerException.RecursiveMessage();
            }

            this.Exception = ex.RecursiveMessage();
            this.StackTrace += ex.RecursiveStackTrace();
        }
    }
}