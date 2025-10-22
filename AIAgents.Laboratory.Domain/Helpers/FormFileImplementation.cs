using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The Form File Implementation class.
/// </summary>
/// <param name="stream">The file stream.</param>
/// <param name="length">The file length</param>
/// <param name="fileName">The file name.</param>
/// <param name="name">The name.</param>
public class FormFileImplementation(Stream stream, long length, string fileName, string name) : IFormFile
{
	/// <summary>
	/// The content type.
	/// </summary>
	public string ContentType { get; set; } = "application/octet-stream";

	/// <summary>
	/// The content disposition.
	/// </summary>
	public string ContentDisposition => $"form-data; name=\"{name}\"; filename=\"{fileName}\"";

	/// <summary>
	/// The headers.
	/// </summary>
	public IHeaderDictionary Headers { get; set; } = new HeaderDictionary();

	/// <summary>
	/// The length of file.
	/// </summary>
	public long Length => length;

	/// <summary>
	/// The name.
	/// </summary>
	public string Name => name;

	/// <summary>
	/// The file name.
	/// </summary>
	public string FileName => fileName;

	/// <summary>
	/// The copy to function.
	/// </summary>
	/// <param name="target">The target file to copy to.</param>
	public void CopyTo(Stream target)
	{
		ArgumentNullException.ThrowIfNull(target);
		stream.CopyTo(target);
	}

	/// <summary>
	/// The copy to async function.
	/// </summary>
	/// <param name="target">The target file to copy to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task to wait on.</returns>
	public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(target);
		await stream.CopyToAsync(target, cancellationToken);
	}

	/// <summary>
	/// Opens the reading stream.
	/// </summary>
	/// <returns>The read stream.</returns>
	public Stream OpenReadStream()
	{
		return stream;
	}
}
