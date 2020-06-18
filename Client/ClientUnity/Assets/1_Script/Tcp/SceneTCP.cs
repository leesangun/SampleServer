using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTCP : MonoBehaviour {
	private ClientTCP _clientTCP;
	
	void Start () {
		_clientTCP = ClientTCP.getInstance((string data)=>{
			print(data);
		});
	}
	
	public void ClickSend(){
		_clientTCP.write("aaa");
	}
}
