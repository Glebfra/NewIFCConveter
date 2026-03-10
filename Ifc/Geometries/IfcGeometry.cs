using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Ifc.API;
using Ifc.Attributes;
using Ifc.Interfaces;
using Xbim.Common;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.PresentationAppearanceResource;
using Xbim.Ifc4.RepresentationResource;

namespace Ifc.Geometries
{
    public abstract class IfcGeometry : IIfcGeometry
    {
        public IEnumerable<IIfcBuilder> GeometryBuilders { get; }
        public IIfcRepresentationContext? RepresentationContext { get; }
        public IColor? Color { get; private set; }
        
        public IfcGeometry(IIfcBuilder geometryBuilder, IIfcRepresentationContext? representationContext = null)
        {
            GeometryBuilders = new[] { geometryBuilder };
            RepresentationContext = representationContext;
        }

        public IfcGeometry(IEnumerable<IIfcBuilder> geometryBuilders,
            IIfcRepresentationContext? representationContext = null)
        {
            GeometryBuilders = geometryBuilders;
            RepresentationContext = representationContext;
        }

        [Pure]
        public IIfcShapeRepresentation CreateShapeRepresentation(IModel model)
        {
            IfcRepresentationItem[] representationItems = GeometryBuilders
                .Select(builder => builder.Build(model))
                .Cast<IfcRepresentationItem>()
                .ToArray();

            if (Color != null)
                StyleItems(model, (Color)Color, representationItems);

            Type geometryType = GetType();
            IfcRepresentationIdentifierAttribute representationIdentifierAttribute =
                geometryType.GetCustomAttribute<IfcRepresentationIdentifierAttribute>();
            IfcRepresentationTypeAttribute representationTypeAttribute =
                geometryType.GetCustomAttribute<IfcRepresentationTypeAttribute>();

            string representationIdentifier = representationIdentifierAttribute.RepresentationIdentifier.ToString();
            string representationType = representationTypeAttribute.IfcRepresentationType.ToString();

            const string transactionName = $"{nameof(IfcGeometry)}: {nameof(CreateShapeRepresentation)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                IfcShapeRepresentation shapeRepresentation = model.Instances.New<IfcShapeRepresentation>(
                    representation =>
                    {
                        representation.ContextOfItems = RepresentationContext as IfcRepresentationContext ??
                                                        model.Instances.OfType<IfcGeometricRepresentationContext>()
                                                            .FirstOrDefault();
                        representation.RepresentationIdentifier = representationIdentifier;
                        representation.RepresentationType = representationType;
                        representation.Items.AddRange(representationItems);
                    });
                transaction.Commit();

                return shapeRepresentation;
            }
        }

        [Pure]
        public IIfcProductDefinitionShape CreateProductDefinitionShape(IModel model,
            IIfcShapeRepresentation shapeRepresentation)
        {
            const string transactionName = $"{nameof(IfcGeometry)}: {nameof(CreateProductDefinitionShape)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                IfcProductDefinitionShape productDefinitionShape = model.Instances.New<IfcProductDefinitionShape>(
                    shape => shape.Representations.Add((IfcShapeRepresentation)shapeRepresentation)
                );
                transaction.Commit();

                return productDefinitionShape;
            }
        }

        public void AssignColor(IColor color)
        {
            Color = color;
        }

        private void StyleItems(IModel model, IColor color, IEnumerable<IIfcRepresentationItem> representationItems)
        {
            IIfcColourRgb colourRgb = CreateColourRgb(model, color);
            IIfcSurfaceStyleShading surfaceStyleShading = CreateSurfaceStyleShading(model, colourRgb);
            IIfcSurfaceStyle surfaceStyle = CreateSurfaceStyle(model, surfaceStyleShading);
            
            const string transactionName = $"{nameof(IfcGeometry)}: {nameof(StyleItems)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                foreach (IIfcRepresentationItem ifcRepresentationItem in representationItems)
                {
                    model.Instances.New<IfcStyledItem>(item =>
                    {
                        item.Item = (IfcRepresentationItem)ifcRepresentationItem;
                        item.Styles.Add(surfaceStyle);
                    });
                }
                transaction.Commit();
            }
        }

        [Pure]
        private static IIfcColourRgb CreateColourRgb(IModel model, IColor color)
        {
            double[] normalizedColour = color.ToNormal();

            const string transactionName = $"{nameof(IfcGeometry)}: {nameof(CreateColourRgb)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                IfcColourRgb colourRgb = model.Instances.New<IfcColourRgb>(colourRgb =>
                {
                    colourRgb.Red = normalizedColour[0];
                    colourRgb.Green = normalizedColour[1];
                    colourRgb.Blue = normalizedColour[2];
                });
                transaction.Commit();
                
                return colourRgb;
            }
        }

        [Pure]
        private static IIfcSurfaceStyleShading CreateSurfaceStyleShading(IModel model, IIfcColourRgb colourRgb)
        {
            const string transactionName = $"{nameof(IfcGeometry)}: {nameof(CreateSurfaceStyleShading)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                IIfcSurfaceStyleShading styleShading = model.Instances.New<IfcSurfaceStyleShading>(shading =>
                {
                    shading.SurfaceColour = (IfcColourRgb)colourRgb;
                });
                transaction.Commit();

                return styleShading;
            }
        }
        
        [Pure]
        private static IIfcSurfaceStyle CreateSurfaceStyle(IModel model, IfcSurfaceStyleElementSelect surfaceStyleElementSelect)
        {
            const string transactionName = $"{nameof(IfcGeometry)}: {nameof(CreateSurfaceStyle)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                IIfcSurfaceStyle surfaceStyle = model.Instances.New<IfcSurfaceStyle>(style =>
                {
                    style.Styles.Add(surfaceStyleElementSelect);
                    style.Side = IfcSurfaceSide.BOTH;
                });
                transaction.Commit();

                return surfaceStyle;
            }
        }
    }
}