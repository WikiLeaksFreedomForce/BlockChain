using System.Collections.Generic;
using BlockChain.Extensions;

namespace BlockChain.Headers {

    public class ExtensionFinder{

        public ExtensionFinder(){
            _extensions = new List<IExtensions>{
                new Png(),
                new Gif(),
                new Zip(),
                new Pdf1(),
                new Pdf2(),
                new Xls(),
                new Z7(),
                new Jpg(),
                new Doc(),
                new GZip(),
                new Tar(),
                new TarGz(),
                new Wikileaks()
            };
        }

        readonly List<IExtensions> _extensions;

        public string Find(byte[] data){

            foreach (var extension in _extensions) {
                if (data.Search(extension.Header) > -1){
                    return extension.Extension;
                }
            }

            return "dat";
        }
    }
}
