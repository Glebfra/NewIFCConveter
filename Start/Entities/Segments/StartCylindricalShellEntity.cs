using Start.API;
using Start.Attributes;

namespace Start.Entities.Segments
{
    /// <summary>
    /// Represents a cylindrical shell entity in the Start framework.
    /// Inherits from the <see cref="StartPipeEntity"/> class.
    /// </summary>
    [StartElement(StartElementTypeEnum.CYLINDRICAL_SHELL)]
    public class StartCylindricalShellEntity : StartPipeEntity
    {
    }
}