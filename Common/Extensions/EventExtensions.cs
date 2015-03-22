using System;

namespace Common.Extensions
{
    /// <summary>
    /// A generic event handler with no event arguments
    /// </summary>
    /// <param name="sender">The instance of an object that sent the event.</param>
    public delegate void GenericEventHandler(object sender);

    /// <summary>
    /// A generic event handler with one event argument.
    /// </summary>
    /// <typeparam name="TA">The type of the event arguments.</typeparam>
    /// <param name="sender">The instance of an object that sent the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void GenericEventHandler<in TA>(object sender, TA e);

    /// <summary>
    /// Class to assist with event handling.
    /// Note: Here is the reasons/rationale behind EventArgs if you wanted to know.
    /// http://stackoverflow.com/a/1142081/294804
    /// Generic EventArgs: http://codereview.stackexchange.com/questions/5470/generic-eventargs-to-go-with-generic-eventhandler
    /// </summary>
    public static class EventExtensions
    {
        /// <summary>
        /// Raises an event. Contains null protection.
        /// http://stackoverflow.com/questions/1609430/copying-delegates
        /// </summary>
        /// <param name="handler">The event handler to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        /// <returns>True if the event was raised successfully.</returns>
        public static bool Raise(this EventHandler handler, object sender, EventArgs e = null)
        {
            return handler.Raise(h => h(sender, e));
        }

        /// <summary>
        /// Raises an event. Contains null protection.
        /// http://stackoverflow.com/questions/1609430/copying-delegates
        /// </summary>
        /// <param name="handler">The event handler to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        /// <typeparam name="TEventArgs">The event argument as a type of EventArgs.</typeparam>
        /// <returns>True if the event was raised successfully.</returns>
        public static bool Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e = null)
            where TEventArgs : EventArgs
        {
            return handler.Raise(h => h(sender, e));
        }

        /// <summary>
        /// A generic way to raise events. Contains null protection.
        /// http://stackoverflow.com/questions/1609430/copying-delegates
        /// </summary>
        /// <param name="handler">The generic event handler instance.</param>
        /// <param name="sender">The instance of an object that sent the event.</param>
        /// <returns>True if the event was raised successfully.</returns>
        public static bool Raise(this GenericEventHandler handler, object sender)
        {
            return handler.Raise(h => h(sender));
        }

        /// <summary>
        /// A generic way to raise events. Contains null protection.
        /// http://stackoverflow.com/questions/1609430/copying-delegates
        /// </summary>
        /// <typeparam name="TA">The type of the event arguments.</typeparam>
        /// <param name="handler">The generic event handler instance.</param>
        /// <param name="sender">The instance of an object that sent the event.</param>
        /// <param name="e">The event arguments</param>
        /// <returns>True if the event was raised successfully.</returns>
        public static bool Raise<TA>(this GenericEventHandler<TA> handler, object sender, TA e)
        {
            return handler.Raise(h => h(sender, e));
        }

        private static bool Raise<T>(this T handler, Action<T> executeHandler)
        {
            var handlerCopy = handler;
            if (!handlerCopy.IsNull())
            {
                executeHandler(handlerCopy);
                return true;
            }

            return false;
        }
    }
}