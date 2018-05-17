using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyVendorName.MyAppName
{
    /// <summary>
    /// Common base interface for services that conform to the Create-Read-Update-Delete pattern.
    /// </summary>
    /// <typeparam name="T">The DTO type used to represent elements of the service.</typeparam>
    public interface ICrudService<T>
    {
        /// <summary>
        /// Returns all elements.
        /// </summary>
        Task<IEnumerable<T>> ReadAllAsync();

        /// <summary>
        /// Returns a specific element.
        /// </summary>
        /// <param name="id">The ID of the element to look for.</param>
        /// <exception cref="KeyNotFoundException">Specified element not found.</exception>
        Task<T> ReadAsync(string id);

        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="element">The element to create (without an ID).</param>
        /// <returns>The element that was created (with the ID).</returns>
        Task<T> CreateAsync(T element);

        /// <summary>
        /// Updates an existing element.
        /// </summary>
        /// <param name="element">The modified element.</param>
        /// <exception cref="KeyNotFoundException">Specified element not found.</exception>
        Task UpdateAsync(T element);

        /// <summary>
        /// Deletes an existing element.
        /// </summary>
        /// <param name="id">The ID of the element to delete.</param>
        /// <exception cref="KeyNotFoundException">Specified element not found.</exception>
        Task DeleteAsync(string id);
    }
}
