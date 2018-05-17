using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyVendorName.MyAppName
{
    /// <summary>
    /// Common base class for controllers that conform to the "collection" REST pattern.
    /// </summary>
    /// <typeparam name="T">The DTO type used to represent elements of the collection.</typeparam>
    public abstract class CollectionController<T> : Controller
    {
        private readonly ICrudService<T> _service;

        protected CollectionController(ICrudService<T> service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all elements.
        /// </summary>
        [HttpGet, Route("")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<T>> ReadAll()
            => await _service.ReadAllAsync();

        /// <summary>
        /// Returns a specific element.
        /// </summary>
        /// <param name="id">The ID of the element to look for.</param>
        [HttpGet, Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified element not found.")]
        public async Task<T> Read([FromRoute] string id)
            => await _service.ReadAsync(id);

        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="element">The element to create (without an ID).</param>
        /// <returns>The element that was created (with the ID).</returns>
        [HttpPost, Route("")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, description: "Missing or invalid request body.")]
        public async Task<ActionResult> Create([FromBody] T element)
        {
            if (element == null) throw new InvalidDataException("Missing request body.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.CreateAsync(element);

            return CreatedAtAction(
                actionName: nameof(Read),
                routeValues: new {id = GetId(result)},
                value: result);
        }

        /// <summary>
        /// Updates an existing element.
        /// </summary>
        /// <param name="id">The ID of the element to update (must match the ID in <paramref name="element"/>).</param>
        /// <param name="element">The modified element.</param>
        [HttpPut, Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, description: "Missing or invalid request body.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified element not found.")]
        public async Task<ActionResult> Set([FromRoute] string id, [FromBody] T element)
        {
            if (element == null) throw new InvalidDataException("Missing request body.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (GetId(element) != id) throw new InvalidDataException($"ID in URI ({id}) must match the ID in the body ({GetId(element)}).");

            await _service.UpdateAsync(element);

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes an existing element.
        /// </summary>
        /// <param name="id">The ID of the element to delete.</param>
        [HttpDelete, Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified element not found.")]
        public async Task Delete([FromRoute] string id)
            => await _service.DeleteAsync(id);

        protected static string GetId(T entity) => GetIdMethod.Invoke(entity, null).ToString();

        // ReSharper disable once StaticMemberInGenericType
        private static readonly MethodInfo GetIdMethod;

        static CollectionController()
        {
            GetIdMethod = typeof(T).GetTypeInfo()
                                   .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                   .First(x => x.GetMethod != null && x.GetCustomAttribute<KeyAttribute>(inherit: true) != null)
                                   .GetMethod;
        }
    }
}
