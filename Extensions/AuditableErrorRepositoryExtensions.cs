using Penguin.Persistence.Abstractions.Interfaces;
using System;

namespace Penguin.Cms.Errors.Extensions
{
    public static class AuditableErrorRepositoryExtensions
    {
        /// <summary>
        /// Adds a new exception to the underlying storage context
        /// </summary>
        /// <param name="errorRepository"></param>
        /// <param name="ex">The exception to add</param>
        /// <param name="url">If relevant, any URL associated with the error</param>
        /// <param name="UserId">If relevant, the ID of the user that generated the error</param>
        public static void Add(this IRepository<AuditableError> errorRepository, Exception ex, string url = null, Guid? UserId = null)
        {
            if (errorRepository is null)
            {
                throw new ArgumentNullException(nameof(errorRepository));
            }

            AuditableError thisError = new(ex)
            {
                RequestUrl = url,
                UserId = UserId ?? Guid.Empty
            };

            errorRepository.Add(thisError);
        }

        /// <summary>
        /// Opens a new writecontext, attempts to add an error, and closes the write context.
        /// If the context was already open, it may not persist until it closes.
        /// Can be called on a null repository. This will be treated as an error and caught/rethrown as
        /// instructed by the "rethrow" parameter
        /// </summary>
        /// <param name="errorRepository">The Error repository to attempt to use as a source</param>
        /// <param name="ex">The exception</param>
        /// <param name="rethrow">If theres an error writing the error, should the error be rethrown?</param>
        /// <param name="url">If relevant, any URL associated with the error</param>
        /// <param name="UserId">If relevant, the ID of the user that generated the error</param>
        /// <returns>False if an error occurs, regardless of whether or not it was rethrown</returns>
        public static bool TryAdd(this IRepository<AuditableError> errorRepository, Exception ex, bool rethrow = false, string url = null, Guid? UserId = null)
        {
            try
            {
                using (IWriteContext context = errorRepository?.WriteContext())
                {
                    AuditableError thisError = new(ex)
                    {
                        RequestUrl = url,
                        UserId = UserId ?? Guid.Empty
                    };

                    errorRepository.Add(thisError);
                }

                return true;
            }
            catch (Exception)
            {
                if (rethrow)
                {
                    throw;
                }

                return false;
            }
        }

        /// <summary>
        /// Opens a new writecontext, attempts to add an error, and closes the write context.
        /// If the context was already open, it may not persist until it closes.
        /// Can be called on a null repository. This will be treated as an error and caught/rethrown as
        /// instructed by the "rethrow" parameter
        /// </summary>
        /// <param name="errorRepository">The Error repository to attempt to use as a source</param>
        /// <param name="error">The exception</param>
        /// <param name="rethrow">If theres an error writing the error, should the error be rethrown?</param>
        /// <param name="url">If relevant, any URL associated with the error</param>
        /// <param name="UserId">If relevant, the ID of the user that generated the error</param>
        /// <returns>False if an error occurs, regardless of whether or not it was rethrown</returns>
        public static bool TryAdd(this IRepository<AuditableError> errorRepository, string error, bool rethrow = false, string url = null, Guid? UserId = null)
        {
            return errorRepository.TryAdd(new Exception(error), rethrow, url, UserId);
        }
    }
}