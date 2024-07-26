using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class WebsocketServer : MonoBehaviour
{
	static private WebSocket ws;
	static private HttpListener server;
	private async void Start()
	{
		await Run();
	}

	static async Task Run()
	{
		await ws.ReceiveAsync(new byte[1024], CancellationToken.None);
        //Httpリスナーを立ち上げ、クライアントからの接続を待つ
  //      server = new HttpListener();
		//server.Prefixes.Add("http://*:8000/");
		//server.Start();
		//var hc = await server.GetContextAsync();
		Debug.Log("リスナー開始");
		//Debug.Log(hc.Request.Url);

		//クライアントからのリクエストがWebSocketでない場合は処理を中断
		//if (!hc.Request.IsWebSocketRequest)
		//{
		//	//クライアント側にエラー(400)を返却し接続を閉じる
		//	Debug.Log("Websocketじゃない");
		//	hc.Response.StatusCode = 400;
		//	hc.Response.Close();
		//	return;
		//}

		//WebSocketでレスポンスを返却
		//var wsc = await hc.AcceptWebSocketAsync(null);
		//ws = wsc.WebSocket;
	
		//Debug.Log(wsc.RequestUri);
		//レスポンスのテストメッセージとして、現在時刻の文字列を取得
		var time = DateTime.Now.ToLongTimeString();

		//文字列をByte型に変換
		var buffer = Encoding.UTF8.GetBytes(time);
		var segment = new ArraySegment<byte>(buffer);

		//クライアント側に文字列を送信
		await ws.SendAsync(segment, WebSocketMessageType.Text,
			true, CancellationToken.None);
		
		
		//接続を閉じる
		ws.CloseAsync(WebSocketCloseStatus.NormalClosure,
			"Done", CancellationToken.None);
		
		await Run();
	}

	private void OnDestroy()
	{
		server.Stop();
	}
}
