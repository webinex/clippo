using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Webinex.Clippo.Ports.Model;

namespace Webinex.Clippo.Adapters.EfCore
{
    internal static class ClippoModelExtensions
    {
        public static Expression<Func<TClip, bool>> WhereDirectoryEquals<TClip>(
            this IClippoModelDefinition<TClip> model,
            string directory)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));
            directory = directory ?? throw new ArgumentNullException(nameof(directory));

            return ClippoExpressions.Equals(model.Directory, directory);
        }

        public static Expression<Func<TClip, bool>> WhereOwnerTypeEquals<TClip>(
            this IClippoModelDefinition<TClip> model,
            string ownerType)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));
            ownerType = ownerType ?? throw new ArgumentNullException(nameof(ownerType));

            return ClippoExpressions.Equals(model.OwnerType, ownerType);
        }

        public static Expression<Func<TClip, bool>> WhereOwnerIdEquals<TClip>(
            this IClippoModelDefinition<TClip> model,
            string ownerId)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));
            ownerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));

            return ClippoExpressions.Equals(model.OwnerId, ownerId);
        }

        public static Expression<Func<TClip, bool>> WhereActive<TClip>(
            this IClippoModelDefinition<TClip> model,
            bool active)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            return ClippoExpressions.Equals(model.Active, active);
        }

        public static Expression<Func<TClip, bool>> WhereIdIn<TClip>(
            this IClippoModelDefinition<TClip> model,
            IEnumerable<string> ids)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));
            ids = ids ?? throw new ArgumentNullException(nameof(ids));

            return ClippoExpressions.Contains(model.Id, ids);
        }
    }
}