using System.Collections;
using System.Collections.Concurrent;
using AutoBogus;
using AutoBogus.Conventions;
using Geordi.Graph;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Geordi.Seeder;

public class Seeder(DbContext context)
{
    public async Task Seed()
    {
        var model = context.Model;

        IGraph<IEntityType, IEntityType> entityGraph = new DirectedAcyclicGraph<IEntityType, IEntityType>();

        var sortedEntityTypes = model.GetEntityTypes()
            .SelectMany(x => x.GetNavigations())
            .Select(x => x.TargetEntityType)
            .OrderBy(x => x.GetForeignKeys().Count())
            .ToHashSet();

        foreach (var entityType in sortedEntityTypes)
        {
            var foreignKeyTypeSet = entityType.GetForeignKeys()
                .Where(x => x.PrincipalEntityType.ClrType != entityType.ClrType)
                .Select(x => x.PrincipalEntityType)
                .ToHashSet()
                .ToArray();

            entityGraph.AddNode(entityType, entityType, foreignKeyTypeSet);
        }

        var (layers, detached) = entityGraph.TopologicalSort();

        var dict = new Dictionary<IEntityType, ConcurrentBag<Guid>>();
        foreach (var entityType in layers.SelectMany(layer => layer))
        {
            var autoFakerType = typeof(AutoFaker<>).MakeGenericType(entityType.ClrType);
            dynamic autoFaker = Activator.CreateInstance(autoFakerType);
            autoFaker.Configure(Configure(entityType, dict));

            var genCount = entityType.GetNavigations()
                .Select(x => x.TargetEntityType)
                .Select(x => entityGraph.GetIncoming(x).Count + entityGraph.GetOutgoing(x).Count)
                .Sum();

            var entityList = autoFaker.Generate(genCount);
            var pk = entityType.FindPrimaryKey().Properties.First();
            var idQueue = new ConcurrentBag<Guid>();
            foreach (var o in entityList as ICollection)
            {
                var props = o.GetType().GetProperties();
                var id = (Guid) props.Single(p => p.Name == pk.Name).GetValue(o);
                for (var i = 0; i < genCount; i++)
                {
                    idQueue.Add(id);
                }
            }

            dict[entityType] = idQueue;

            context.AddRange(entityList);
            await context.SaveChangesAsync();
        }
    }

    private Action<IAutoGenerateConfigBuilder> Configure(IEntityType entityType, Dictionary<IEntityType, ConcurrentBag<Guid>> idDict) =>
        delegate(IAutoGenerateConfigBuilder builder)
        {
             var foreignKeyIdSet = entityType.GetForeignKeys()
                 .Where(x => x.PrincipalEntityType.ClrType == entityType.ClrType)
                 .SelectMany(x => x.Properties)
                 .Select(x => x.Name);

             foreach (var name in foreignKeyIdSet)
             {
                 builder.WithSkip(entityType.ClrType, name);
             }

             var foreignKeyTypeSet = entityType.GetForeignKeys()
                 .Select(x => x.PrincipalEntityType.ClrType)
                 .ToHashSet();

             var navigationTypeSet = entityType.GetNavigations()
                 .Select(x => x.ClrType)
                 .ToHashSet();

             var skipTypes = foreignKeyTypeSet.Concat(navigationTypeSet).ToHashSet();
             foreach (var skipType in skipTypes)
             {
                 builder.WithSkip(skipType);
             }

             builder
                 .WithConventions();

             builder.WithOverride(new ForeignKeyOverride(entityType, idDict));
        };

    private class ForeignKeyOverride(IEntityType entityType, IDictionary<IEntityType, ConcurrentBag<Guid>> idDict) : AutoGeneratorOverride
    {
        public override bool CanOverride(AutoGenerateContext context)
        {
            var foreignKeys = entityType
                .GetNavigations()
                .Select(x => x.ForeignKey.Properties.First().Name)
                .ToHashSet();

            return foreignKeys.Contains(context.GenerateName);
        }

        public override void Generate(AutoGenerateOverrideContext context)
        {
            var matchingForeignKey = entityType
                    .GetNavigations()
                    .Where(x => x.ForeignKey.Properties.First().Name == context.GenerateName)
                    .Select(x => x.TargetEntityType)
                    .First();

            var fkIds = idDict[matchingForeignKey];
            fkIds.TryPeek(out var id);
            context.Instance = id;
        }
    }
}
