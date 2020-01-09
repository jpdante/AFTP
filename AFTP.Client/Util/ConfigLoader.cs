using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFTP.Client.Model;
using AFTP.Client.Model.Config;
using Newtonsoft.Json;

namespace AFTP.Client.Util {
    public class ConfigLoader {
        private readonly FileStream _fileStream;
        private readonly StreamReader _streamReader;
        private readonly StreamWriter _streamWriter;

        public ConfigLoader(string path) {
            var fileExists = File.Exists(path);
            _fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            _streamReader = new StreamReader(_fileStream);
            _streamWriter = new StreamWriter(_fileStream);
            if (fileExists) return;
            _streamWriter.Write(JsonConvert.SerializeObject(new AftpClientConfig {
                ConfigVersion = "1v",
                Servers = new List<ServerConfig>()
            }, Formatting.Indented));
            _streamWriter.Flush();
        }

        public AftpClientConfig LoadConfig() {
            return JsonConvert.DeserializeObject<AftpClientConfig>(_streamReader.ReadToEnd());
        }

        public void SaveConfig(AftpClientConfig config) {
            _fileStream.SetLength(0);
            _streamWriter.Write(JsonConvert.SerializeObject(config, Formatting.Indented));
            _streamWriter.Flush();
        }
    }
}
