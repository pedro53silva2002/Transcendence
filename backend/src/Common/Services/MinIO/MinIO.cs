using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Security;
using System.Reactive.Linq;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Trippie.Common.Services.MinIO;

public interface IMinIOService
{
	Task<string> UploadFileAsync(string bucketName, string objectName, Stream fileStream, string contentType);
	Task<Stream> DownloadFileAsync(string bucketName, string objectName);
	Task DeleteFileAsync(string bucketName, string objectName);
	Task<bool> FileExistsAsync(string bucketName, string objectName);
	Task CreateBucketAsync(string bucketName);
	Task<string> GetObjectUrl(string bucketName, string objectName);
}

public class MinIOService : IMinIOService
{
    private readonly IMinioClient _minioClient;

    public MinIOService(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

	public async Task<string> GetObjectUrl(string bucketName, string objectName)
	{
		return await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
			.WithBucket(bucketName)
			.WithObject(objectName)
			.WithExpiry(60 * 60 * 24)); // 24 hours
	}

    public async Task<string> UploadFileAsync(string bucketName, string objectName, Stream fileStream, string contentType)
    {
        fileStream.Position = 0;

        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType));
	
		var profilePhotoPath = $"{bucketName}/{objectName}";
		return profilePhotoPath;
    }



    public async Task<Stream> DownloadFileAsync(string bucketName, string objectName)
    {
        var memoryStream = new MemoryStream();

        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            }));

        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<bool> FileExistsAsync(string bucketName, string objectName)
    {
        try
        {
            await _minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName));

            return true;
        }
        catch
        {
            return false;
        }
    }

	public async Task DeleteFileAsync(string bucketName, string objectName)
	{
		await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
			.WithBucket(bucketName)
			.WithObject(objectName));
	}

	public async Task CreateBucketAsync(string bucketName)
{
    bool found = await _minioClient.BucketExistsAsync(
        new BucketExistsArgs().WithBucket(bucketName));

    if (!found)
    {
        await _minioClient.MakeBucketAsync(
            new MakeBucketArgs().WithBucket(bucketName));

        var policy = $$"""
		{
		"Version":"2012-10-17",
		"Statement":[
			{
			"Effect":"Allow",
			"Principal":{"AWS":["*"]},
			"Action":["s3:GetObject"],
			"Resource":["arn:aws:s3:::{{bucketName}}/*"]
			}
		]
		}
		""";

        await _minioClient.SetPolicyAsync(
		new SetPolicyArgs()
			.WithBucket(bucketName)
			.WithPolicy(policy));
    }
}
}