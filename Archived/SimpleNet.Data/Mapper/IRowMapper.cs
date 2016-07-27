using System.Data;

namespace SimpleNet.Data.Mapper
{
    /// <summary>
    /// Represents the operation of mapping a <see cref="IDataRecord"/> to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type this row mapper will be mapping to.</typeparam>
    /// <seealso cref="ReflectionRowMapper&lt;TResult&gt;"/>
    public interface IRowMapper<out TResult>
    {
        /// <summary>
        /// When implemented by a class, returns a new <typeparamref name="TResult"/> based on <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The <see cref="IDataRecord"/> to map.</param>
        /// <returns>The instance of <typeparamref name="TResult"/> that is based on <paramref name="row"/>.</returns>
        TResult MapRow(IDataRecord row);

    }
}
