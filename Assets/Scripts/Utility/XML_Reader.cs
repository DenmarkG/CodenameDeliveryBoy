using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO; 

public class XML_Reader 
{
	private TextAsset m_dialogFile = Resources.Load("Dialog/DialogTest") as TextAsset;
//	XmlDocument root = new XmlDocument();

	private List<string> m_loadedLines = new List<string>();

	//	private static FileStream m_fileStream = new FileStream("Resources/Dialog/DialogTest.xml", FileMode.Open);
	public List<string> LoadDialog()
	{
		if (m_dialogFile != null)
		{
			//XmlReader reader = new XmlReader(new StringReader(m_dialogFile.text) );
//			while(reader.Read() )
//			{
//				//[#todo] finish this implemenation
//			}
		}
		return m_loadedLines;
	}
}
