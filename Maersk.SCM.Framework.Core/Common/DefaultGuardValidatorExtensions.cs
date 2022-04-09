using Maersk.SCM.Framework.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maersk.SCM.Framework.Core.Common
{
    public static class DefaultGuardValidatorExtensions
    {
        public static string VerifyOrThrowExceptionWhenEmptyString(this string stringToValidate, string message)
        {
            if (stringToValidate == string.Empty)
            {
                throw new DomainEntityArgumentException(message);
            }

            return stringToValidate;
        }

        public static string VerifyOrThrowException(this string stringToValidate, string message)
        {
            if (string.IsNullOrWhiteSpace(stringToValidate))
            {
                throw new DomainEntityArgumentException(message);
            }

            return stringToValidate;
        }

        public static int VerifyOrThrowException(this int intToValidate, string message)
        {
            if (intToValidate == default)
            {
                throw new DomainEntityArgumentException(message);
            }

            return intToValidate;
        }

        public static long VerifyOrThrowException(this long intToValidate, string message)
        {
            if (intToValidate == default)
            {
                throw new DomainEntityArgumentException(message);
            }

            return intToValidate;
        }

        public static decimal VerifyOrThrowException(this decimal decimalToValidate, string message)
        {
            if (decimalToValidate == default)
            {
                throw new DomainEntityArgumentException(nameof(decimalToValidate));
            }

            return decimalToValidate;
        }

        public static Guid VerifyOrThrowException(this Guid guidToValidate, string message)
        {
            if (guidToValidate == default)
            {
                throw new DomainEntityArgumentException(nameof(guidToValidate));
            }

            return guidToValidate;
        }

        public static DateTimeOffset VerifyOrThrowException(this DateTimeOffset dateTimeOffsetToValidate, string message)
        {
            if (dateTimeOffsetToValidate == default)
            {
                throw new DomainEntityArgumentException(nameof(dateTimeOffsetToValidate));
            }

            return dateTimeOffsetToValidate;
        }

        public static DateTime VerifyOrThrowException(this DateTime dateTimeToValidate, string message)
        {
            if (dateTimeToValidate == default)
            {
                throw new DomainEntityArgumentException(nameof(dateTimeToValidate));
            }

            return dateTimeToValidate;
        }

        public static ICollection VerifyOrThrowException(this ICollection collectionToValidate, string message)
        {
            if (collectionToValidate == null || collectionToValidate.Count == 0)
            {
                throw new DomainEntityArgumentException(nameof(collectionToValidate));
            }

            return collectionToValidate;
        }

        public static T VerifyOrThrowException<T>(this T objectToValidate, string message) where T : class
        {
            if (objectToValidate == default)
            {
                throw new DomainEntityArgumentException($"{nameof(objectToValidate)} - {message}");
            }

            return objectToValidate;
        }
    }
}
