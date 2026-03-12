using System.Diagnostics.Contracts;

namespace IFCConverter.Interfaces
{
    public interface IIfcElementConverter
    {
        [Pure]
        public object BuildIfc(object start);

        [Pure]
        public object BuildStart(object ifc);
    }
}