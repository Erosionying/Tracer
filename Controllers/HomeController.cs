﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tracer.Attributes;
using System.Net.WebSockets;
using Tracer.Models;
using Aiursoft.Pylon.Attributes;

namespace Tracer.Controllers
{
    public class HomeController : Controller
    {
        private static byte[] _data;
        private const int _length = 1024 * 1024 * 1;
        private static byte[] GetData()
        {
            if (_data == null)
            {
                _data = new byte[_length];
                for (int i = 0; i < _length; i++)
                {
                    _data[i] = 1;
                }
            }
            return _data;
        }
        private static JsonResult _message = null;

        private IPusher<WebSocket> _pusher;
        public HomeController()
        {
            _pusher = new WebSocketPusher();
        }


        public IActionResult Index()
        {
            return View();
        }

        [AiurNoCache]
        [AiurForceWebSocket]
        public async Task<IActionResult> Pushing()
        {
            await _pusher.Accept(HttpContext);
            while (_pusher.Connected)
            {
                try
                {
                    await _pusher.SendMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff"));
                    await Task.Delay(10);
                }
                catch
                {
                    break;
                }
            }
            return null;
        }

        [AiurNoCache]
        public IActionResult Ping()
        {
            if (_message == null)
            {
                _message = Json(new { message = "ok" });
            }
            return _message;
        }

        [AiurNoCache]
        public IActionResult Download()
        {
            HttpContext.Response.Headers.Add("Content-Length", _length.ToString());
            return new FileContentResult(GetData(), "application/octet-stream");
        }
    }
}
