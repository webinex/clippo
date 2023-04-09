---
title: Model Definition
sidebar_position: 1
---

# Model Definition

Model definition needed to construct dynamic queries to database and operate your model.

```csharp
public class AttachmentModelDefinitionConfiguration : ClippoModelDefinitionConfiguration<Attachment>
{
    protected override void Configure(ClipModelBuilder<Attachment> model)
    {
        model
            .HasId(x => x.Id)
            .HasOwnerType(x => x.OwnerType)
            .HasOwnerId(x => x.OwnerId)
            .HasDirectory(x => x.Directory)
            .HasActive(x => x.Active)
            .HasSizeBytes(x => x.SizeBytes)
            .HasFileName(x => x.FileName)
            .HasMimeType(x => x.MimeType)
            .HasReference(x => x.Reference)
            .HasNew((values) => new Attachment(values))
            .HasSetValues((attachment, values) => attachment.SetValues(values));
    }
}
```

Registration:

```csharp
services.AddClippo<Attachment>(x => x
    .AddModelDefinitionConfiguration<AttachmentModelDefinitionConfiguration>();
```

If you have simple `Clip` implementation, you can use `.UseDefaultConstructor()` instead of `HasNew` and `HasSetValues` in this case, your entity might have public parameterless constructor or constructor with single `IDictionary<string, object>` parameter.  

When you using parameterless constructor, after creating new instance, `HasSetValues` delegate would be called (if specified).   

```csharp
public class Attachment {
  public Attachment() {
    // ...
  }
}
```

or 

```csharp
public class Attachment {
  public Attachment([NotNull] IDictionary<string, object> values) {
    // ...
  }
}
```

When you need scoped services, you might define `IClippoModelDefinition<TClip>`:

```csharp
public interface IClippoModelDefinition<TClip>
{
    Expression<Func<TClip, object>> Id { get; }
    Expression<Func<TClip, string>> OwnerType { get; }
    Expression<Func<TClip, string>> OwnerId { get; }
    Expression<Func<TClip, string>> Directory { get; }
    Expression<Func<TClip, bool>> Active { get; }
    Expression<Func<TClip, int>> SizeBytes { get; }
    Expression<Func<TClip, string>> FileName { get; }
    Expression<Func<TClip, string>> MimeType { get; }
    Expression<Func<TClip, string>> Reference { get; }
    TClip New(IDictionary<string, object> values);
    void SetValues(TClip clip, IDictionary<string, object> values);
}
```

And register it, it would be registered as scoped service, so you can inject scoped services

```csharp
services.AddClippo<Attachment>(x => x
    .AddModelDefinition<AttachmentModelDefinition>();
```