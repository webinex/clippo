using Webinex.Coded;

namespace Webinex.Clippo
{
    public static class ClippoFailures
    {
        public static CodedFailure<string> AlreadyActive(string id) =>
            new(ClippoCodes.ALREADY_ACTIVE, id);

        public static CodedFailure<string> NoRef(string id) =>
            new(ClippoCodes.NO_REF, id);

        public static CodedFailure<( string file, object _ )> NoEmpty(string fileName) =>
            new(ClippoCodes.NO_EMPTY, (fileName, _: null));


        public static CodedFailure<( string file, long size )> Max(string fileName, long size) =>
            new(ClippoCodes.NO_EMPTY, (fileName, size));

        public static CodedFailure NotFound() => new(ClippoCodes.NOT_FOUND);
    }
}