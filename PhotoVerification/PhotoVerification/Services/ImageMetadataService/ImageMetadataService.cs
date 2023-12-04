namespace PhotoVerification.Services.ImageMetadataService
{
    public class ImageMetadataService : IImageMetadataService
    {

        public string ProcessMetadata(IEnumerable<MetadataExtractor.Directory> directories)
        {
            //foreach (var directory in directories)
            //{
            //    foreach (var tag in directory.Tags)
            //    {
            //        Console.WriteLine($"{tag.Name} = {tag.Description}");
            //    }
            //}

            var edited = Enum.GetNames(typeof(EditedSoftware))
               .Any(edited => directories
               .SelectMany(directory => directory.Tags)
               .Any(tag => tag.Description.Contains(edited, StringComparison.OrdinalIgnoreCase)));

            if (edited)
            {
                return "был отредактирован";
            }
            return "не был отредактирован.";
        }  
    }
}
