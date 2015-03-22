using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Extensions;

namespace Common
{
    /// <summary>
    /// Provides methods that transform imperative (procedural) practices into functional (declarative) ones.
    /// </summary>
    public static class Functional
    {
        /// <summary>
        /// The standard for-loop as a method.
        /// </summary>
        /// <typeparam name="T">The type of the loop variable.</typeparam>
        /// <param name="initializer">The initial value of the looping variable.</param>
        /// <param name="condition">The condition for which the loop will persist.</param>
        /// <param name="iterator">The method to iterate the loop variable.
        /// Unlike standard for-loops, this method returns a value to reassign the loop variable;
        /// meaning, the looping variable must be reassigned every iteration. This is not a requirement of standard for-loops.</param>
        /// <param name="body">The body of the for-loop. Only allows for 'continue' via a 'return' within the method.
        /// Use the overloaded method with Func instead of Action to be able to 'break'.</param>
        /// <exception cref="ArgumentNullException">Thrown if condition, iterator, or body are null.</exception>
        public static void ForLoop<T>(T initializer, Func<T, bool> condition, Func<T, T> iterator, Action<T> body)
        {
            body.ThrowIfNull();

            ForLoop(initializer, condition, iterator, i => { body(i); return true; });
        }

        /// <summary>
        /// The standard for-loop as a method.
        /// </summary>
        /// <typeparam name="T">The type of the loop variable.</typeparam>
        /// <param name="initializer">The initial value of the looping variable.</param>
        /// <param name="condition">The condition for which the loop will persist.</param>
        /// <param name="iterator">The method to iterate the loop variable.
        /// Unlike standard for-loops, this method returns a value to reassign the loop variable;
        /// meaning, the looping variable must be reassigned every iteration. This is not a requirement of standard for-loops.</param>
        /// <param name="body">The body of the for-loop. Allows 'continue' via a 'return true' within the method and
        /// 'break' via a 'return false' within the method.</param>
        public static void ForLoop<T>(T initializer, Func<T, bool> condition, Func<T, T> iterator, Func<T, bool> body)
        {
            condition.ThrowIfNull();
            iterator.ThrowIfNull();
            body.ThrowIfNull();

            for (var i = initializer; condition(i); i = iterator(i))
            {
                if (!body(i)) break;
            }
        }

        /// <summary>
        /// A using statement as a method.
        /// </summary>
        /// <typeparam name="T">The type of disposable item.</typeparam>
        /// <param name="disposableItem">The item that requires disposing after use.</param>
        /// <param name="body">The body of the using statement.</param>
        public static void UsingStatement<T>(T disposableItem, Action<T> body) where T : IDisposable
        {
            body.ThrowIfNull();

            UsingStatement(disposableItem, i => { body(i); return true; });
        }

        /// <summary>
        /// A using statement as a method.
        /// </summary>
        /// <typeparam name="TDisposable">The type of disposable item.</typeparam>
        /// <typeparam name="TResult">The type of the result from the body. This should not implement IDisposable.</typeparam>
        /// <param name="disposableItem">The item that requires disposing after use.</param>
        /// <param name="body">The body of the using statement. It returns the result of the operations done within the disposable item.</param>
        /// <returns>The result of type TResult of the body is returned.</returns>
        public static TResult UsingStatement<TDisposable, TResult>(TDisposable disposableItem, Func<TDisposable, TResult> body)
            where TDisposable : IDisposable
        {
            body.ThrowIfNull();

            using (var item = disposableItem)
            {
                return body(item);
            }
        }
    }
}