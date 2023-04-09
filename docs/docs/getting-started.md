---
title: Getting Started
sidebar_position: 2
---

# Getting Started

## Install Nuget package

```
dotnet add package Webinex.Wispo
```

## Create a model

It can be actually any model, names of properties are not regulated.
As well, you can add additional properties to model.  

:::caution Public Getters and Setters
All fields except `Id` might have public getters and setters
:::

:::info EF Core
When you using EFCore, `Id`, `OwnerType`, `OwnerId`, `Directory` and `Active` might be
plain properties, because they would be used to construct queries to database.
:::

```csharp
public class Attachment
{
    public Guid Id { get; set; }
    public string OwnerType { get; set; }
    public string OwnerId { get; set; }
    public string Directory { get; set; }
    public bool Active { get; set; }
    public string FileName { get; set; }
    public string MimeType { get; set; }
    public int SizeBytes { get; set; }
    public string Reference { get; set; }
}
```

## Create a model definition

Model definition needed to construct dynamic queries to database and operate your model.
When you using `.UseDefaultConstructor()` your entity might have public parameterless constructor
or constructor with single `IDictionary<string, object>` parameter.  

See [Advanced Guide](/docs/advanced-guides/model-definition) for additional details.  

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
            .UseDefaultConstructor();
    }
}
```

## EFCore usage

Add `IClippoDbContext<Attachment>` to your DbContext

```csharp
internal class MyDbContext : DbContext, IClippoDbContext<Attachment>
{
    public DbSet<Attachment> Clips { get; set; }
}
```

## Register services

```csharp
services.AddClippo<Attachment>(x => x
    .AddDbContext<MyDbContext>()
    .AddFileSystemStore("D://your-files-folder")
    .AddModelDefinitionConfiguration<AttachmentModelDefinitionConfiguration>()
    .Interceptors
    .AddDbContextSaveChanges());
```

## Add controller

It would add controller used by frontend library on `/api/clippo`.

```csharp
services
    .AddControllers()
    // ....
    .AddClippoJson() // Adds built-in actions mapping
    .AddClippoController<Attachment>() // Default controller registration
```

## Usage

### Client side

Defined global client

```jsx typescript
import axios from 'axios';
import { useClippo, ClippoOptions } from '@webinex/clippo';

export interface AttachmentDto {
  id: string;
  ownerType?: string;
  ownerId?: string;
  directory?: string;
  active: boolean;
  fileName: string;
  mimeType: string;
  sizeBytes: number;
  reference: string;
}

const axiosInstance = axios.create({ baseURL: '/api/clippo' });

axiosInstance.interceptors.request.use(async (request) => {
  request.headers['authorization'] = ... get your access token;
  return request;
});

export function useAppClippo(options: ClippoOptions<AttachmentDto>) {
  return useClippo({ axios: axiosInstance, ...options });
}
```

Use your clippo client

```jsx typescript
import { useDropzone } from 'react-dropzone';
import { useAppClippo } from './clippoClient';

function MyComponent({ id }: { id: string }) {
  const {
    fetch,
    store,
    getById,
    content,
    apply,
    open,
    remove,
    stage,
    value,
    setValue,
    update,
    originalItems,
  } = useAppClippo({
    ownerId: id,
    ownerType: 'OwnerType',
  });

  const {
    getRootProps,
    getInputProps,
    open: openDialog,
  } = useDropzone({
    onDrop: store,
    noClick: true,
  });

  return (
    <div {...getRootProps()}>
      <input {...getInputProps()} />

      <div>
        Drop files or click <button onClick={openDialog}>Open</button> to upload files
      </div>

      <ul>
        {value.items.map((item) => (
          <li key={item.id}>
            {item.fileName} <button onClick={() => remove(item.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  )
}
```

If you want changes to be applied only with save of "container" entity, you can use _defer_ mode.
In this case, uploaded entities would not be linked to owner, removes would not be called to server.
Instead, all this actions would be saved to `value.actions` and you can use it when would save your entity.

```jsx typescript
const {
	...
} = useAppClippo({
	ownerId: id,
	ownerType: 'OwnerType',
	defer: true
});
```

### Server side

When you using clippo in _defered_ mode

```csharp
public class YourBusinessService
{
    private readonly IClippo<Attachment> _clippo;

    public async Task SaveAsync(BusinessEntity entity)
    {
        await _clippo.ApplyAsync(entity.Actions);
    }
}
```

For actions, which was created for draft entity (entity existing on client side only, not saved previously to database),
you might call `ApplyAsync` extension method, it would automatically update `ActivateClipAction`s with supplied parameters.  

```csharp
await _clippo.ApplyAsync(entity.Actions, ownerType, ownerId, directory);
```
