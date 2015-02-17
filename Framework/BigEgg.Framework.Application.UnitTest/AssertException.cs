using System;
using System.Runtime.Serialization;

namespace BigEgg.Framework.Application.UnitTesting
{
    /// <summary>
    /// Represents assertion errors that occur at runtime.
    /// </summary>
    [Serializable]
    public class AssertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertException"/> class.
        /// </summary>
        public AssertException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssertException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public AssertException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected AssertException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Represents event sender and obserable not same assertion errors that occur at runtime.
    /// </summary>
    [Serializable]
    public class SenderObservableNotSameException : AssertException
    {
        public SenderObservableNotSameException()
            : base("The sender object of the event isn't the observable")
        { }
    }

    /// <summary>
    /// Represents no event raise assertion error that occur at runtime.
    /// </summary>
    [Serializable]
    public class NoEventRaiseException : AssertException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoEventRaiseException"/> class.
        /// </summary>
        public NoEventRaiseException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoEventRaiseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NoEventRaiseException(string message) : base(message) { }
    }

    /// <summary>
    /// Represents event raise more than once assertion error that occur at runtime.
    /// </summary>
    [Serializable]
    public class EventRaiseMoreThanOnceException : AssertException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventRaiseMoreThanOnceException"/> class.
        /// </summary>
        public EventRaiseMoreThanOnceException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRaiseMoreThanOnceException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EventRaiseMoreThanOnceException(string message) : base(message) { }
    }
}