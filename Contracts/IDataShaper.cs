using Entities.Models;
using System.Dynamic;

namespace Contracts
{
    public interface IDataShaper<T>
    {
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldtring);
        ShapedEntity ShapeData(T entity, string fieldString);
    }
}
