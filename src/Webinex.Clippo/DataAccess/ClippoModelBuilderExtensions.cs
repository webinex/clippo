using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webinex.Clippo;

public class ClippoModelConfiguration<TMeta, TData>
    where TMeta : class, ICloneable
    where TData : class, ICloneable
{
    private string _tableName = "Clippo_Rows";
    private string _schemaName = "dbo";
    private Action<OwnedNavigationBuilder>? _dataOwnNavigationBuilder;
    private Action<OwnedNavigationBuilder>? _metaOwnNavigationBuilder;

    public ClippoModelConfiguration<TMeta, TData> ToTable(string tableName, string schemaName = "dbo")
    {
        _tableName = tableName;
        _schemaName = schemaName;
        return this;
    }

    public ClippoModelConfiguration<TMeta, TData> OwnsData(Action<OwnedNavigationBuilder> own)
    {
        _dataOwnNavigationBuilder = own;
        return this;
    }

    public ClippoModelConfiguration<TMeta, TData> OwnsMeta(Action<OwnedNavigationBuilder> own)
    {
        _metaOwnNavigationBuilder = own;
        return this;
    }

    internal void Apply(ModelBuilder model)
    {
        model.Entity<VRow<TMeta, TData>>(
            row =>
            {
                row.ToTable(_tableName, _schemaName);

                row.HasKey(x => x.Id);
                row.Property(x => x.Id).HasMaxLength(500).IsRequired();
                row.Property(x => x.Type).IsRequired();
                row.Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
                row.Property(x => x.Name).HasColumnName("Name").IsRequired(false).HasMaxLength(500);
                row.Property(x => x.Bytes).HasColumnName("Bytes").IsRequired(false);
                row.Property(x => x.Ref).HasColumnName("Ref").IsRequired(false).HasMaxLength(250);
                row.Property(x => x.MimeType).HasColumnName("MimeType").IsRequired(false).HasMaxLength(250);

                row.OwnsOne(
                    x => x.Folder,
                    vFolder =>
                    {
                        vFolder.Property(x => x.Type).HasColumnName("Folder_Type");
                        vFolder.Property(x => x.Id).HasColumnName("Folder_Id");
                    });

                if (_dataOwnNavigationBuilder != null)
                {
                    row.OwnsOne(x => x.Data, x => _dataOwnNavigationBuilder(x));
                    row.Navigation(x => x.Data).IsRequired();
                }
                else if (typeof(TData) != typeof(VNone))
                {
                    row.OwnsOne(x => x.Data);
                    row.Navigation(x => x.Data).IsRequired();
                }
                else
                {
                    row.Ignore(x => x.Data);
                }

                if (_metaOwnNavigationBuilder != null)
                {
                    row.OwnsOne(x => x.Meta, x => _metaOwnNavigationBuilder(x));
                    row.Navigation(x => x.Meta).IsRequired();
                }
                else if (typeof(TMeta) != typeof(VNone))
                {
                    row.OwnsOne(x => x.Meta);
                    row.Navigation(x => x.Meta).IsRequired();
                }
                else
                {
                    row.Ignore(x => x.Meta);
                }
            });
    }
}

public static class ClippoModelBuilderExtensions
{
    public static ModelBuilder AddClippo<TMeta, TData>(this ModelBuilder model, Action<ClippoModelConfiguration<TMeta, TData>>? configure = null)
        where TMeta : class, ICloneable
        where TData : class, ICloneable
    {
        var configuration = new ClippoModelConfiguration<TMeta, TData>();
        configure?.Invoke(configuration);
        configuration.Apply(model);
        return model;
    }
}