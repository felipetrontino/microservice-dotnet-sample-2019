using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using Framework.Core.Bus;
using Framework.Core.Entities;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace Framework.Test.Extensions
{
    public static class ObjectAssertionsExtensions
    {
        public static void BeEquivalentToMessage<T>(this ObjectAssertions assertions, T expectation, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config = null)
            where T : class, IBusMessage
        {
            BeEquivalentToModel(assertions, expectation, x =>
            {
                config?.Invoke(x);
                x.Excluding(y => y.SelectedMemberPath.Contains(nameof(IBusMessage.MessageId)));

                return x;
            });
        }

        public static void BeEquivalentToMessage<T>(this GenericCollectionAssertions<T> assertions, IEnumerable<T> expectation, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config = null)
            where T : class, IBusMessage
        {
            BeEquivalentToModel(assertions, expectation, x =>
            {
                config?.Invoke(x);
                x.Excluding(y => y.SelectedMemberPath.Contains(nameof(IBusMessage.MessageId)));

                return x;
            });
        }

        public static void BeEquivalentToEntity<T>(this ObjectAssertions assertions, T expectation, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config = null)
           where T : class, IEntity
        {
            BeEquivalentToModel(assertions, expectation, x =>
            {
                x.Excluding(y => y.SelectedMemberPath.Contains(nameof(IAuditEntity.InsertedAt)));
                x.Excluding(y => y.SelectedMemberPath.Contains(nameof(IAuditEntity.UpdatedAt)));
                x.Excluding(y => y.SelectedMemberPath.Contains(nameof(IAuditEntity.DeletedAt)));
                x.IgnoringCyclicReferences();

                config?.Invoke(x);

                return x;
            });
        }

        public static void BeEquivalentToEntity<T>(this GenericCollectionAssertions<T> assertions, IEnumerable<T> expectation, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config = null)
            where T : class, IEntity
        {
            BeEquivalentToModel(assertions, expectation, x =>
            {
                x.Excluding(y => y.SelectedMemberPath.Contains(nameof(IAuditEntity.InsertedAt)));
                x.Excluding(y => y.SelectedMemberPath.Contains(nameof(IAuditEntity.UpdatedAt)));
                x.Excluding(y => y.SelectedMemberPath.Contains(nameof(IAuditEntity.DeletedAt)));
                x.IgnoringCyclicReferences();

                config?.Invoke(x);

                return x;
            });
        }

        private static void BeEquivalentToModel<T>(this ObjectAssertions assertions, T expectation, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config = null)
        {
            assertions.BeEquivalentTo(expectation, x =>
            {
                x.Excluding(y => y.SelectedMemberInfo.Name.StartsWith(nameof(IEntity.Id)));
                x.Excluding(y => y.RuntimeType == typeof(Geometry));
                x.Using<Geometry>(y => y.Subject.Centroid.Should().Be(y.Expectation.Centroid)).WhenTypeIs<Geometry>();

                config?.Invoke(x);

                return x;
            });
        }

        private static void BeEquivalentToModel<T>(this GenericCollectionAssertions<T> assertions, IEnumerable<T> expectation, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config = null)
        {
            assertions.BeEquivalentTo(expectation, x =>
            {
                x.Excluding(y => y.SelectedMemberInfo.Name.StartsWith(nameof(IEntity.Id)));
                x.Excluding(y => y.RuntimeType == typeof(Geometry));
                x.Using<Geometry>(y => y.Subject.Centroid.Should().Be(y.Expectation.Centroid)).WhenTypeIs<Geometry>();

                config?.Invoke(x);

                return x;
            });
        }
    }
}