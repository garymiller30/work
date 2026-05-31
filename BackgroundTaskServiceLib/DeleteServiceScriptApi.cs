namespace BackgroundTaskServiceLib
{
    public sealed class DeleteServiceScriptApi
    {
        public void Enqueue(string path)
        {
            DeferredDeleteService.Enqueue(path, DeferredDeleteMode.RecycleBin);
        }

        public void EnqueuePermanent(string path)
        {
            DeferredDeleteService.Enqueue(path, DeferredDeleteMode.Permanent);
        }
    }
}
