using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerAddress
{
	string m_IP;
	int m_Port;

	public string IP => m_IP;
	public int Port => m_Port;

	public ServerAddress(string ip, int port)
	{
		m_IP = ip;
		m_Port = port;
	}

	public override string ToString()
	{
		return $"{m_IP}:{m_Port}";
	}
}
