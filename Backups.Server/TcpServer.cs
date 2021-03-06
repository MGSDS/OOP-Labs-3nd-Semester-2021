using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Backups.NetworkTransfer.Entities;
using Backups.NetworkTransfer.Headers;
using Backups.Server.Repositories;
namespace Backups.Server
{
    public class TcpServer
    {
        private TcpListener _tcpListener;
        private IServerRepository _serverRepository;
        private NetworkStream _stream;
        private TcpClient _client;

        public TcpServer(ushort port, IServerRepository serverRepository)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _serverRepository = serverRepository;
        }

        public void Start()
        {
            _tcpListener.Start();
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }

        public void Read()
        {
            if (!_tcpListener.Server.IsBound)
                throw new InvalidOperationException("Server is not started");
            _client = _tcpListener.AcceptTcpClient();
            _stream = _client.GetStream();
            ReceiveFiles();
        }

        private TransferFile ReceiveSingleFile()
        {
            byte[] headerBytes = GetHeader();
            var header = new FileHeader(headerBytes);
            var stream = new MemoryStream();
            byte[] fileBytes = new byte[header.GetSize()];
            ReadStream(fileBytes);
            stream.Write(fileBytes, 0, header.GetSize());
            return new TransferFile(header.GetName(), stream);
        }

        private void ReceiveFiles()
        {
            var header = new FolderHeader(GetHeader());
            int filesCount = header.GetFilesCount();
            var files = new List<TransferFile>(filesCount);
            for (int i = 0; i < filesCount; i++)
                files.Add(ReceiveSingleFile());
            _serverRepository.Save(files, header.GetFolderName());
            }

        private byte[] GetHeader()
        {
            byte[] headerSizeBytes = new byte[4];
            ReadStream(headerSizeBytes);
            int headerSize = BitConverter.ToInt32(headerSizeBytes);
            byte[] headerBytes = new byte[headerSize];
            ReadStream(headerBytes);
            return headerBytes;
        }

        private void ReadStream(byte[] data)
        {
            int offset = 0;
            int remaining = data.Length;
            while (remaining > 0)
            {
                int read = _stream.Read(data, offset, remaining);
                if (read <= 0)
                    throw new EndOfStreamException($"End of stream reached with {remaining} bytes left to read");

                remaining -= read;
                offset += read;
            }
        }
    }
}