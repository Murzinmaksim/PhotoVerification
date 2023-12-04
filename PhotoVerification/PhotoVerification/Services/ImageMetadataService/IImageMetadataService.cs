

namespace PhotoVerification.Services.ImageMetadataService
{
    public interface IImageMetadataService
    {
        string ProcessMetadata(IEnumerable<MetadataExtractor.Directory> directories);
    }
}
