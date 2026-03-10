using System.Linq;
using Ifc.Interfaces;
using Utils;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MaterialResource;
using Xbim.Ifc4.MeasureResource;

namespace Ifc.Builders
{
    public class IfcMaterialBuilder : IIfcMaterialBuilder
    {
        public object? Instance { get; private set; }

        public IfcLabel MaterialName { get; }
        public IfcText Description { get; }
        public IfcLabel Category { get; }
        
        private readonly Logger _logger = Logger.GetInstance();

        public IfcMaterialBuilder(IfcLabel materialName, IfcText description, IfcLabel category)
        {
            MaterialName = materialName;
            Description = description;
            Category = category;
        }
        
        public IIfcMaterial CreateMaterial(IModel model)
        {
            const string transactionName = $"{nameof(IfcMaterialBuilder)}: {nameof(CreateMaterial)}";
            _logger.Info($"Begin transaction: {transactionName}");
            
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                Instance = model.Instances.New<IfcMaterial>(ifcMaterial =>
                {
                    ifcMaterial.Name = MaterialName;
                    ifcMaterial.Description = Description;
                    ifcMaterial.Category = Category;
                });
                transaction.Commit();
                
                return (IIfcMaterial)Instance;
            }
        }

        public bool GetOrCreateMaterial(IModel model, out IIfcMaterial material)
        {
            material = model.Instances.OfType<IIfcMaterial>().FirstOrDefault(material => material.Name == MaterialName)!;
            if (material == null!)
            {
                material = CreateMaterial(model);
                return true;
            }
            
            Instance = material;
            return false;
        }
        
        public object Build(IModel model)
        {
            return CreateMaterial(model);
        }
    }
}