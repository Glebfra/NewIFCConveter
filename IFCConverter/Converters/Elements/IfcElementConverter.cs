using System.Reflection;
using Ifc.Builders;
using Ifc.Interfaces;
using IFCConverter.Interfaces;
using Start.Attributes;
using Start.Interfaces;
using Utils;
using Xbim.Common;
using Xbim.Ifc.Extensions;
using Xbim.Ifc4.Interfaces;

namespace IFCConverter.Converters.Elements
{
    public abstract class IfcElementConverter<TStart, TIfc> : IIfcElementConverter
        where TStart : IStartEntity
        where TIfc : IIfcProduct, IInstantiableEntity
    {
        protected readonly IModel _Model;
        
        private readonly Logger _logger = Logger.GetInstance();

        protected IfcElementConverter(IModel model)
        {
            _Model = model;
        }
        
        public abstract TIfc BuildIfcElement(TStart start);
        public abstract TStart BuildStartElement(TIfc ifc);
        
        public object BuildIfc(object start)
        {
            return BuildIfcElement((TStart)start);
        }
        
        public object BuildStart(object ifc)
        {
            return BuildStartElement((TIfc)ifc)!;
        }

        protected void TryAddMaterial(TStart start, IIfcProductBuilder<TIfc> builder)
        {
            if (start is IStartMaterializedEntity materializedEntity)
            {
                IIfcMaterialBuilder materialBuilder = new IfcMaterialBuilder(materializedEntity.MaterialName, "", "");
                if (materialBuilder.GetOrCreateMaterial(_Model, out IIfcMaterial material))
                    _logger.Info($"Created material with name: {material.Name}");
                builder.AssignMaterial(material);
            }
        }

        protected string GenerateTag(TStart start)
        {
            return start.GetType().GetCustomAttribute<StartElementAttribute>().Type.ToString();
        }

        protected string GenerateName(TStart start)
        {
            return !start.Name.IsEmpty() 
                ? start.Name 
                : start switch
            {
                IStartTwoNodeEntity twoNodeEntity => $"{start.GetType().Name}_{twoNodeEntity.StartNode.Name}_{twoNodeEntity.EndNode.Name}",
                IStartOneNodeEntity oneNodeEntity => $"{start.GetType().Name}_{oneNodeEntity.Node.Name}",
                _ => $"{start.GetType().Name}_{start.ID}"
            };
        }
    }
}