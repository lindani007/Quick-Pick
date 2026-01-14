using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick_Employer.QuickPickEmployer.ViewModel
{
    public class ImageHelpers
    {
            public static async Task<byte[]?> ImageSourceToBytesAsync(ImageSource source, CancellationToken ct = default)
            {
                if (source == null) return null;

                // local file
                if (source is FileImageSource fileSource)
                {
                    var path = fileSource.File;
                    if (string.IsNullOrWhiteSpace(path)) return null;

                    // remote-file path guard
                    if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
                    {
                        using var client = new HttpClient();
                        return await client.GetByteArrayAsync(path).ConfigureAwait(false);
                    }

                    if (!File.Exists(path)) return null;
                    return await File.ReadAllBytesAsync(path, ct).ConfigureAwait(false);
                }

                // FromStream
                if (source is StreamImageSource streamSource)
                {
                    var streamFunc = streamSource.Stream;
                    if (streamFunc == null) return null;
                    using var stream = await streamFunc(ct).ConfigureAwait(false);
                    if (stream == null) return null;
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms, ct).ConfigureAwait(false);
                    return ms.ToArray();
                }

                // URI
                if (source is UriImageSource uriSource)
                {
                    var uri = uriSource.Uri;
                    if (uri == null) return null;
                    using var client = new HttpClient();
                    return await client.GetByteArrayAsync(uri).ConfigureAwait(false);
                }

                return null;
            }
    }
}
