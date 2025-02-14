using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IO.Swagger.Services;

public interface ICsvConvertor
{
    public IAsyncEnumerable<dynamic> Convert(string csvUri);
}

public class CsvConvertor : ICsvConvertor
{
    protected IHttpHandler _httpHandler;
    protected ICsvReaderWrapper _csvReaderWrapper;
    
    public CsvConvertor(IHttpHandler httpHandler, ICsvReaderWrapper csvReaderWrapper)
    {
        _httpHandler = httpHandler;
        _csvReaderWrapper = csvReaderWrapper;
    }
    
    
    public async IAsyncEnumerable<dynamic> Convert(string csvUri)
    {
        using (Stream stream = await _httpHandler.GetAsyncStream(csvUri))
        {
            using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                yield return _csvReaderWrapper.GetRecordsAsync<dynamic>(streamReader);
        }
    }
}