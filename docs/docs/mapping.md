---
title: Mapping
sidebar_position: 5
---

# Mapping

By default, controller will not map results and return same model.  
You can override this behavior to map to your model using two ways:  

1. Add your own implementation of `IClipDtoMapper<TClip, TClipDto>`
2. Add and register your own DTO model (might have public constructor with only one argument of type TClip)

## Mapper

```csharp
public class ClipDtoMapper<Attachment, AttachmentDto> : IClipDtoMapper<Attachment, AttachmentDto>
{
    public async Task<AttachmentDto[]> MapAsync([NotNull] IEnumerable<Attachment> clips)
    {
        ....
    }
}
```

```csharp
services
    .AddControllers()
    // ....
    .AddClippoJson()
    .AddClippoController<Attachment, AttachmentDto>() // DTO type specified as second type argument
    .Services
    ...
    .AddScoped<IClipDtoMapper<Attachment, AttachmentDto>, ClipDtoMapper<Attachment, AttachmentDto>>();
```

## Using public constructor

When you using default constructor, you define you can use your DTO model with public constructor.  
Clippo would pass Attachment instance to your public constructor.  

```csharp
public class AttachmentDto
{
    public AttachmentDto(Attachment attachment)
    {
        Id = attachment.AttachmentId;
        ....
    }
  
    public Guid Id { get; }
    
    ....
}
```

```csharp
services
    .AddControllers()
    // ....
    .AddClippoJson()
    .AddClippoController<Attachment, AttachmentDto>() // DTO type specified as second type argument
```