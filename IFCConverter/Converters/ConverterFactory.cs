using System.Diagnostics.Contracts;
using IFCConverter.Converters.Elements;
using IFCConverter.Interfaces;
using Start.Entities.ExpansionJoints;
using Start.Entities.Fittings;
using Start.Entities.Segments;
using Start.Interfaces;
using Xbim.Common;

namespace IFCConverter.Converters
{
    public static class ConverterFactory
    {
        [Pure]
        public static IIfcElementConverter? CreateConverter(IModel model, IStartEntity startEntity)
        {
            return startEntity switch
            {
                StartAngularExpansionJointEntity => new AngularExpansionJointConverter(model),

                StartValveEntity => new ValveConverter(model),
                StartAbstractReducerEntity => new ReducerConverter(model),
                StartAbstractBendEntity => new BendConverter(model),
                StartAbstractTeeEntity => new TeeConverter(model),

                StartConeElementEntity => new ConeElementConverter(model),
                StartAbstractSegmentEntity => new PipeConverter(model),
                _ => null
            };
        }
    }
}